namespace Acrossoft.Go.Game
{
    public class BoardControl
    {
        private readonly Board m_board;

        public BoardControl(Board board)
        {
            m_board = board;
        }

        public Board Board
        {
            get { return m_board; }
        }

        public bool CanPlay(int x, int y, Stone stone)
        {
            if (!m_board.InBoard(x, y))
            {
                return false;
            }
            if (m_board.Get(x, y) != Stone.NONE)
            {
                return false;
            }
            return true;
        }

        public void Play(int x, int y, Stone stone)
        {
            if (!CanPlay(x, y, stone))
            {
                return;
            }
            m_board.Set(x, y, stone);
        }
    }
}
