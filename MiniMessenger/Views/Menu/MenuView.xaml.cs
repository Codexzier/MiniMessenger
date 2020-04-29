using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Views.Base;
using MiniMessenger.Views.Devices;
using MiniMessenger.Views.Main;
using MiniMessenger.Views.Userlist;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MiniMessenger.Views.Menu
{
    public partial class MenuView : UserControl
    {
        
        private readonly MenuViewModel _viewModel;

        public MenuView()
        {
            this.InitializeComponent();

            this._viewModel = (MenuViewModel)this.DataContext;

            EventbusManager.Register<MenuView, UpdateCommandMessage>(this.UpdateCommandMessageReceived);
        }

        private bool UpdateCommandMessageReceived(IMessageContainer arg)
        {
            if (arg.Content is ViewOpen command)
            {
                this._viewModel.ViewOpened = command;
                return true;
            }

            return false;
        }

        private void ButtonOpenMain_Click(object sender, RoutedEventArgs e)
        {
            if (EventbusManager.IsViewOpen(typeof(MainView), 0))
            {
                return;
            }

            this._viewModel.ViewOpened = ViewOpen.Main;
            EventbusManager.OpenView<MainView>(0);
        }

        private void ButtonOpenUserList_Click(object sender, RoutedEventArgs e)
        {
            if(EventbusManager.IsViewOpen(typeof(UserlistView), 0))
            {
                return;
            }

            this._viewModel.ViewOpened = ViewOpen.Userlist;
            EventbusManager.OpenView<UserlistView>(0);
        }

        private void ButtonShowMessenger_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonShowDevices_Click(object sender, RoutedEventArgs e)
        {
            if(EventbusManager.IsViewOpen(typeof(DevicesView), 2))
            {
                EventbusManager.CloseView<DevicesView>(2);
                return;
            }

            EventbusManager.OpenView<DevicesView>(2);
        }
    }
}
