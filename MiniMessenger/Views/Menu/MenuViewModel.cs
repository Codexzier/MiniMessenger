using MiniMessenger.Views.Base;

namespace MiniMessenger.Views.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        public int Interval { get; set; } = 1;

        public ViewOpen ViewOpened { get; set; } = ViewOpen.Main;
    }
}
