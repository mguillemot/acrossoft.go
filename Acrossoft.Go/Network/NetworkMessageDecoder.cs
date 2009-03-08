using Acrossoft.Engine.Network;
using Acrossoft.Go.Network.Messages.Lobby;
using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Network 
{

    public class NetworkMessageDecoder
    {
        private readonly PacketReader m_packetReader = new PacketReader();

        public NetworkMessage Decode(LocalNetworkGamer gamer)
        {
            NetworkGamer sender;
            gamer.ReceiveData(m_packetReader, out sender);
            if (m_packetReader.Length >= 2)
            {
                var messageType = (Protocol) m_packetReader.ReadUInt16();
                NetworkMessage msg = null;
                switch (messageType)
                {
                    // 1-999

                    // 1000-1999
                    case Protocol.NET_CHAT_CONTENT:
                        msg = new ChatContentNetMessage();
                        break;

                    // 2000-2999
                }
                if (msg != null)
                {
                    msg.Gamer = sender;
                    if (msg.Decode(m_packetReader))
                    {
                        return msg;
                    }
                }
            }
            return null;
        }
    }
}