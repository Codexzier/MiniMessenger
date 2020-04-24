using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MiniMessenger.Views.Chat
{
    public class BoolenToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool b)
            {
                return b ? Brushes.GreenYellow : Brushes.LightGreen;
            }

            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
