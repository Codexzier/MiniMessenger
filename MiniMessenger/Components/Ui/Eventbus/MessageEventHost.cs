using System;
using System.Windows;

namespace MiniMessenger.Components.Ui.Eventbus
{
    public class MessageEventHost<TView, TMessage> : IMessageEventHost
        where TMessage : IMessageContainer
        where TView : DependencyObject
    {
        public Type ViewType => typeof(TView);
        public Type MessageType => typeof(TMessage);

        private Func<IMessageContainer, bool> _receiverMethod;

        public void Send(IMessageContainer message) => this.SendEvent?.Invoke(message);

        public void Subscribe(Func<IMessageContainer, bool> receiverMethod)
        {
            this._receiverMethod = receiverMethod;

            // TODO: Is mapping but how can I set directly
            this.SendEvent += (message) => { receiverMethod.Invoke(message); return true; };
            //this.SendEvent += receiverMethod;
            //receiverMethod += (message) => { return true; };
        }

        public void Remove()
        {
            this.SendEvent -= (message) => { return true; };
        }

        private bool MessageEventHost_SendEvent(IMessageContainer message) => throw new NotImplementedException();

        public void RemoveEventMethod()
        {
            this.SendEvent -= (message) => { return true; };
        }

        public delegate bool SendEventHandler(IMessageContainer message);
        public event SendEventHandler SendEvent;
    }
}
