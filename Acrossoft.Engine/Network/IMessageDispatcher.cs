namespace Acrossoft.Engine.Network
{
    public interface IMessageDispatcher
    {
        void DispatchMessage(Message message);

        void RegisterProcessor(ushort interestedMessage, IMessageProcessor processor);

        void UnregisterProcessor(ushort interetedMessage, IMessageProcessor processor);

        void UnregisterProcessor(IMessageProcessor processor);
    }
}
