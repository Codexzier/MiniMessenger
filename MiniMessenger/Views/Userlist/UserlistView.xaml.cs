using MiniMessenger.Components.Service;
using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Views.Base;
using MiniMessenger.Views.Chat;
using MiniMessenger.Views.Menu;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiniMessenger.Views.Userlist
{
    /// <summary>
    /// Interaction logic for UserlistView.xaml
    /// </summary>
    public partial class UserlistView : UserControl, IDisposable
    {
        private readonly UserlistViewModel _viewModel;
        private ServiceConnector _serviceConnector;
        private EventbusManager _eventbus;

        public UserlistView()
        {
            this.InitializeComponent();

            this._serviceConnector = ServiceConnector.GetInstance();
            this._viewModel = (UserlistViewModel)this.DataContext;

            this._eventbus = EventbusManager.GetEventbus();
            this._eventbus.Register<UserlistView, UpdateCommandMessage>(this.ReceiveUpdateCommand);
        }

        private bool ReceiveUpdateCommand(IMessageContainer arg)
        {
            if(arg.Content is CommandMessage received && received.Equals(CommandMessage.Update))
            {
               var userItems = this._serviceConnector.GetUserItems();

                foreach (var item in userItems)
                {
                    var user = this._viewModel.Users.FirstOrDefault(f => f.ID == item.ID);

                    if(user != null)
                    {
                        this._viewModel.Users.Remove(item);
                        this._viewModel.Users.Add(item);
                        //if (user.IsOnline != item.IsOnline)
                        //{
                        //    item.IsOnline = user.IsOnline;

                            
                        //}
                    }
                    else
                    {
                        this._viewModel.Users.Add(item);
                    }

                }

                return true;
            }

            return false;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


            this._eventbus.Send<ChatView, LoadCommandMessage>(new LoadCommandMessage(this._viewModel.SelectedUser), true);
        }

        public void Dispose() => this._eventbus.Deregister<UserlistView>();
    }
}
