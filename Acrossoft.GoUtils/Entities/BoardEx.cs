using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Acrossoft.GoUtils.Entities
{

    // extended board representation
    public class BoardEx
    {
        private readonly Board m_board;
        private readonly int[,] m_groupmap;
        private readonly List<Group> m_grouplist;
        private readonly List<Point> m_hotpoints; // indicates hot points (ex simple ko)
        private int m_npassinarow; // number of consecutive pass
        private int m_nbcapture;
        private int m_nwcapture;
        private int m_nmove;
        private Stone m_color;

        public BoardEx(int size)
        {
            m_board = new Board(size);
            m_groupmap = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    m_groupmap[i,j] = -1;
                }
            }
            m_grouplist = new List<Group>();
            m_hotpoints = new List<Point>();
            m_npassinarow = 0;
            m_nbcapture = 0;
            m_nwcapture = 0;
            m_nmove = 0;
            m_color = Stone.BLACK;
        }

        // Assumption: empty board
        public BoardEx(Board board)
        {
            m_board = board;
            m_groupmap = new int[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    m_groupmap[i,j] = -1;
                }
            }
            m_grouplist = new List<Group>();
            m_hotpoints = new List<Point>();
            m_npassinarow = 0;
            m_nbcapture = 0;
            m_nwcapture = 0;
            m_nmove = 0;
            m_color = Stone.BLACK;
        }

        public object Clone()
        {
            var clone = new BoardEx( (Board)m_board.Clone() );

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    clone.m_groupmap[i, j] = m_groupmap[i, j];
                }
            }
            foreach (var g in m_grouplist)
            {
                clone.m_grouplist.Add((Group)g.Clone());
            }
            foreach (var p in m_hotpoints)
            {
                clone.m_hotpoints.Add(new Point(p.X, p.Y));
            }
            clone.m_npassinarow = m_npassinarow;
            clone.m_nbcapture = m_nbcapture;
            clone.m_nwcapture = m_nwcapture;
            clone.m_nmove = m_nmove;
            clone.m_color = m_color;
            return clone;
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
            get { return m_nmove; }
        }

        public int PassCount
        {
            get { return m_npassinarow; }
        }

        public Stone Color
        {
            get { return m_color; }
        }

        public int GetGroupIndex(Point p)
        {
            return m_groupmap[p.X,p.Y];
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
            //Utils.HashSet<Point> libset = new Utils.HashSet<Point>(); // apparement ca plante
            HashSet<Point> libset = new HashSet<Point>();
            foreach (var p in group)
            {
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
            foreach (var hp in m_hotpoints)
            {
                if (hp == p)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }


        // -- in fact, for Move/LegalMove, the color parameter is useless -> to remove ?
        //    the color can be checked using "Color" accessor

        // check if legal move under japanese rules, ko rule depending on hot points.
        public bool LegalMove(Point p)
        {
            return InBoard(p) && (Get(p) == Stone.NONE) && !HotPoint(p) && !SuicidePlay(p, m_color) ;             
        }

        public void Pass()
        {
            ++m_npassinarow;
            m_color = (m_color == Stone.BLACK) ? Stone.WHITE : Stone.BLACK;
        }

        // play any playable move (even illegal ones)
        // return false if the move is unplayable
        public bool Move(Point p)
        {
            if (!InBoard(p) || Get(p)!=Stone.NONE)
            {
                // not only illegal, but impossible moves
                return false;
            }

            m_npassinarow = 0;
            ++m_nmove;

            //update hotpoints (1) remove old
            m_hotpoints.Clear();
            bool kopossible = true;

            //put the stone, as a new lone stone group
            m_board[p.X, p.Y] = m_color;
            Group newgroup = new Group() ;
            newgroup += p;
            int id = m_grouplist.Count;
            m_grouplist.Add(newgroup);
            m_groupmap[p.X,p.Y] = id;

            // list groups touched
            //Utils.HashSet<Group> touchedgroups = new Utils.HashSet<Group>(); // apparement ca plante
            HashSet<Group> touchedgroups = new HashSet<Group>();
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
                if (g.Count > 0 && (Get(g[0]) == m_color))
                    friends.Add(g);
                else
                    opponents.Add(g);
            }

            // check liberties of opponent groups and remove if captured (ajusting capture count)
            int capturecount = 0; // simpleko test
            foreach (var g in opponents)
            {
                if (GetLibertyCount(g) == 0)
                {
                    capturecount += g.Count; // simpleko test
                    if (m_color == Stone.BLACK) m_nbcapture += g.Count;
                    else m_nwcapture -= g.Count;
                    Point p0 = g[0];
                    RemoveGroup(m_groupmap[p0.X,p0.Y]);
                }
            }

            if (capturecount != 1) kopossible = false; // simpleko test

            id = m_grouplist.Count - 1; //id new group

            // list white groups touched, connect them.
            foreach (var g in friends)
            {
                Point p0 = g[0];
                int id0 = m_groupmap[p0.X,p0.Y]; //id group to merge
                id = MergeGroup(id, id0); // id merged group
            }
            
            // test suicide (can be legal in some rule set)
            id = m_grouplist.Count - 1;
            newgroup = m_grouplist[id];
            if (GetLibertyCount(newgroup) == 0)
            {
                if (m_color == Stone.BLACK) m_nwcapture += newgroup.Count;
                else m_nbcapture -= newgroup.Count;
                RemoveGroup(id);
            }
            else
            {
                //update hotpoints (2) add new
                if (kopossible && (newgroup.Count == 1) && (GetLibertyCount(newgroup) == 1))
                {
                    Point p0 = newgroup[0];
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

            m_color = (m_color == Stone.BLACK) ? Stone.WHITE : Stone.BLACK;
            return true;
        }

        public void RemoveId(int id)
        {
            m_grouplist.RemoveAt(id);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (m_groupmap[i,j] > id)
                    {
                        --m_groupmap[i,j];
                    }
                    else if (m_groupmap[i,j] == id)
                    {
                        m_groupmap[i,j] = -1;
                    }
                }
            }
        }

        public void RemoveGroup(int id)
        {
            Group g = m_grouplist[id];
            foreach (var p in g)
            {
                m_board[p.X, p.Y] = Stone.NONE;
            }
            RemoveId(id);
        }

        public int MergeGroup(int id0, int id1)
        {
            if (id0 != id1)
            {
                Group g1 = m_grouplist[id1];
                foreach (var p in g1)
                {
                    m_groupmap[p.X,p.Y] = id0; // reindex
                }
                m_grouplist[id0] += g1; // merge
                RemoveId(id1); // remove old
            }
            return (id0 > id1) ? id0 - 1 : id0;
        }

    }
}
