using MiniMessenger.Components.Data;
using MiniMessenger.Components.UserSettings;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;

namespace MiniMessenger.Components.Service
{
    public class ServiceConnector : IServiceConnector
    {
        private static ServiceConnector _instance;
        private UserItem _user;

        /// <summary>
        /// This id can use for user and device
        /// </summary>
        private const string _urlParameterId = "id";

        public string ServerAddress { get; private set; }
        public IEnumerable<string> ServerAddressItems { get; private set; }

        private ServiceConnector(string serverAddress, IEnumerable<string> serverAddressItems)
        {
            this.ServerAddress = serverAddress;
            this.ServerAddressItems = serverAddressItems;
        }

        public static ServiceConnector GetInstance()
        {
            if (_instance == null)
            {
                var settings = UserSettingsLoader.GetInstance().Settings;
                var serverAddress = settings.ServerAddress;
                var serverAddressItems = settings.ServerAddressItems;
                _instance = new ServiceConnector(serverAddress, serverAddressItems);
            }

            return _instance;
        }


        public UserItem[] GetAllUsers()
        {
            var userId = this._user != null ? this._user.ID : -1;

            var downloadString = this.TryDownloadString($"{this.ServerAddress}getAllUsers?{_urlParameterId}={userId}");

            if (string.IsNullOrEmpty(downloadString))
            {
                return new UserItem[0];
            }

            return JsonConvert.DeserializeObject<ResponseUsers>(downloadString) is ResponseUsers response
                ? response.Content as UserItem[]
                : new UserItem[0];
        }

        public MessageItem[] GetMessages(long toUserId)
        {
            var downloadString = this.TryDownloadString($"{this.ServerAddress}getMessages?{_urlParameterId}={this._user.ID}&touserid={toUserId}");

            if (string.IsNullOrEmpty(downloadString))
            {
                return new MessageItem[0];
            }

            var response = JsonConvert.DeserializeObject<ResponseMessages>(downloadString);

            if(!response.Success)
            {
                Debug.WriteLine("Error: can not get aktual messages.");
                return new MessageItem[0];
            }

            return response.Content;
        }


        public bool SendMessage(long toUserId, string sendText)
        {
            var downloadString = this.TryDownloadString($"{this.ServerAddress}sendMessage?{_urlParameterId}={this._user.ID}&touserid={toUserId}&messagetext={sendText}&fromme={true}");

            if (string.IsNullOrEmpty(downloadString))
            {
                return false;
            }

            var result = JsonConvert.DeserializeObject<Response>(downloadString);

            return result.Success;
        }

        public bool TrySetUsername(string username, out UserItem userItem)
        {
            userItem = null;
            username = username.Replace(' ', '_');

            var downloadString = this.TryDownloadString($"{this.ServerAddress}addUser?username={username}");

            if(string.IsNullOrEmpty(downloadString) || downloadString.Equals("[]"))
            {
                return false;
            }

            // TODO: Umständlich zu lesen
            _ = JsonConvert.DeserializeObject<ResponseUsers>(downloadString) is ResponseUsers response
               ? userItem = (response.Content as UserItem[])[0]
               : userItem = null;

            this._user = userItem;

            return userItem != null;
        }

        public void SetAddress(string serverAddres) => this.ServerAddress = serverAddres;

        private string TryDownloadString(string urlString)
        {
            var client = new WebClient
            {
                BaseAddress = this.ServerAddress
            };

            client.Credentials = System.Net.CredentialCache.DefaultCredentials;

            string result = string.Empty;

            try
            {
                result = client.DownloadString(urlString);
            }
            catch (WebException webException)
            {
                MessageBox.Show($"Kann keine Verbindung zu {urlString} herstellen");
                MessageBox.Show(webException.Message);
            }

            return result;
        }

        public IEnumerable<DeviceItem> DeviceGetAll()
        {
            var downloadString = this.TryDownloadString($"{this.ServerAddress}deviceGetAll");
            if (string.IsNullOrEmpty(downloadString))
            {
                return new DeviceItem[0];
            }

            var result = JsonConvert.DeserializeObject<ResponseDevices>(downloadString);

            if (result.Content is DeviceItem[] deviceItems && result.Success)
            {
                return deviceItems;
            }

            return new DeviceItem[0];

        }

        public long DeviceGetValue(long id)
        {
            var downloadString = this.TryDownloadString($"{this.ServerAddress}deviceGetValue?{_urlParameterId}={id}");
            if (string.IsNullOrEmpty(downloadString))
            {
                return -1;
            }

            var result = JsonConvert.DeserializeObject<ResponseDevice>(downloadString);
            if (!result.Success)
            {
                return -1;
            }

            return result.Value;
        }


        public bool DeviceSendCommand(long id, long value) => throw new System.NotImplementedException();
        
        
    }
}
