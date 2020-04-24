using MiniMessenger.Components.Data;
using MiniMessenger.Components.UserSettings;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Windows;

namespace MiniMessenger.Components.Service
{
    public class ServiceConnector
    {
        private static ServiceConnector _service;
        private UserItem _user;

        public string ServerAddress { get; private set; }

        private ServiceConnector(string serverAddress)
        {
            this.ServerAddress = serverAddress;
        }

        internal static ServiceConnector GetInstance()
        {
            if (_service == null)
            {
                var serverAddress = UserSettingsLoader.GetInstance().GetUrl();
                _service = new ServiceConnector(serverAddress);
            }

            return _service;
        }


        public UserItem[] GetUserItems()
        {
            var downloadString = this.TryDownloadString($"{this.ServerAddress}?userid={this._user.ID}");

            if (string.IsNullOrEmpty(downloadString))
            {
                return new UserItem[0];
            }

            return JsonConvert.DeserializeObject<ResponseUsers>(downloadString) is ResponseUsers response
                ? response.Content as UserItem[]
                : new UserItem[0];
        }

        internal MessageItem[] GetMessages(long toUserId)
        {
            var downloadString = this.TryDownloadString($"{this.ServerAddress}getMessages?userid={this._user.ID}&touserid={toUserId}");

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


        internal bool SendMessage(long toUserId, string sendText)
        {
            var downloadString = this.TryDownloadString($"{this.ServerAddress}sendMessage?userid={this._user.ID}&touserid={toUserId}&messagetext={sendText}&fromme={true}");

            if (string.IsNullOrEmpty(downloadString))
            {
                return false;
            }

            var result = JsonConvert.DeserializeObject<Response>(downloadString);

            return result.Success;
        }

        internal bool TrySetUsername(string username, out UserItem userItem)
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

        internal void SetAddress(string serverAddres) => this.ServerAddress = serverAddres;

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
    }
}
