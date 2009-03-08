using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Acrossoft.GoUtils.Entities
{

    // extended board representation
    public class BoardEx
    {
        private readonly Board m_board;
        private readonly int[][] m_groupmap;
        private readonly List<Group> m_grouplist;
        private readonly List<Point> m_hotpoints; // indicates hot points (ex simple ko)
        private int m_npassinarow; // number of consecutive pass
        private int m_nbcapture;
        private int m_nwcapture;
        private int m_nmove;

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

        public BoardEx(Board board)
        {
            m_board = board;
            m_groupmap = new int[Size][];
            for (int i = 0; i < Size; i++)
            {
                m_groupmap[i] = new int[Size];
                for (int j = 0; j < Size; j++)
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

        // -- access to board members -- //

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
            return m_board[x, y];
        }

        public Stone Get(Point p)
        {
            return m_board[p.X, p.Y];
        }


        // -- get some advanced properties -- //
 
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

        public int GetGroupIndex(Point p)
        {
            return m_groupmap[p.X][p.Y];
        }

        public Group GetGroup(Point p)
        {
            int idx = GetGroupIndex(p);
            return (idx == -1) ? null : m_grouplist[idx];
        }


        // -- liberties/capture/suicide -- //

        // get the number of neighbor empty point
        private int GetLocalLibertyCount(Point p)
        {
            int libcount = 0;
            for (Dir d = Dir.LEFT; d <= Dir.DOWN; ++d)
            {
                Point p1 = Op.Translate(p, d);
                if (InBoard(p1) && (Get(p1) == Stone.NONE))
                {
                    ++libcount;
                }
            }
            return libcount;
        }

        // Assuming that 'group' is not empty
        public int GetLibertyCount(Group group)
        {
            //int id = GetGroupIndex(group.At(0));
            Utils.HashSet<Point> libset = new Utils.HashSet<Point>(); // en esperant ne pas comparer les pointeurs ^^
            for (int i = 0; i < group.Count; ++i)
            {
                Point p = group.At(i);
                for (Dir d = Dir.LEFT; d <= Dir.DOWN; ++d)
                {
                    Point p1 = Op.Translate(p, d);
                    if (InBoard(p1) && (Get(p1) == Stone.NONE))
                    {
                        libset.Add(p1);
                    }
                }
            }
            return libset.Count;
        }

        // Assuming p is empty (else, meaningless)
        public bool SuicidePlay(Point p, Stone color)
        {
            bool res = true;
            if (GetLocalLibertyCount(p) == 0)
            {
                // the position is surrounded by stones
                for (Dir d = Dir.LEFT; d <= Dir.DOWN; ++d)
                {
                    Point p1 = Op.Translate(p, d);
                    if (InBoard(p1))
                    {
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
                        else // opponent color
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
            }
            else
            {
                // there is an empty point next to the played stone.
                res = false;
            }
            return res;
        }

        // check if p is a hot point
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

        // check if legal move under japanese rules, ko rule depending on hot points.
        public bool LegalMove(Point p, Stone color)
        {
            return InBoard(p) && (Get(p) == Stone.NONE) && !HotPoint(p) && !SuicidePlay(p, color) ;             
        }


        // Note d'Arl:
        // determining hot points (for simple ko only)
        // when putting a stone:
        //  - remove precedent hot point. (m_hotpoints.Clear())
        //  - if the move kills a lone stone at position p,
        //    and the killing stone becomes a lone stone, then p becomes a hot point. (m_hotpoints.Add(p))


        // play any playable move (even illegal)
        public void Move(Point p, Stone color)
        {
            if (!InBoard(p) || Get(p)!=Stone.NONE)
            {
                // not only illegal, but impossible moves
                return;
            }

            ++m_nmove;

            //update hotpoints (1) remove old
            m_hotpoints.Clear();
            bool kopossible = true;

            //put the stone, as a new lone stone group
            m_board[p.X, p.Y] = color;
            Group newgroup = new Group() ;
            newgroup += p;
            int id = m_grouplist.Count;
            m_grouplist.Add(newgroup);
            m_groupmap[p.X][p.Y] = id;

            // list groups touched
            Utils.HashSet<Group> touchedgroups = new Utils.HashSet<Group>();
            for (Dir d = Dir.LEFT; d <= Dir.DOWN; ++d)
            {
                Point p1 = Op.Translate(p, d);
                if (InBoard(p1) && (Get(p1) != Stone.NONE))
                {
                    touchedgroups.Add(GetGroup(p1)) ;
                }
            }

            List<Group> opponents = new List<Group>();
            List<Group> friends = new List<Group>();
            foreach (var g in touchedgroups)
            {
                if (g.Count > 0 && (Get(g.At(0)) == color))
                    friends.Add(g);
                else
                    opponents.Add(g);
            }

            // check liberties of opponent groups and remove if captured (ajusting capture count)
            int capturecount = 0; // simpleko test
            for (int i = 0; i < opponents.Count; ++i)
            {
                Group g = opponents[i];
                if (GetLibertyCount(g) == 0)
                {
                    capturecount += g.Count; // simpleko test
                    if (color == Stone.BLACK) m_nbcapture += g.Count;
                    else m_nwcapture -= g.Count;
                    Point p0 = g.At(0);
                    RemoveGroup(m_groupmap[p0.X][p0.Y]);
                }
            }

            if (capturecount != 1) kopossible = false; // simpleko test

            id = m_grouplist.Count - 1; //id new group

            // list white groups touched, connect them.
            for (int i = 0; i < friends.Count; ++i)
            {
                Group g = friends[i];
                Point p0 = g.At(0);
                int id0 = m_groupmap[p0.X][p0.Y]; //id group to merge
                id = MergeGroup(id, id0); // id merged group
            }
            
            // test suicide (can be legal in some rule set)
            id = m_grouplist.Count - 1;
            newgroup = m_grouplist[id];
            if (GetLibertyCount(newgroup) == 0)
            {
                if (color == Stone.BLACK) m_nwcapture += newgroup.Count;
                else m_nbcapture -= newgroup.Count;
                RemoveGroup(id);
            }
            else
            {
                //update hotpoints (2) add new
                if (kopossible && (newgroup.Count == 1) && (GetLibertyCount(newgroup) == 1))
                {
                    Point p0 = newgroup.At(0);
                    for (Dir d = Dir.LEFT; d <= Dir.DOWN; ++d)
                    {
                        Point p1 = Op.Translate(p0, d);
                        if (InBoard(p1) && (Get(p1) == Stone.NONE))
                        {
                            m_hotpoints.Add(p1);
                            break;
                        }
                    }
                    
                }
            }
        }

        public void RemoveId(int id)
        {
            m_grouplist.RemoveAt(id);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (m_groupmap[i][j] > id)
                    {
                        --m_groupmap[i][j];
                    }
                    else if (m_groupmap[i][j] == id)
                    {
                        m_groupmap[i][j] = -1;
                    }
                }
            }
        }

        public void RemoveGroup(int id)
        {
            Group g = m_grouplist[id];
            for (int i = 0; i < g.Count; ++i)
            {
                Point p = g.At(i) ;
                m_board[p.X, p.Y] = Stone.NONE;
            }
            RemoveId(id);
        }

        public int MergeGroup(int id0, int id1)
        {
            if (id0 != id1)
            {
                //Group g0 = m_grouplist[id0];
                Group g1 = m_grouplist[id1];
                for (int i = 0; i < g1.Count; ++i)
                {
                    Point p = g1.At(i) ;
                    m_groupmap[p.X][p.Y] = id0; // reindex
                }
                //g0 += g1;
                m_grouplist[id0] += g1; // merge
                RemoveId(id1); // remove old
            }
            return (id0 > id1) ? id0 - 1 : id0;
        }

    }
}
