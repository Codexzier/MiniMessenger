using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MiniMessenger.Components.Ui.Smily
{
    /// <summary>
    /// Interaction logic for SmilyEmoteControl.xaml
    /// </summary>
    public partial class SmilyEmoteControl : UserControl
    {
        public static readonly DependencyProperty SmilyProperty = DependencyProperty.Register(
                                                        "Smily",
                                                        typeof(SmilyEmote),
                                                        typeof(SmilyEmoteControl),
                                                        new PropertyMetadata(default(SmilyEmote), 
                                                            new PropertyChangedCallback((obj, dd) =>
                                                            {
                                                                Debug.WriteLine("Test Smily changed property");
                                                            }
                                                            )));

        public SmilyEmote Smily
        {
            get => (SmilyEmote)this.GetValue(SmilyProperty);
            set => this.SetValue(SmilyProperty, value);
        }

        public SmilyEmoteControl()
        {
            this.InitializeComponent();
        }
    }
}
