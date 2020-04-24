using MiniMessenger.Components.Data;
using MiniMessenger.Views.Base;
using System.Collections.ObjectModel;

namespace MiniMessenger.Views.Userlist
{
    public class UserlistViewModel : BaseViewModel
    {
        private ObservableCollection<UserItem> _users = new ObservableCollection<UserItem>();
        private UserItem _selectedUser;

        public ObservableCollection<UserItem> Users
        {
            get => this._users;
            set
            {
                this._users = value;
                this.OnNotifyPropertyChanged(nameof(this.Users));
            }
        }

        public UserItem SelectedUser
        {
            get => this._selectedUser;
            set
            {
                this._selectedUser = value;
                this.OnNotifyPropertyChanged(nameof(this.SelectedUser));
            }
        }
    }
}
