using Acrossoft.Engine.Network;

namespace Acrossoft.Go.Network.Messages.Session
{
    public class SessionCreateErrorMessage : Message
    {
        public override ushort MessageId
        {
            get { return (ushort) Protocol.SESSION_CREATE_ERROR; }
        }
    }
}