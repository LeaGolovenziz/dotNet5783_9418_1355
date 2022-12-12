using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Class of ToString functions extentions
    /// </summary>
    internal static class ToString
    {
        /// <summary>
        /// Generic extention function that returns string of the object's details
        /// </summary>
        /// <typeparam name="Item"></typeparam>
        /// <param name="_item"></param>
        /// <param name="result"></param>
        /// <returns>string</returns>
        public static string ToStringProperty<Obj>(this Obj obj, string result = "")
        {
            // creats ienumerable of the object properties
            IEnumerable<PropertyInfo> propertyInfos = obj!.GetType().GetProperties();

            // go over all the properties
            foreach (var propertyInfo in propertyInfos)
            {
                // get the proprty's value
                var value = propertyInfo.GetValue(obj, null);
                // if the value is a collection print every item in recursion
                if (value is IEnumerable && value is not string)
                {
                    IEnumerable items = (IEnumerable)value;

                    foreach (var item in items)
                        item.ToStringProperty(result);
                }
                // else peint the property's name and value
                else
                    result += $"{propertyInfo.Name}: {value}\n";

            }
            return result;
        }
    }
}
