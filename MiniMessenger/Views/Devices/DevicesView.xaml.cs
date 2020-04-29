using MiniMessenger.Components.Messenger;
using MiniMessenger.Components.Service;
using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Views.Base;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MiniMessenger.Views.Devices
{
    /// <summary>
    /// Interaction logic for DevicesView.xaml
    /// </summary>
    public partial class DevicesView : UserControl, IDisposable
    {
        private readonly DevicesViewModel _viewModel;
        private readonly ServiceConnector _serviceConnector;
        private readonly int _channel = 2;

        public DevicesView()
        {
            this.InitializeComponent();

            this._serviceConnector = ServiceConnector.GetInstance();
            this._viewModel = (DevicesViewModel)this.DataContext;

            this.SetupUpdate();
        }

        private void SetupUpdate()
        {
            MessengerManager.GetInstance().Add(() =>
            {
                if (!EventbusManager.IsViewOpen(typeof(DevicesView), this._channel))
                {
                    return;
                }


                EventbusManager.Send<DevicesView, DeviceUpdateCommand>(new DeviceUpdateCommand(CommandMessage.Update), this._channel);
            });
            EventbusManager.Register<DevicesView, DeviceUpdateCommand>(this.ReceiveUpdateCommand);
        }

        public void Dispose() => EventbusManager.Deregister<DevicesView>();

        private bool ReceiveUpdateCommand(IMessageContainer arg)
        {
            if (arg.Content is CommandMessage received && received.Equals(CommandMessage.Update))
            {
                var deviceItems = this._serviceConnector.DeviceGetAll();

                foreach (var item in deviceItems)
                {
                    var deviceItem = this._viewModel.Devices.FirstOrDefault(f => f.ID == item.ID);

                    if (deviceItem == null)
                    {
                        this._viewModel.Devices.Add(item);
                    }
                    else
                    {
                        this._viewModel.Devices.Remove(deviceItem);
                        this._viewModel.Devices.Add(item);
                    }
                }
            }

            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this._viewModel.UpdateDevice == null)
            {
                return;
            }

            this._serviceConnector.DeviceSendCommand(this._viewModel.UpdateDevice.ID, this._viewModel.UpdateDevice.Value, this._viewModel.UpdateDevice.Text);
        }


    }
}
