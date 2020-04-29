using MiniMessenger.Components.Messenger;
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
        private readonly ServiceConnector _serviceConnector;


        public UserlistView()
        {
            this.InitializeComponent();

            this._serviceConnector = ServiceConnector.GetInstance();
            this._viewModel = (UserlistViewModel)this.DataContext;

            this.SetupUpdate();
        }

        private void SetupUpdate()
        {
            MessengerManager.GetInstance().Add(() =>
            {
                if (!EventbusManager.IsViewOpen(typeof(UserlistView), 0))
                {
                    return;
                }

                
                EventbusManager.Send<UserlistView, UpdateCommandMessage>(new UpdateCommandMessage(CommandMessage.Update), 0);
            });
            EventbusManager.Register<UserlistView, UpdateCommandMessage>(this.ReceiveUpdateCommand);
        }

        private bool ReceiveUpdateCommand(IMessageContainer arg)
        {
            if(arg.Content is CommandMessage received && received.Equals(CommandMessage.Update))
            {
               var userItems = this._serviceConnector.GetAllUsers();

                foreach (var item in userItems)
                {
                    var user = this._viewModel.Users.FirstOrDefault(f => f.ID == item.ID);

                    if(user != null)
                    {
                        this._viewModel.Users.Remove(item);
                        this._viewModel.Users.Add(item);
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
            EventbusManager.Send<MenuView, UpdateCommandMessage>(new UpdateCommandMessage(ViewOpen.Chat), 0);
            EventbusManager.Send<ChatView, LoadCommandMessage>(new LoadCommandMessage(this._viewModel.SelectedUser), 0, true);
        }

        public void Dispose() => EventbusManager.Deregister<UserlistView>();
    }
}
