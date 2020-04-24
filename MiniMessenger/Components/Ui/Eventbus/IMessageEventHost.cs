using System;

namespace MiniMessenger.Components.Ui.Eventbus
{
    public interface IMessageEventHost
    {
        Type ViewType { get; }
        Type MessageType { get; }

        void Send(IMessageContainer message);

        void Subscribe(Func<IMessageContainer, bool> receiverMethod);
        void RemoveEventMethod();
    }
}