using MiniMessenger.Components.Ui.Eventbus;
using MiniMessenger.Views.Base;

namespace MiniMessenger.Views.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        private ViewOpen _viewOpened = ViewOpen.Main;

        public ViewOpen ViewOpened { get => _viewOpened;
            set
            {
                _viewOpened = value;
                this.OnNotifyPropertyChanged(nameof(this.ViewOpened));
            }
        }
    }
}
