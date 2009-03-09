using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Acrossoft.GoUtils.Entities;

namespace Acrossoft.GoAi
{
    public class Ai
    {
        private readonly Random m_randomgenerator;
        private BoardEx m_boardex;

        public Ai(int size)
        {
            m_boardex = new BoardEx(size);
            m_randomgenerator = new Random();
        }

        public void Set(BoardEx boardex)
        {
            m_boardex = (BoardEx)boardex.Clone();
        }

        // (-1,0) for pass. (0,-1) for resign.
        public Point BestMove()
        {
            List<Point> legalmoves = new List<Point>();
            for (int i = 0; i < m_boardex.Size; ++i)
            {
                for (int j = 0; j < m_boardex.Size; ++j)
                {
                    Point p = new Point(i,j) ;
                    if (m_boardex.LegalMove(p))
                    {
                        legalmoves.Add(p);
                    }
                }
            }

            Point best;
            if (legalmoves.Count == 0)
            {
                best = new Point(-1, 0); // pass
            }
            else
            {
                best = legalmoves[m_randomgenerator.Next(0, legalmoves.Count)];
            }

            return best;
        }

        public void Pass()
        {
            m_boardex.Pass();
        }

        public void Move(Point p)
        {
            m_boardex.Move(p);
        }
    }
}
