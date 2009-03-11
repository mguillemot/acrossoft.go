using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acrossoft.GoUtils.Entities;
using Microsoft.Xna.Framework;

namespace Acrossoft.GoUtils.GameTree
{
    class GoMove
    {
        private List<string> m_remarks;
        private Stone m_move; // B, W or SETUP (NONE)
        private List<Point> m_prisoners;
        private List<string> m_comments;
        private List<GoMark> m_marks;
        //private int m_userdata;
        private List<GoMove> m_variations;

        public GoMove()
        {
            m_remarks = new List<string>();
            m_move = Stone.NONE;
            m_comments = new List<string>();
            m_marks = new List<GoMark>();
            m_variations = new List<GoMove>();
        }
    }
}
