using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Acrossoft.GoUtils.Entities
{

    // extended board representation
    class BoardEx
    {
        private readonly Board m_board;
        private readonly int[][] m_groupmap;
        private readonly List<Group> m_grouplist;
        private readonly List<Point> m_hotpoints; // indicates hot points (ex simple ko)
        private readonly int m_npassinarow; // number of consecutive pass
        private readonly int m_nbcapture;
        private readonly int m_nwcapture;
        private readonly int m_nmove;

        public BoardEx(int size)
        {
            m_board = new Board(size);
            m_groupmap = new int[size][];
            for (int i = 0; i < size; i++)
            {
                m_groupmap[i] = new int[size];
                for (int j = 0; j < size; j++)
                {
                    m_groupmap[i][j] = -1;
                }
            }
            m_grouplist = new List<Group>();
            m_hotpoints = new List<Point>();
            m_npassinarow = 0;
            m_nbcapture = 0;
            m_nwcapture = 0;
            m_nmove = 0;
        }

        // access to board members

        public int Size
        {
            get { return m_board.Size; }
        }

        public bool InBoard(int x, int y)
        {
            return m_board.InBoard(x,y);
        }

        public bool InBoard(Point p)
        {
            return m_board.InBoard(p.X, p.Y);
        }

        public Stone Get(int x, int y)
        {
            return m_board.Get(x,y);
        }

        public Stone Get(Point p)
        {
            return m_board.Get(p.X, p.Y);
        }

 /*     public void Set(int x, int y, Stone value)
        {
            m_board.Set(x, y, value);
        }

        public Board Convert()
        {
            return m_board;
        }*/


        // get some advanced properties

        public int CaptureCount(Stone color)
        {
            return (color == Stone.BLACK) ? m_nbcapture : m_nwcapture;
        }

        public int MoveCount
        {
            get { return m_nmove ; }
        }

        public int PassCount
        {
            get { return m_npassinarow; }
        }

        private int GetGroupIndex(Point p)
        {
            return m_groupmap[p.X][p.Y];
        }

        public Group GetGroup(Point p)
        {
            int idx = GetGroupIndex(p);
            return (idx == -1) ? null : m_grouplist[idx];
        }


        // liberties

        private int GetLocalLibertyCount(Point p)
        {
            int libcount = 0;
            for (Dir d = Dir.LEFT; d <= Dir.DOWN; ++d)
            {
                if (Get(Op.Translate(p, d)) == Stone.NONE) ++libcount;
            }
            return libcount;
        }

        // Assume that group is not empty
        public int GetLibertyCount(Group group)
        {
            //int id = GetGroupIndex(group.At(0));
            HashSet<Point> libset = new HashSet<Point>() ; // en esperant ne pas comparer les pointeurs ^^
            for (int i = 0; i < group.Count; ++i)
            {
                Point p0 = group.At(i);
                for (Dir d = Dir.LEFT; d <= Dir.DOWN; ++d)
                {
                    Point p = Op.Translate(p0, d);
                    if (Get(p) == Stone.NONE)
                    {
                        libset.Add(p);
                    }
                }
            }
            return libset.Count;
        }

        // Assume p is empty (else, meaningless)
        public bool SuicidePlay(Point p, Stone color)
        {
            bool res = true;
            if (GetLocalLibertyCount(p) == 0)
            {
                // the position is surrounded by stones
                for (Dir d = Dir.LEFT; d <= Dir.DOWN; ++d)
                {
                    Point p1 = Op.Translate(p, d);
                    if (Get(p1) == color)
                    {
                        // the stone is connecting to a group
                        if (GetLibertyCount(GetGroup(p1)) > 1)
                        {
                            // the new formed group will have at least 1 liberty.
                            res = false;
                            break;
                        }
                    }
                    else // opponent color)
                    {
                        // the stone is touching an opponent group
                        if (GetLibertyCount(GetGroup(p1)) <= 1)
                        {
                            // the opponent group liberties will be reduced to 0 (captured).
                            res = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                // there is an empty point next to the played stone.
                res = false;
            }
            return res;
        }

        public bool HotPoint(Point p)
        {
            bool res = false;
            for (int i = 0; i < m_hotpoints.Count; ++i)
            {
                if (m_hotpoints[i] == p)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        // japanese rules, ko rule depending on hot points.
        public bool LegalMove(Point p, Stone color)
        {
            return InBoard(p) && (Get(p) == Stone.NONE) && !HotPoint(p) && !SuicidePlay(p, color) ;             
        }

        // determining hot points (for simple ko only)
        // when putting a stone:
        //  - remove precedent hot point. (m_hotpoints.Clear())
        //  - if the move kills a lone stone at position p,
        //    and the killing stone becomes a lone stone, then p becomes a hot point. (m_hotpoints.Add(p))
    }
}
