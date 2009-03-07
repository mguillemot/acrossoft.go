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
    }
}
