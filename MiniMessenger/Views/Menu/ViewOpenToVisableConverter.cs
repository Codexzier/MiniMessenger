using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MiniMessenger.Views.Menu
{
    public class ViewOpenToVisableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is ViewOpen vo && parameter is string viewName)
            {
                if (vo.ToString().Equals(viewName))
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
