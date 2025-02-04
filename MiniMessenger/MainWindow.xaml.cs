﻿using MiniMessenger.Components.Data;
using MiniMessenger.Components.Messenger;
using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Views.Base;
using MiniMessenger.Views.Main;
using MiniMessenger.Views.Menu;
using MiniMessenger.Views.Userlist;
using System.Windows;

namespace MiniMessenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EventbusManager.OpenView<MainView>(0);
            EventbusManager.Send<MenuView, UpdateCommandMessage>(new UpdateCommandMessage(ViewOpen.Main), 0);
            EventbusManager.Register<MainWindow, UpdateCommandMessage>(this.UpdateCommandMessageReceived);
        }

        private bool UpdateCommandMessageReceived(IMessageContainer arg)
        {
            if (arg.Content is UserItem userItem)
            {
                this.Title = $"Mini Messenger - {userItem.Username}";
                return true;
            }

            this.Title = $"Mini Messenger - ERROR";
            return false;
        }
    }
}
