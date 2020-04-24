using System;

namespace MiniMessenger.Components.Ui.Eventbus
{
    public class RegisterContainer
    {
        private Type _typeView;
        private readonly Func<IMessageContainer, bool> _receiverMethod;

        public RegisterContainer(Type typeView, Func<IMessageContainer, bool> receiverMethod)
        {
            this._typeView = typeView;
            this._receiverMethod = receiverMethod;
        }

        public Type View => this._typeView;

        public Func<IMessageContainer, bool> EventMethod => this._receiverMethod;
    }
}
