using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace MiniMessenger.Components.Messenger
{
    public class MessengerManager
    {
        private static MessengerManager _instance;

        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();


        public int Interval { get; private set; }

        private MessengerManager()
        {
            this._dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            this._dispatcherTimer.Tick += this._dispatcherTimer_Tick;

        }

        ~MessengerManager()
        {
            this._dispatcherTimer.Stop();
            this._dispatcherTimer = null;
        }

        public static MessengerManager GetInstance()
        {
            if(_instance == null)
            {
                _instance = new MessengerManager();
            }

            return _instance;
        }

        public void Start(int interval)
        {
            this.Interval = interval;
            this._dispatcherTimer.Start();
        }

        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (this._dispatcherTimer == null)
            {
                return;
            }

            if (this.Interval <= 0)
            {
                this.Interval = 1;
            }

            this._dispatcherTimer.Interval = TimeSpan.FromSeconds(this.Interval);

            // update open view

            this.SendUpdate();
        }

        private IEnumerable<Action> _updateableList = new List<Action>();

        public void Add(Action update)
        {
            this._updateableList = this._updateableList.Append(update);
        }

        private void SendUpdate()
        {
            foreach (var item in this._updateableList)
            {
                item.Invoke();
            }
        }
    }
}
