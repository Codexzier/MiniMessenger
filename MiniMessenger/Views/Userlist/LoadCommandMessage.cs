using MiniMessenger.Components.Ui.Eventbus;

namespace MiniMessenger.Views.Userlist
{
    public class LoadCommandMessage : IMessageContainer
    {
        public LoadCommandMessage(object selectedUser)
        {
            this.Content = selectedUser;
        }
        public object Content { get; }
    }
}