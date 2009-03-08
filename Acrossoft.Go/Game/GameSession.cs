using System.Collections.Generic;
using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Game
{
    public class GameSession
    {
        private readonly Dictionary<byte, Player> m_playersById = new Dictionary<byte, Player>();
        private NetworkSession m_networkSession;
        //private bool m_paused;

        public void InitializeLocalSession()
        {
        }

        public void LinkToNetworkSession(NetworkSession networkSession)
        {
            m_networkSession = networkSession;
        }

        public NetworkSession NetworkSession
        {
            get { return m_networkSession; }
        }

        public int MaxPlayers
        {
            get { return 16; }
        }

        public int MaxPlayersByTeam
        {
            get { return 4; }
        }

        public int MaxTeams
        {
            get { return 4; }
        }

        public void RemovePlayer(byte id)
        {
            m_playersById.Remove(id);
        }

        public ICollection<Player> Players
        {
            get { return m_playersById.Values; }
        }

        public Player GetPlayer(byte id)
        {
            return m_playersById[id];
        }

        public static bool Serialize(PacketWriter writer, GameSession session)
        {
            ICollection<Player> players = session.Players;
            writer.Write((byte) players.Count);
            foreach (Player player in players)
            {
                Player.Serialize(writer, player);
            }
            return true;
        }

        public static GameSession Unserialize(PacketReader reader)
        {
            var session = new GameSession();
            int nPlayers = reader.ReadByte();
            for (int i = 0; i < nPlayers; i++)
            {
                var player = Player.Unserialize(reader, session);
                if (player != null)
                {
                    session.RegisterPlayer(player);
                }
            }
            return session;
        }

        /*
        public void Pause()
        {
            if (!m_paused)
            {

                m_paused = true;
            }
        }

        public void Unpause()
        {
            if (m_paused)
            {

                m_paused = false;
            }
        }*/

        public void RegisterPlayer(Player player)
        {
            m_playersById[player.Id] = player;
        }
    }
}