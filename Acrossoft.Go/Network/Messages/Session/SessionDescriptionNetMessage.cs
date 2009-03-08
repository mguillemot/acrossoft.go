using Acrossoft.Engine.Network;
using Acrossoft.Go.Game;
using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Network.Messages.Session
{
    public class SessionDescriptionNetMessage : NetworkMessage
    {
        public override ushort MessageId
        {
            get { return (ushort) Protocol.NET_SESSION_DESCRIPTION; }
        }

        public GameSession Session { get; set; }

        public override bool Decode(PacketReader reader)
        {
            Session = GameSession.Unserialize(reader);
            return Session != null;
        }

        protected override bool EncodeContent(PacketWriter writer)
        {
            return GameSession.Serialize(writer, Session);
        }
    }
}
