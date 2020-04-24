using MiniMessenger.Components.Data;

namespace MiniMessenger.Components.Service
{
    internal class ResponseUsers : Response
    {
        public UserItem[] Content { get; set; }
    }
}