using Microsoft.Xna.Framework;

namespace Acrossoft.GoUtils.Entities
{

    public enum Dir
    {
            LEFT, UP, RIGHT, DOWN
    }

    class Op
    {
        public static Point Translate(Point p, Dir dir)
        {
            int x = (dir == Dir.LEFT) ? -1 : (dir == Dir.RIGHT) ? 1 : 0;
            int y = (dir == Dir.UP) ? -1 : (dir == Dir.DOWN) ? 1 : 0;
            return new Point(p.X+x, p.Y+y);
        }
    }

 /*   public class Point
    {
        private readonly int m_x ;
        private readonly int m_y ;

        public enum Direction
        {
            LEFT, UP, RIGHT, DOWN
        }

        public Point(int x, int y)
        {
            m_x = x;
            m_y = y;
        }

        public Point(Point p)
        {
            m_x = p.m_x;
            m_y = p.m_y;
        }
        
        public Point(Direction dir)
        {
            m_x = (dir == Direction.LEFT) ? -1 : (dir == Direction.RIGHT) ? 1 : 0;
            m_y = (dir == Direction.UP) ? -1 : (dir == Direction.DOWN) ? 1 : 0;
        }

        public static Point operator+(Point a, Point b)
        {
            return new Point(a.m_x + b.m_x, a.m_y + b.m_y);
        }

        public int X
        {
            get { return m_x; }
        }

        public int Y
        {
            get { return m_y; }
        }
    }*/
 }
