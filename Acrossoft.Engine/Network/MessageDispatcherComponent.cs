using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Acrossoft.Engine.Network
{
    public class MessageDispatcherComponent : GameComponent, IMessageDispatcher
    {
        private readonly Queue<Message> m_incomingMessages = new Queue<Message>();
        private readonly Dictionary<ushort, List<IMessageProcessor>> m_processorsByMessage = new Dictionary<ushort, List<IMessageProcessor>>();
        private readonly Dictionary<IMessageProcessor, List<ushort>> m_messagesByProcessor = new Dictionary<IMessageProcessor, List<ushort>>();

        public MessageDispatcherComponent(Game game) 
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            Game.Services.AddService(typeof(IMessageDispatcher), this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            while (m_incomingMessages.Count > 0)
            {
                var message = m_incomingMessages.Dequeue();
                Dispatch(message);
            }
        }

        public void DispatchMessage(Message message)
        {
            m_incomingMessages.Enqueue(message);
        }

        private void Dispatch(Message message)
        {
            List<IMessageProcessor> processors;
            if (m_processorsByMessage.TryGetValue(message.MessageId, out processors))
            {
                foreach (var processor in processors)
                {
                    if (!processor.OnMessage(message))
                    {
                        break;
                    }
                }
            }
        }

        public void RegisterProcessor(ushort interestedMessage, IMessageProcessor processor)
        {
            List<IMessageProcessor> processors;
            if (!m_processorsByMessage.TryGetValue(interestedMessage, out processors))
            {
                processors = new List<IMessageProcessor>();
                m_processorsByMessage[interestedMessage] = processors;
            }
            processors.Add(processor);
            //
            List<ushort> messages;
            if (!m_messagesByProcessor.TryGetValue(processor, out messages))
            {
                messages = new List<ushort>();
                m_messagesByProcessor[processor] = messages;
            }
            messages.Add(interestedMessage);
        }

        public void UnregisterProcessor(ushort interetedMessage, IMessageProcessor processor)
        {
            List<IMessageProcessor> processors;
            if (m_processorsByMessage.TryGetValue(interetedMessage, out processors))
            {
                processors.Remove(processor);
            }
            //
            List<ushort> messages;
            if (m_messagesByProcessor.TryGetValue(processor, out messages))
            {
                messages.Remove(interetedMessage);
            }
        }

        public void UnregisterProcessor(IMessageProcessor processor)
        {
            List<ushort> messages;
            if (m_messagesByProcessor.TryGetValue(processor, out messages))
            {
                foreach (var message in messages)
                {
                    var processorsByMessage = m_processorsByMessage[message];
                    processorsByMessage.RemoveAll(messageProcessor => messageProcessor == processor);
                    if (processorsByMessage.Count == 0)
                    {
                        m_processorsByMessage.Remove(message);
                    }
                }
                m_messagesByProcessor.Remove(processor);
            }
        }
    }
}
