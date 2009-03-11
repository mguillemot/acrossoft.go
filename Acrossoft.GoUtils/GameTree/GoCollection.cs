using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acrossoft.GoUtils.GameTree
{
    class GoCollection
    {
        public readonly List<GoEvent> m_events;

        public GoCollection()
        {
            m_events = new List<GoEvent>();
        }
    }
}
