using Acrossoft.Engine.Network;
using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Network.Messages.Lobby
{
    public class ChatContentNetMessage : NetworkMessage
    {
        public string Content { get; set; }

        public override ushort MessageId
        {
            get { return (ushort) Protocol.NET_CHAT_CONTENT; }
        }

        public override bool Decode(PacketReader reader)
        {
            Content = reader.ReadString();
            return true;
        }

        protected override bool EncodeContent(PacketWriter writer)
        {
            writer.Write(Content);
            return true;
        }
    }
}
