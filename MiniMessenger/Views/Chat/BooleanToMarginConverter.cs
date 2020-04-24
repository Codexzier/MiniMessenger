using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MiniMessenger.Views.Chat
{
    public class BooleanToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool b)
            {
                return b ? new Thickness(15, 0, 0, 0) : new Thickness();
            }

            return new Thickness();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
