using System;

namespace MiniMessenger.Components.Ui.Eventbus
{
    public class EventbusException : Exception
    {
        public EventbusException()
        {
        }

        public EventbusException(string message) : base(message)
        {
        }

        public EventbusException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}