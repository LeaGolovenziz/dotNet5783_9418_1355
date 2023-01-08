using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL.OrderWindows.Convertors
{
    public class BooleanTrueToVisibilityConverter : IValueConverter
    {
        // return "Visible" if the value is true and "Hidden" if false
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
              => (bool)value ? Visibility.Visible : Visibility.Hidden;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanFalseToVisibilityConverter : IValueConverter
    {
        // return "Visible" if the value is true and "Hidden" if false
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
              => !(bool)value ? Visibility.Visible : Visibility.Hidden;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
