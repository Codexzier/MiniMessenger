using MiniMessenger.Components.Ui.Eventbus;

namespace MiniMessenger.Views.Main
{
    public class MainMessage : IMessageContainer
    {
        public MainMessage(object content)
        {
            this.Content = content;
        }
        public object Content { get; }
    }
}
