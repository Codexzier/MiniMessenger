using MiniMessenger.Components.Data;
using MiniMessenger.Views.Base;
using System.Collections.ObjectModel;

namespace MiniMessenger.Views.Chat
{
    public class ChatViewModel : BaseViewModel
    {
        private string _sendText;
        private UserItem _userItem;
        private int _lastIndex;

        public ObservableCollection<MessageItem> Messages { get; set; } = new ObservableCollection<MessageItem>();

        public string SendText
        {
            get => this._sendText;
            set
            {
                this._sendText = value;
                this.OnNotifyPropertyChanged(nameof(this.SendText));
            }
        }

        public UserItem UserItem { get => this._userItem; set { this._userItem = value; this.OnNotifyPropertyChanged(nameof(this.UserItem)); } }

        public int LastIndex
        {
            get => this._lastIndex;
            set
            {
                this._lastIndex = value;
                this.OnNotifyPropertyChanged(nameof(this.LastIndex));
            }
        }
    }
}
