using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Game
{
    public class Player
    {
        private readonly GameSession m_session;
        private readonly byte m_id;

        public Player(GameSession session, byte id)
        {
            m_session = session;
            m_id = id;
        }

        public byte Id
        {
            get { return m_id; }
        }

        public NetworkGamer Gamer
        {
            get
            {
                foreach (var gamer in m_session.NetworkSession.AllGamers)
                {
                    if (gamer.Id == m_id)
                    {
                        return gamer;
                    }
                }
                return null;
            }
        }

        public static bool Serialize(PacketWriter writer, Player player)
        {
            writer.Write(player.Id);
            return true;
        }

        public static Player Unserialize(PacketReader reader, GameSession session)
        {
            var id = reader.ReadByte();
            var player = new Player(session, id);
            return player;
        }
    }
}
