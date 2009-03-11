using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acrossoft.GoUtils.Entities;
using Microsoft.Xna.Framework;

namespace Acrossoft.GoUtils.GameTree
{
    // ishi: event / sgf:GameTree
    class GoEvent
    {
        private List<string> m_remarks;
        private GoEventHeader m_header;
        private int m_setup;
        private List<GoMove> m_tree;

        public GoEvent()
        {
            m_remarks = new List<string>();
            m_header = new GoEventHeader();
            m_setup = 0;
            m_tree = new List<GoMove>();
        }
    }

    struct GoEventHeader
    {
        private List<string> m_comments;    // ishi:COM / sgf:GC (game comment)

        int m_size;         // ishi:boardsize / sgf:SZ!
        Point m_upperleft, m_lowerright ;   // subrectangle (ishi only)
        string m_rules;     // ishi:rules / sgf:RU!
        string m_timelimit; // ishi:timelimit / sgf:TM (float)
        string m_overtime;  // sgf:OT*
        int m_handicap;     // ishi:handicap / sgf: HA (informative?)
        float m_komi;       // ishi:komi / sgf: KO
        string m_result;    // ishi:result / sgf:RE!
        
        // info interesting to save.
        string m_name;              // sgf:GN   (game name)
        DateTime m_date;            // ishi:date / sgf:DT!
        string m_black, m_white;    // player infos    ishi:black-white / sgf:PB-PW
        string m_bteam, m_wteam;    // sgf:BT-WT
        int m_brank, m_wrank;       // sgf:BR-WR
        
        // info probably unnecessary
        string m_place;     // ishi:place / sgf:PC
        string m_source;    // ishi:source / sgf:SO

        string m_analysis;  // commentator's name    ishi:analysis / sgf:AN (annotation)
        string m_recorder;  // recorder's name   ishi:recorder / sgf:US

        string m_copyright; // sgf:CP
        string m_event;     // sgf:EV   (event name eg:tournament)
        string m_opening;   // sgf:ON   (name of opening player)
        string m_round;     // sgf:RO   (tournament round)
        List<string> m_userdata;    // ishi:userdata
    }
}
