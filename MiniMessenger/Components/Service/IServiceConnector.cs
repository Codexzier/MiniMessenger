using MiniMessenger.Components.Data;
using System.Collections.Generic;

namespace MiniMessenger.Components.Service
{
    public interface IServiceConnector
    {
        UserItem[] GetAllUsers();

        MessageItem[] GetMessages(long toUserId);

        bool SendMessage(long toUserId, string sendText);

        bool TrySetUsername(string username, out UserItem userItem);

        void SetAddress(string serverAddres);


        IEnumerable<DeviceItem> DeviceGetAll();

        bool DeviceSendCommand(long id, long value);
        bool DeviceSendCommand(long id, string text);

        long DeviceGetValue(long id);
        string DeviceGetText(long id);

        ResponseDevice DeviceGet(long id);
        bool DeviceSendCommand(long id, long value, string sendText);
    }
}