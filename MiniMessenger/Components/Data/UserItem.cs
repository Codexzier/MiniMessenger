using MiniMessenger.Components.Ui.Smily;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace MiniMessenger.Components.Data
{
    public class UserItem : IEquatable<UserItem>, INotifyPropertyChanged
    {
        private bool _isOnline;
        private SmilyEmote _smile;

        public long ID { get; set; }

        public string Username { get; set; }

        public bool IsOnline
        {
            get => this._isOnline; 
            set
            {
                //if(this._isOnline == value)
                //{
                //    return;
                //}
                this._isOnline = value;
                this.OnPropertyChanged(nameof(this.IsOnline));
            }
        }

        public SmilyEmote Smile
        {
            get => this._smile;
            set
            {
                this._smile = value;
                this.OnPropertyChanged(nameof(this.Smile));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Equals([AllowNull] UserItem other) => this.ID.Equals(other.ID);
    }
}
