using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL.Convertors
{
    public class StatusToColorConvertor:IValueConverter
    {
        // return the appropriate color according to the status
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Enums.OrderStatus status = (BO.Enums.OrderStatus)value;

            if (status == BO.Enums.OrderStatus.Confirmed)
                return Brushes.Red;
            else if (status == BO.Enums.OrderStatus.Sent)
                return Brushes.Orange;
            return Brushes.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
