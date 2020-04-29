using MiniMessenger.Components.Data;
using MiniMessenger.Components.Messenger;
using MiniMessenger.Components.Service;
using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Components.UserSettings;
using MiniMessenger.Views.Menu;
using MiniMessenger.Views.Userlist;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MiniMessenger.Views.Main
{
    public partial class MainView : UserControl, IDisposable
    {
        private readonly MainViewModel _viewModel;

        public MainView()
        {
            this.InitializeComponent();

            this._viewModel = (MainViewModel)this.DataContext;

            EventbusManager.Register<MainView, MainMessage>(this.EventBusReceivedMessage);
            var settings = UserSettingsLoader.GetInstance().Settings;
            this._viewModel.Username = settings.Username;
            this._viewModel.Interval = settings.Interval;
            this._viewModel.HostAddressItems = settings.ServerAddressItems;
            this._viewModel.HostAddress = settings.ServerAddress;
        }

        private bool EventBusReceivedMessage(IMessageContainer arg)
        {
            // do things with the content


            return true;
        }

        public void Dispose()
        {
            EventbusManager.Deregister<MainView>();
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this._viewModel.Username))
            {
                MessageBox.Show("username must be set.");
                return;
            }

            if (string.IsNullOrEmpty(this._viewModel.HostAddress))
            {
                MessageBox.Show("host address must be set.");
                return;
            }

            var service = ServiceConnector.GetInstance();
            service.SetAddress(this._viewModel.HostAddress);

            if (service.TrySetUsername(this._viewModel.Username, out var userItem))
            {
                EventbusManager.OpenView<UserlistView>(0);
                EventbusManager.Send<MainWindow, UpdateCommandMessage>(new UpdateCommandMessage(userItem), 0);
                EventbusManager.Send<MenuView, UpdateCommandMessage>(new UpdateCommandMessage(ViewOpen.Userlist), 0);

                if (!this._viewModel.HostAddressItems.Any(a => a.Equals(this._viewModel.HostAddress)))
                {
                    this._viewModel.HostAddressItems = this._viewModel.HostAddressItems.Append(this._viewModel.HostAddress);
                }

                var settings = new SettingsFile
                {
                    Username = this._viewModel.Username,
                    Interval = this._viewModel.Interval,
                    ServerAddress = this._viewModel.HostAddress,
                    ServerAddressItems = this._viewModel.HostAddressItems
                };
                UserSettingsLoader.GetInstance().Save(settings);

                MessengerManager.GetInstance().Start(this._viewModel.Interval);
                return;
            }

            MessageBox.Show($"Can not use username: {this._viewModel.Username}");
        }
    }
}
