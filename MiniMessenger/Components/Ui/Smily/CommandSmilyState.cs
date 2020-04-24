using MiniMessenger.Components.Ui.Eventbus;

namespace MiniMessenger.Components.Ui.Smily
{
    internal class CommandSmilyState : IMessageContainer
    {
        public CommandSmilyState(SmilyEmote emote)
        {
            this.Content = emote;
        }

        public object Content { get; }
    }
}