using MiniMessenger.Components.Service;
using MiniMessenger.Views.Base;
using System.Collections.ObjectModel;

namespace MiniMessenger.Views.Devices
{
    public class DevicesViewModel : BaseViewModel
    {
        private ObservableCollection<DeviceItem> _devices = new ObservableCollection<DeviceItem>();
        private DeviceItem _selectedDevice;
        private DeviceItem _updateDevice;

        public ObservableCollection<DeviceItem> Devices
        {
            get => this._devices;
            set
            {
                this._devices = value;
                this.OnNotifyPropertyChanged(nameof(this.Devices));
            }
        }

        public DeviceItem SelectedDevice
        {
            get => this._selectedDevice;
            set
            {
                this._selectedDevice = value;

                if(this._selectedDevice != null)
                {
                    if(this.UpdateDevice == null || (!this.UpdateDevice.ID.Equals(this._selectedDevice.ID)))
                    {
                        this.UpdateDevice = this._selectedDevice;
                    }
                }

                this.OnNotifyPropertyChanged(nameof(this.SelectedDevice));
            }
        }

        public DeviceItem UpdateDevice 
        { 
            get => this._updateDevice;
            set
            {
                this._updateDevice = value;
                this.OnNotifyPropertyChanged(nameof(this.UpdateDevice));
            }
        }
    }
}
