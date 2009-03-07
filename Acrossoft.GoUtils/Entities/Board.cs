namespace Acrossoft.GoUtils
{
    public class Board
    {
        private readonly int m_size;
        private readonly Stone[][] m_content;

        public Board(int size)
        {
            m_size = size;
            m_content = new Stone[size][];
            for (int i = 0; i < size; i++)
            {
                m_content[i] = new Stone[size];
                for (int j = 0; j < size; j++)
                {
                    m_content[i][j] = Stone.NONE;
                }
            }
        }

        public int Size
        {
            get { return m_size; }
        }

        public bool InBoard(int x, int y)
        {
            return 1 <= x && x <= m_size && 1 <= y && y <= m_size;
        }

        public Stone Get(int x, int y)
        {
            return m_content[x - 1][y - 1];
        }

        public void Set(int x, int y, Stone value)
        {
            m_content[x - 1][y - 1] = value;
        }
    }
}
