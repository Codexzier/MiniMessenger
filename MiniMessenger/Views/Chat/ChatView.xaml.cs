using MiniMessenger.Components.Data;
using MiniMessenger.Components.Messenger;
using MiniMessenger.Components.Service;
using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Views.Userlist;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MiniMessenger.Views.Chat
{
    public partial class ChatView : UserControl, IDisposable
    {
        private readonly ChatViewModel _viewModel;
        public ChatView()
        {
            this.InitializeComponent();

            this._viewModel = (ChatViewModel)this.DataContext;

            MessengerManager.GetInstance().Add(() =>
            {
                if (!EventbusManager.IsViewOpen(typeof(ChatView), 0))
                {
                    return;
                }

                this._dispatcherTimer_Tick();
            });
            EventbusManager.Register<ChatView, LoadCommandMessage>(this.LoadCommandReceived);
        }

        private bool LoadCommandReceived(IMessageContainer arg) 
        {
            this._viewModel.UserItem = arg.Content as UserItem;

            return false;
        }

        private void _dispatcherTimer_Tick()
        {
            var messages = ServiceConnector.GetInstance().GetMessages(this._viewModel.UserItem.ID);

            foreach (var item in messages)
            {
                if (this._viewModel.Messages.Contains(item))
                {
                    continue;
                }

                this._viewModel.Messages.Add(item);
            }

            // TODO: sieht nicht gut aus.
            this._viewModel.LastIndex = this._viewModel.Messages.Count - 1;
            this.listbox.ScrollIntoView(this.listbox.SelectedItem);
        }

        private void ButtonSendText_Click(object sender, RoutedEventArgs e)
        {
            this.SendMessageToUser();
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                this.SendMessageToUser();
            }
        }

        private void SendMessageToUser()
        {
            if (string.IsNullOrEmpty(this._viewModel.SendText))
            {
                return;
            }

            ServiceConnector.GetInstance().SendMessage(this._viewModel.UserItem.ID, this._viewModel.SendText);
            this._viewModel.SendText = string.Empty;
        }

        public void Dispose() => EventbusManager.Deregister<ChatView>();
    }
}
