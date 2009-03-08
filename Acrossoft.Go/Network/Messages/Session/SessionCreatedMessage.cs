using Acrossoft.Engine.Network;
using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Network.Messages.Session
{
    public class SessionCreatedMessage : Message
    {
        public NetworkSession Session { get; set; }

        public override ushort MessageId
        {
            get { return (ushort) Protocol.SESSION_CREATED; }
        }
    }
}
