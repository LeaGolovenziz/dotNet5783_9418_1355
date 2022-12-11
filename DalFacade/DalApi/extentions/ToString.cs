using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    internal static class ToString
    {
        public static string ToStringProperty<Item>(this Item _item, string result = "")
        {
            IEnumerable<PropertyInfo> propertyInfos = _item!.GetType().GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                var value = propertyInfo.GetValue(_item, null);
                if (value is IEnumerable && value is not string)
                {
                    IEnumerable items = (IEnumerable)value;

                    foreach (var item in items)
                        item.ToStringProperty(result);
                }
                else
                    result += $"{propertyInfo.Name}: {value}\n";

            }
            return result;
        }
    }
}
