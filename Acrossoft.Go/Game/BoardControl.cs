using Acrossoft.GoUtils.Entities;
using Microsoft.Xna.Framework;

namespace Acrossoft.Go.Game
{
    public class BoardControl
    {
        private readonly Board m_board;
        private readonly BoardEx m_boardex;

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
    }
}
