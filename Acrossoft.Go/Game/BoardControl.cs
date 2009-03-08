using Acrossoft.GoUtils.Entities;
using Microsoft.Xna.Framework;

namespace Acrossoft.Go.Game
{
    public class BoardControl
    {
        private readonly Board m_board;
        private readonly BoardEx m_boardex;

        private Board m_boardcopy; // debug display

        public BoardControl(Board board)
        {
            m_board = board;
            m_boardex = new BoardEx(m_board);
        }

        public Board Board
        {
            get { return m_board; }
        }

        public bool CanPlay(Point p, Stone stone)
        {
            return m_boardex.LegalMove(p, stone);
        }

        public void Play(Point p, Stone stone)
        {
            if (!CanPlay(p, stone))
            {
                return;
            }
            m_boardex.Move(p, stone);
        }

        public void HighlightGroup(int id)
        {
            m_boardcopy = new Board(m_board);
            for (int i = 0; i < m_board.Size; i++)
            {
                for (int j = 0; j < m_board.Size; j++)
                {
                    if (m_boardex.GetGroupIndex(new Point(i, j)) != id)
                        m_board.Set(i, j, Stone.NONE);
                }
            }
        }

        public void RestoreBoard()
        {
            for (int i = 0; i < m_board.Size; i++)
            {
                for (int j = 0; j < m_board.Size; j++)
                {
                    m_board.Set(i, j, m_boardcopy.Get(i,j));
                }
            }
        }
    }
}
