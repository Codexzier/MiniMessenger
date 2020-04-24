using MiniMessenger.Components.Ui.Eventbus;

namespace MiniMessenger.Views.Menu
{
    internal class UpdateCommandMessage : IMessageContainer
    {
        public UpdateCommandMessage(object command)
        {
            this.Content = command;
        }

        public object Content { get; }
    }
}