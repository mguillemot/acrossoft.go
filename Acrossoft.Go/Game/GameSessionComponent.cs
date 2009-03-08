using Acrossoft.Engine.Console;
using Acrossoft.Engine.Network;
using Acrossoft.Go.Network;
using Acrossoft.Go.Network.Messages.Session;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace Acrossoft.Go.Game
{
    public class GameSessionComponent : GameComponent, IMessageProcessor
    {
        private GameSession m_gameSession;
        private NetworkSession m_networkSessionWaitingToBeLinked;

        public GameSessionComponent(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            var messageDispatcher = (IMessageDispatcher) Game.Services.GetService(typeof (IMessageDispatcher));
            messageDispatcher.RegisterProcessor((ushort) Protocol.PLAYER_JOINED_SESSION, this);
            messageDispatcher.RegisterProcessor((ushort) Protocol.PLAYER_LEFT_SESSION, this);
            messageDispatcher.RegisterProcessor((ushort) Protocol.SESSION_CREATED, this);
            messageDispatcher.RegisterProcessor((ushort) Protocol.SESSION_CREATE_ERROR, this);
            messageDispatcher.RegisterProcessor((ushort) Protocol.SESSION_JOINED, this);
            messageDispatcher.RegisterProcessor((ushort) Protocol.SESSION_JOIN_ERROR, this);
            messageDispatcher.RegisterProcessor((ushort) Protocol.SESSION_ENDED, this);
            messageDispatcher.RegisterProcessor((ushort) Protocol.NET_SESSION_DESCRIPTION, this);

            m_gameSession = new GameSession();
            m_gameSession.InitializeLocalSession();
        }

        public bool OnMessage(Message message)
        {
            switch ((Protocol) message.MessageId)
            {
                case Protocol.SESSION_CREATED:
                    var sessionCreatedMsg = (SessionCreatedMessage) message;
                    m_gameSession.LinkToNetworkSession(sessionCreatedMsg.Session);
                    Console.WriteLine("Session created and linked to network. Host is " +
                                      sessionCreatedMsg.Session.Host.Gamertag);
                    return false;
                case Protocol.SESSION_CREATE_ERROR:
                    Console.WriteLine("Session create error");
                    return false;
                case Protocol.SESSION_JOINED:
                    var sessionJoinedMsg = (SessionJoinedMessage) message;
                    m_networkSessionWaitingToBeLinked = sessionJoinedMsg.Session;
                    Console.WriteLine("Session joined and linked to network. Host is " +
                                      sessionJoinedMsg.Session.Host.Gamertag);
                    return false;
                case Protocol.SESSION_JOIN_ERROR:
                    var joinErrorMsg = (SessionJoinErrorMessage) message;
                    Console.WriteLine("Session join error: " + joinErrorMsg.JoinError);
                    return false;
                case Protocol.SESSION_ENDED:
                    Console.WriteLine("Session ended");
                    // TODO flag it
                    return false;
                case Protocol.NET_SESSION_DESCRIPTION:
                    var sessionDescriptionMsg = (SessionDescriptionNetMessage) message;
                    //m_gameSession = sessionDescriptionMsg.Session;
                    //m_gameSession.LinkToNetworkSession(m_networkSessionWaitingToBeLinked);
                    //m_networkSessionWaitingToBeLinked = null;
                    // TODO
                    return false;
                case Protocol.PLAYER_JOINED_SESSION:
                    var joinMsg = (PlayerJoinedSessionMessage) message;
                    var player = new Player(m_gameSession, joinMsg.Gamer.Id);
                    m_gameSession.RegisterPlayer(player);
                    Console.WriteLine(joinMsg.Gamer.Gamertag + " joined my session");
                    return false;
                case Protocol.PLAYER_LEFT_SESSION:
                    var leftMsg = (PlayerLeftSessionMessage) message;
                    m_gameSession.RemovePlayer(leftMsg.Gamer.Id);
                    Console.WriteLine(leftMsg.Gamer.Gamertag + " left my session");
                    return false;
            }
            return true;
        }
    }
}