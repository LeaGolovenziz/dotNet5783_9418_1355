using System.Reflection;

namespace BO
{
    /// <summary>
    /// Class of copying functions extentions
    /// </summary>
    internal static class Copy
    {
        /// <summary>
        /// Generic ectention copying function of struct Source to class Target
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>Target</returns>
        public static Target CopyPropTo<Source, Target>(this Source source, Target target)
        {
            // Creating a dictionary of couples - property name and property information of the target object
            Dictionary<string, PropertyInfo> propertyInfoTarget = target!.GetType().GetProperties()
                .ToDictionary(key => key.Name, value => value);

            // Creating an ienumerable of source's properties information
            IEnumerable<PropertyInfo> propertyInfoSource = source!.GetType().GetProperties();

            // Go over the ienumerable of source's properties information
            foreach (var item in propertyInfoSource)
            {
                // If there is in the dictionary a property that have the same name as item (property of source) 
                // and if the type of item is string or not a class (which means that we can copy it)
                if (propertyInfoTarget.ContainsKey(item.Name) && (item.PropertyType == typeof(string) || !item.PropertyType.IsClass))
                {
                    // Find the type of the source's and target's properties
                    Type typeSource = Nullable.GetUnderlyingType(item.PropertyType)!;
                    Type typeTarget = Nullable.GetUnderlyingType(propertyInfoTarget[item.Name].PropertyType)!;

                    // Get the value of source's property
                    object value = item.GetValue(source)!;

                    if (value is not null)
                    {
                        // If the type of the source is enum - copy its value to the matching targets's property 
                        if (typeSource is not null && typeSource.IsEnum)
                            propertyInfoTarget[item.Name].SetValue(target, Enum.ToObject(typeTarget, value));

                        // If the types of the properties are equal - copy theres value
                        else if (propertyInfoTarget[item.Name].PropertyType == item.PropertyType)
                            propertyInfoTarget[item.Name].SetValue(target, value);
                    }
                }
            }
            return target;
        }

        /// <summary>
        /// Generic ectention copying function of class Source to struct Target
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>Target</returns>
        public static Target CopyPropToStruct<Source, Target>(this Source source, Target target) where Target : struct
        {
            object obj = target;
            source.CopyPropTo(obj);
            return (Target)obj;
        }

        /// <summary>
        ///  Generic ectention copying function of ienumerable of struct to ienumerable of class
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="sources"></param>
        /// <returns>IEnumerable<Target></returns>
        public static IEnumerable<Target> CopyListTo<Source, Target>(this IEnumerable<Source> sources) where Target : new()
        => from source in sources
           select source.CopyPropTo(new Target());

        /// <summary>
        /// Generic ectention copying function of ienumerable of class to ienumerable of struct
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="sources"></param>
        /// <returns>IEnumerable<Target></returns>
        public static IEnumerable<Target> CopyListToStruct<Source, Target>(this IEnumerable<Source> sources) where Target : struct
            => from source in sources
               select source.CopyPropTo(new Target());
    }
}
