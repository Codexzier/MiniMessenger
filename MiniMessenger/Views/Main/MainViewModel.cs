using MiniMessenger.Views.Base;
using System.Collections.Generic;

namespace MiniMessenger.Views.Main
{
    public class MainViewModel : BaseViewModel
    {
        private string _username;
        private string _hostAddress;
        private IEnumerable<string> _hostAddressItems;
        private int _interval;

        public string Username
        {
            get => this._username;
            set
            {
                this._username = value;
                this.OnNotifyPropertyChanged(nameof(this.Username));
            }
        }

        public string HostAddress
        {
            get => this._hostAddress;
            set
            {
                this._hostAddress = value;
                this.OnNotifyPropertyChanged(nameof(this.HostAddress));
            }
        }

        public IEnumerable<string> HostAddressItems
        {
            get => this._hostAddressItems;
            set
            {
                this._hostAddressItems = value;
                this.OnNotifyPropertyChanged(nameof(this.HostAddressItems));
            }
        }

        public int Interval
        {
            get => this._interval;
            set
            {
                this._interval = value;
                this.OnNotifyPropertyChanged(nameof(this.Interval));
            }
        }
    }
}
