using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MiniMessenger.Views.Userlist
{
    public class BooleanToOnlineStateBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool b)
            {
                return b ? Brushes.Green : Brushes.DarkGray;
            }

            return Brushes.Gray;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
