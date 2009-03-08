using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Acrossoft.GoUtils.Entities
{
    public class Group
    {
        private readonly List<Point> m_positions ;

        public Group()
        {
            m_positions = new List<Point>();
        }

        public Point At(int i)
        {
            return m_positions[i] ;
        }

        public int Count
        {
            get { return m_positions.Count; }
        }

        public static Group operator +(Group g, Point p)
        {
            Group res = new Group();
            res.m_positions.InsertRange(0, g.m_positions);
            res.m_positions.Insert(res.Count, p);
            return res;
        }
        public static Group operator +(Group l, Group r)
        {
            Group res = new Group();
            res.m_positions.InsertRange(0, l.m_positions);
            res.m_positions.InsertRange(res.Count, r.m_positions);
            return res;
        }
    }
}
