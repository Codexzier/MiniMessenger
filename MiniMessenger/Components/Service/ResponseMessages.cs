using MiniMessenger.Components.Data;

namespace MiniMessenger.Components.Service
{
    internal class ResponseMessages : Response
    {
        public MessageItem[] Content { get; set; }
    }
}