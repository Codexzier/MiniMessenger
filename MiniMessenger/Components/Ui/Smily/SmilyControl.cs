using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Components.Ui.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MiniMessenger.Components.Ui.Smily
{

    public class SmilyControl : Control, IDisposable, INotifyPropertyChanged
    {
        private Image _image;
        private SmilyEmote _smile;

        static SmilyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SmilyControl), new FrameworkPropertyMetadata(typeof(SmilyControl)));
        }

        private readonly EventbusManager _eventbus;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public SmilyControl()
        {
            this._eventbus = EventbusManager.GetEventbus();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._image = this.GetContentPresenter<Image>();
        }

        public SmilyEmote Smile
        {
            get => this._smile; 
            set
            {
                this._smile = value;

                this.UpdateIcon(value);

                this.OnPropertyChanged(nameof(this.Smile));
            }
        }

        private void UpdateIcon(SmilyEmote value)
        {
            if(this._image == null)
            {
                this._image = this.GetContentPresenter<Image>();
                Debug.WriteLine("image was not set");
                return;
            }

            switch (value)
            {
                case SmilyEmote.Angry:
                    this._image.Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}/Assets/Smilies/angry.gif"));
                    break;
                case SmilyEmote.Annoyed:
                    break;
                default:
                    break;
            }
        }

        public void Dispose() => this._eventbus.Deregister<SmilyControl>();
    }
}
