using Acrossoft.Engine.Network;
using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Network.Messages.Session
{
    public class SessionJoinErrorMessage : Message
    {
        public NetworkSessionJoinError JoinError { get; set; }

        public override ushort MessageId
        {
            get { return (ushort) Protocol.SESSION_JOIN_ERROR; }
        }
    }
}