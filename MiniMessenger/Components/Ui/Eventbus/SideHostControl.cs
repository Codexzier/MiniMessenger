
using MiniMessenger.Components.Ui.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MiniMessenger.Components.Ui.Eventbus
{
    public class SideHostControl : Control
    {
        public static IList<SideHostTypeChannel> TypeViews = new List<SideHostTypeChannel>();

        public static bool IsViewOpen(Type view, int channel)
        {
            return TypeViews.Any(a => a.Channel == channel && a.TypeView.Equals(view));
        }


        private ContentPresenter _presenter;

        public int Channel { get; set; }


        static SideHostControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(SideHostControl), new FrameworkPropertyMetadata(typeof(SideHostControl)));

        public SideHostControl()
        {
            EventbusManager.OpenViewEvent += this._eventbus_OpenViewEvent;
            EventbusManager.CloseViewEvent += this.EventbusManager_CloseViewEvent;
        }



        public override void OnApplyTemplate() => this._presenter = this.GetContentPresenter<ContentPresenter>();


        private void _eventbus_OpenViewEvent(object obj, int channel)
        {
            if (channel != this.Channel)
            {
                return;
            }

            if (this._presenter.Content != null &&
               this._presenter.Content is IDisposable disposable)
            {
                disposable.Dispose();
            }

            this.RemoveViewFromChannel(obj, channel);

            SideHostControl.TypeViews.Add(new SideHostTypeChannel(channel, obj.GetType()));

            this._presenter.Content = (Control)obj;
        }

        

        private void EventbusManager_CloseViewEvent(Type view, int channel)
        {
            if (channel != this.Channel)
            {
                return;
            }

            if (this._presenter.Content == null)
            {
                return;
            }

            if (!(this._presenter.Content is IDisposable disposable))
            {
                return;
            }

            if (this.RemoveViewFromChannel(view, channel))
            {
                disposable.Dispose();
                this._presenter.Content = null;
            }
        }

        private bool RemoveViewFromChannel(object obj, int channel)
        {
            var d = SideHostControl.TypeViews.FirstOrDefault(a => {
                if (a.Channel != channel)
                {
                    return false;
                }

                if (a.TypeView.Equals(obj.GetType()))
                {
                    return true;
                }

                if (obj is Type view)
                {
                    var presenterHasType = this._presenter.Content.GetType().Name;
                    var viewTypeName = view.Name;

                    return presenterHasType.Equals(viewTypeName);
                }

                return false;
            });

            if (d != null)
            {
                SideHostControl.TypeViews.Remove(d);
                return true;
            }

            return false;
        }
    }
}
