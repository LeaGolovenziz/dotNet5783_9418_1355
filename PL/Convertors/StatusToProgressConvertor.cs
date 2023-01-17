using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PL.Convertors
{
    public class StatusToProgressConvertor: IValueConverter
    {
        // return the appropriate color according to the status
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Enums.OrderStatus status = (BO.Enums.OrderStatus)Enum.Parse(typeof(BO.Enums.OrderStatus), value.ToString()!);

            if (status == BO.Enums.OrderStatus.Confirmed)
                return 15;
            else if (status == BO.Enums.OrderStatus.Sent)
                return 50;
            return 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

