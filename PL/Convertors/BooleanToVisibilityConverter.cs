﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL.OrderWindows.Convertors
{
    public class BooleanFalseToVisibilityConverter : IValueConverter
    {
        // return "Visible" if the value is false and "Hidden" if true
        public object Convert(object value, Type targetType, object parameter,CultureInfo culture)
              => !(bool)value ? Visibility.Visible : Visibility.Hidden;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanTrueToVisibilityConverter : IValueConverter
    {
        // return "Visible" if the value is true and "Hidden" if false
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
              =>  (bool)value ? Visibility.Visible : Visibility.Hidden;
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value != "0")
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
