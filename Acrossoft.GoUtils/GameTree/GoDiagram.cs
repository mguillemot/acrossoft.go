using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acrossoft.GoUtils.GameTree
{
    class GoDiagram
    {
        private List<string> m_remarks;
        private int m_setup;
        private int m_hide;
        private List<string> m_comments;
        private List<GoMark> m_marks;
        private int m_userdata;

        public GoDiagram()
        {
            m_remarks = new List<string>();
            m_setup = 0;
            m_hide = 0;
            m_comments = new List<string>();
            m_marks = new List<GoMark>();
            m_userdata = 0;
        }
    }
}
