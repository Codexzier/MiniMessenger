using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace MiniMessenger.Components.Ui.Eventbus
{
    public static class EventbusManager
    {
        public static int RegisteredCount => EventbusManagerInternal.GetEventbus().RegisteredCount;

        public static int RegisteredCountAll => EventbusManagerInternal.GetEventbus().RegisteredCountAll;

        public static int RegisteredCountByView<TView>() where TView : DependencyObject => EventbusManagerInternal.GetEventbus().RegisteredCountByView<TView>();
        internal static ViewOpen GetViewOpened(int channel) => EventbusManagerInternal.GetEventbus().GeViewOpenend(channel);


        /// <summary>
        /// create new internal instance host for message event. 
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="receiverMethod"></param>
        public static void Register<TView, TMessage>(Func<IMessageContainer, bool> receiverMethod)
            where TView : DependencyObject
            where TMessage : IMessageContainer => EventbusManagerInternal.GetEventbus().Register<TView, TMessage>(receiverMethod);

        /// <summary>
        /// Close target view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        public static void CloseView<TView>(int channel) => EventbusManagerInternal.GetEventbus().CloseView<TView>(channel);

        /// <summary>
        /// Deregister closing content. Every view need an dispose interface to cleanup events and unused references.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        public static void Deregister<TView>() => EventbusManagerInternal.GetEventbus().Deregister<TView>();


        internal static bool IsViewOpen(Type type, int channel) => EventbusManagerInternal.GetEventbus().IsViewOpen(type, channel);

        /// <summary>
        /// Open new instance of a view. The view must setup the viewModel to the DataContext.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        public static void OpenView<TView>(int channel) => EventbusManagerInternal.GetEventbus().OpenView<TView>(channel);

        public static bool Send<TView, TMessageType>(TMessageType message, int channel, bool openView = false) where TMessageType : IMessageContainer => EventbusManagerInternal.GetEventbus().Send<TView, TMessageType>(message,channel, openView);

        /// <summary>
        /// Eventbus singleton. can only one instance exist for the application
        /// </summary>
        private class EventbusManagerInternal
        {
            private static EventbusManagerInternal eventbus;

            private readonly IDictionary<Type, IList<IMessageEventHost>> _messageContainers = new Dictionary<Type, IList<IMessageEventHost>>();

            private EventbusManagerInternal() { }

            public static EventbusManagerInternal GetEventbus()
            {
                if (eventbus == null)
                {
                    eventbus = new EventbusManagerInternal();
                }

                return eventbus;
            }

            public int RegisteredCount => this._messageContainers.Count;

            public int RegisteredCountAll => this._messageContainers.Sum(c => c.Value.Count);

            public int RegisteredCountByView<TView>() where TView : DependencyObject
            {
                if (this._messageContainers.Count == 0)
                {
                    return 0;
                }

                return this._messageContainers[typeof(TView)].Count;
            }

            /// <summary>
            /// create new internal instance host for message event. 
            /// </summary>
            /// <typeparam name="TView"></typeparam>
            /// <typeparam name="TMessage"></typeparam>
            /// <param name="receiverMethod"></param>
            public void Register<TView, TMessage>(Func<IMessageContainer, bool> receiverMethod)
                where TView : DependencyObject
                where TMessage : IMessageContainer
            {

                var host = new MessageEventHost<TView, TMessage>();
                host.Subscribe(receiverMethod);

                if (!this._messageContainers.ContainsKey(host.ViewType))
                {
                    this._messageContainers.Add(host.ViewType, new List<IMessageEventHost>());
                }

                if (this._messageContainers[host.ViewType].Any(a => a.MessageType == host.MessageType))
                {
                    throw new EventbusException($"can not register one moretime to the viewType {host.ViewType.Name} about message type: {host.MessageType.Name}");
                }

                this._messageContainers[host.ViewType].Add(host);
            }

            /// <summary>
            /// Deregister closing content. Every view need an dispose interface to cleanup events and unused references.
            /// </summary>
            /// <typeparam name="TView"></typeparam>
            public void Deregister<TView>()
            {
                if (this._messageContainers.ContainsKey(typeof(TView)))
                {
                    foreach (var item in this._messageContainers[typeof(TView)])
                    {
                        item.RemoveEventMethod();
                    }
                    this._messageContainers.Remove(typeof(TView));
                }
            }

            internal bool IsViewOpen(Type type, int channel)
            {
                return SideHostControl.IsViewOpen(type, channel);
            }

            internal ViewOpen GeViewOpenend(int channel)
            {
                if (SideHostControl.TypeViews.All(a => a.Channel != channel) || !SideHostControl.TypeViews.Any(a => a.Channel == channel && a.TypeView.Name.EndsWith("View")))
                {
                    return ViewOpen.Nothing;
                }

                var typeView = SideHostControl.TypeViews.FirstOrDefault(f => f.Channel == channel);

                var viewName = typeView.TypeView.Name.Remove(typeView.TypeView.Name.Length - 4); //SideHostControl.TypeView.Name.Remove(SideHostControl.TypeView.Name.Length - 4);

                if(Enum.TryParse(typeof(ViewOpen), viewName, out object result))
                {
                    return (ViewOpen)result;
                }

                throw new Exception($"Type has no enum: {typeView.TypeView.Name}");
            }

            /// <summary>
            /// Open new instance of a view. The view must setup the viewModel to the DataContext.
            /// </summary>
            /// <typeparam name="TView"></typeparam>
            public void OpenView<TView>(int channel)
            {
                this.OpenViewEvent?.Invoke((TView)Activator.CreateInstance(typeof(TView)), channel);
            }

            internal void CloseView<TView>(int channel)
            {
                this.CloseViewEvent?.Invoke(typeof(TView), channel);
            }

            public bool Send<TView, TMessageType>(TMessageType message, int channel = 0, bool openView = false) where TMessageType : IMessageContainer
            {
                foreach (var itemEventHosts in this._messageContainers)
                {
                    if (itemEventHosts.Key != typeof(TView))
                    {
                        continue;
                    }

                    foreach (var itemEventHost in itemEventHosts.Value)
                    {
                        if (itemEventHost.MessageType != message.GetType())
                        {
                            continue;
                        }

                        itemEventHost.Send(message);
                        return true;
                    }
                }

                if (this.OpenNewView<TView, TMessageType>(message, openView, channel))
                {
                    return true;
                }

                throw new EventbusException($"Not found or registered. View: {typeof(TView).Name}, {typeof(TMessageType).Name}");
            }

            private bool OpenNewView<TView, TMessageType>(TMessageType message, bool openView, int channel) where TMessageType : IMessageContainer
            {
                if (openView)
                {
                    this.OpenView<TView>(channel);

                    if (this.Send<TView, TMessageType>(message, channel))
                    {
                        return true;
                    }
                }

                return false;
            }

            public event OpenViewEventHandler OpenViewEvent;

            public event CloseViewEventHandler CloseViewEvent;
        }

        public delegate void OpenViewEventHandler(object obj, int channel);

        public static event OpenViewEventHandler OpenViewEvent
        {
            add { EventbusManagerInternal.GetEventbus().OpenViewEvent += value; }
            remove { EventbusManagerInternal.GetEventbus().OpenViewEvent -= value; }
        }

        public delegate void CloseViewEventHandler(Type view, int channel);
        public static event CloseViewEventHandler CloseViewEvent
        {
            add { EventbusManagerInternal.GetEventbus().CloseViewEvent += value;  }
            remove { EventbusManagerInternal.GetEventbus().CloseViewEvent -= value; }
        }
    }
}
