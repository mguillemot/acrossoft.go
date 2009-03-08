namespace Acrossoft.Engine.Network
{
    public interface IMessageProcessor
    {
        bool OnMessage(Message message);
    }
}
