using System.Collections;
using System.Collections.Generic;

namespace Acrossoft.GoUtils.Utils
{
    public class HashSet<T> : IEnumerable<T>
    {
        private readonly Dictionary<T, bool> m_content = new Dictionary<T, bool>();

        public void Remove(T value)
        {
            m_content.Remove(value);
        }

        public bool Contains(T value)
        {
            return m_content.ContainsKey(value);
        }

        public void Add(T value)
        {
            if (!Contains(value))
            {
                m_content.Add(value, false);
            }
        }

        public void Clear()
        {
            m_content.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_content.Keys.GetEnumerator();
        }

        public int Count
        {
            get { return m_content.Count; }
        }
    }
}
