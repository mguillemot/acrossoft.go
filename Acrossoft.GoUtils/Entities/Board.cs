using System;

namespace Acrossoft.GoUtils.Entities
{
    public class Board : ICloneable
    {
        private readonly int m_size;
        private readonly Stone[,] m_content;

        public Board(int size)
        {
            m_size = size;
            m_content = new Stone[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    m_content[i, j] = Stone.NONE;
                }
            }
        }

        public object Clone()
        {
            var clone = new Board(Size);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    clone[i, j] = this[i, j];
                }
            }
            return clone;
        }

        public int Size
        {
            get { return m_size; }
        }

        public bool InBoard(int x, int y)
        {
            return 0 <= x && x <= m_size-1 && 0 <= y && y <= m_size-1;
        }

        public Stone this[int x, int y]
        {
            get { return m_content[x, y]; }
            set { m_content[x, y] = value; }
        }
    }
}
