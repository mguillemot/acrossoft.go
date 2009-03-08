using Acrossoft.Engine.Network;
using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Network.Messages.Session
{
    public class SessionJoinedMessage : Message
    {
        public NetworkSession Session { get; set; }

        public override ushort MessageId
        {
            get { return (ushort) Protocol.SESSION_JOINED; }
        }
    }
}
