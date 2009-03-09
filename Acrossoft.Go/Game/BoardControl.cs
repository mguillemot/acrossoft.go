using Acrossoft.GoUtils.Entities;
using Acrossoft.GoAi;
using Microsoft.Xna.Framework;

namespace Acrossoft.Go.Game
{
    public class BoardControl
    {
        private readonly Board m_board;
        private readonly BoardEx m_boardex;
        private readonly Ai m_ai;

        private Board m_boardcopy; // debug display

        public BoardControl(Board board)
        {
            m_board = board;
            m_boardex = new BoardEx(m_board);
            m_ai = new Ai(board.Size);
            m_ai.Set(m_boardex);
        }

        public Board Board
        {
            get { return m_board; }
        }

        public bool MyTurnToPlay(Stone color)
        {
            return (m_boardex.Color == color);
        }

        public bool CanPlay(Point p)
        {
            return m_boardex.LegalMove(p);
        }

        public void PlayAi()
        {
            Point best = m_ai.BestMove();
            bool pass = false;
            if (!CanPlay(best))
            {
                pass = true;
            }
            if (!pass)
            {
                if (!Play(best))
                {
                    pass = true;
                }
            }
            if (pass)
            {
                Pass();
            }
        }

        public bool Play(Point p)
        {
            bool played = false;
            if (CanPlay(p))
            {
                if (m_boardex.Move(p))
                {
                    m_ai.Move(p);
                    played = true;
                }
            }
            return played;
        }

        public void Pass()
        {
            m_boardex.Pass();
            m_ai.Pass();
        }

        public void HighlightGroup(int id)
        {
            m_boardcopy = (Board) m_board.Clone();
            for (int i = 0; i < m_board.Size; i++)
            {
                for (int j = 0; j < m_board.Size; j++)
                {
                    if (m_boardex.GetGroupIndex(new Point(i, j)) != id)
                        m_board[i, j] = Stone.NONE;
                }
            }
        }

        public void RestoreBoard()
        {
            for (int i = 0; i < m_board.Size; i++)
            {
                for (int j = 0; j < m_board.Size; j++)
                {
                    m_board[i, j] = m_boardcopy[i, j];
                }
            }
        }
    }
}
