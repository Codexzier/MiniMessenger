using System;

namespace MiniMessenger.Components.Ui.Eventbus
{
    public class SideHostTypeChannel
    {
        public SideHostTypeChannel(int channel, Type typeView)
        {
            this.Channel = channel;
            this.TypeView = typeView;
        }

        public int Channel { get; private set; }

        public Type TypeView { get; private set; }
    }
}