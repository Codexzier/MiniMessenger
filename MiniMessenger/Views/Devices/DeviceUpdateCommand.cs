using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Views.Base;

namespace MiniMessenger.Views.Devices
{
    internal class DeviceUpdateCommand : IMessageContainer
    {
        public DeviceUpdateCommand(CommandMessage update) => this.Content = update;

        public object Content { get; }
    }
}