using Acrossoft.Engine.Network;
using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Network.Messages.Session
{
    public class PlayerLeftSessionMessage : Message
    {
        public override ushort MessageId
        {
            get { return (ushort) Protocol.PLAYER_LEFT_SESSION; }
        }

        public NetworkGamer Gamer { get; set; }
    }
}
