using MiniMessenger.Components.Data;
using System.Collections;
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

        long DeviceGetValue(long id);
    }
}