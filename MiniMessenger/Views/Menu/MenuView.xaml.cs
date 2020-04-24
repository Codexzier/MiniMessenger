using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Views.Base;
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
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private readonly MenuViewModel _viewModel;
        private readonly EventbusManager _eventbusManager;

        public MenuView()
        {
            this.InitializeComponent();

            this._viewModel = (MenuViewModel)this.DataContext;

            this._eventbusManager = EventbusManager.GetEventbus();
            EventbusManager.GetEventbus().Register<MenuView, UpdateCommandMessage>(this.UpdateCommandMessageReceived);
            this._dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            this._dispatcherTimer.Tick += this._dispatcherTimer_Tick;
            this._dispatcherTimer.Start();
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

        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if(this._dispatcherTimer == null)
            {
                return;
            }

            if (this._viewModel.Interval <= 0)
            {
                this._viewModel.Interval = 1;
            }

            this._dispatcherTimer.Interval = TimeSpan.FromSeconds(this._viewModel.Interval);

            if (!this._eventbusManager.IsViewOpen(typeof(UserlistView)))
            {
                return;
            }

            this._eventbusManager.Send<UserlistView, UpdateCommandMessage>(new UpdateCommandMessage(CommandMessage.Update));
        }

        private void ButtonOpenMain_Click(object sender, RoutedEventArgs e)
        {
            if (this._eventbusManager.IsViewOpen(typeof(MainView)))
            {
                return;
            }

            this._viewModel.ViewOpened = ViewOpen.Main;
            this._eventbusManager.OpenView<MainView>();
        }

        private void ButtonOpenUserList_Click(object sender, RoutedEventArgs e)
        {
            if(this._eventbusManager.IsViewOpen(typeof(UserlistView)))
            {
                return;
            }

            this._viewModel.ViewOpened = ViewOpen.Userlist;
            this._eventbusManager.OpenView<UserlistView>();
        }


        ~MenuView()
        {
            this._dispatcherTimer.Stop();
        }
    }
}
