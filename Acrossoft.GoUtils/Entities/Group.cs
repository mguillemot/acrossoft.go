using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Acrossoft.GoUtils.Entities
{
    public class Group : IEnumerable<Point>
    {
        private readonly List<Point> m_positions ;

        public Group()
        {
            m_positions = new List<Point>();
        }

        public object Clone()
        {
            var clone = new Group();
            foreach (var p in m_positions)
            {
                clone.m_positions.Add(new Point(p.X, p.Y));
            }
            return clone;
        }

        public void Clear()
        {
           m_positions.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return GetEnumerator();
        }

        public IEnumerator<Point> GetEnumerator()
        {
           return m_positions.GetEnumerator();
        }

        public int Count
        {
            get { return m_positions.Count; }
        }

        public Point this[int i]
        {
            get { return m_positions[i]; }
            set { m_positions[i] = value; }
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
