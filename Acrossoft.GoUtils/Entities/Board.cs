﻿namespace Acrossoft.GoUtils.Entities
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

        public Board(Board board)
        {
            m_size = board.m_size;
            m_content = new Stone[m_size][];
            for (int i = 0; i < m_size; i++)
            {
                m_content[i] = new Stone[m_size];
                for (int j = 0; j < m_size; j++)
                {
                    m_content[i][j] = board.m_content[i][j];
                }
            }
        }

        public int Size
        {
            get { return m_size; }
        }

        public bool InBoard(int x, int y)
        {
            return 0 <= x && x <= m_size-1 && 0 <= y && y <= m_size-1;
        }

        public Stone Get(int x, int y)
        {
            return m_content[x][y];
        }

        public void Set(int x, int y, Stone value)
        {
            m_content[x][y] = value;
        }

    }
}
