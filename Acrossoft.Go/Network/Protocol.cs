namespace Acrossoft.Go.Network
{
    public enum Protocol : ushort 
    {
        // 1-999: System messages
        LOCAL_PLAYER_SIGN_IN = 1,
        LOCAL_PLAYER_SIGN_OUT,

        // 1000-1999: Lobby messages
        REMOTE_PLAYER_SIGN_IN = 1000,
        REMOTE_PLAYER_SIGN_OUT,
        NET_CHAT_CONTENT,

        // 2000-2999: Gameplay messages
        TODO = 2000,

        // 3000-3999: Session management messages
        PLAYER_JOINED_SESSION = 3000,
        PLAYER_LEFT_SESSION,
        SESSION_CREATED,
        SESSION_CREATE_ERROR,
        SESSION_JOINED,
        SESSION_JOIN_ERROR,
        SESSION_ENDED,
        NET_SESSION_DESCRIPTION
    }
}
