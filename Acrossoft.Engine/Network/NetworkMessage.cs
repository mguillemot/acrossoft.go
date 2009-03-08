using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Engine.Network
{
    public abstract class NetworkMessage : Message
    {
        public NetworkGamer Gamer { get; set; }

        public abstract bool Decode(PacketReader reader);

        public bool Encode(PacketWriter writer)
        {
            writer.Write((ushort) MessageId);
            return EncodeContent(writer);
        }

        protected abstract bool EncodeContent(PacketWriter writer);

    }
}
