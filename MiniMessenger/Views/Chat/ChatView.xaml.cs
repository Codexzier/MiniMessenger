using MiniMessenger.Components.Data;
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
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private readonly ChatViewModel _viewModel;
        public ChatView()
        {
            this.InitializeComponent();

            this._viewModel = (ChatViewModel)this.DataContext;
            EventbusManager.GetEventbus().Register<ChatView, LoadCommandMessage>(this.LoadCommandReceived);

            this._dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            this._dispatcherTimer.Tick += this._dispatcherTimer_Tick;
            this._dispatcherTimer.Start();
        }

        private bool LoadCommandReceived(IMessageContainer arg) 
        {
            this._viewModel.UserItem = arg.Content as UserItem;

            this._dispatcherTimer.Start();

            return false;
        }

        private void _dispatcherTimer_Tick(object sender, EventArgs e)
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

        public void Dispose() => EventbusManager.GetEventbus().Deregister<ChatView>();
    }
}
