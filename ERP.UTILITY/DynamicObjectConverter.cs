using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UTILITY
{
    public static class DynamicObjectConverter
    {
        public static T ConvertToType<T>(dynamic dynamicObject) where T : new()
        {
            // Create an instance of the target type
            T targetObject = new T();

            // Check if the dynamicObject is an ExpandoObject
            if (dynamicObject is IDictionary<string, object> dictionary)
            {
                // Get the properties of the target type
                var properties = typeof(T).GetProperties();

                // Map properties from dynamicObject to the targetObject
                foreach (var property in properties)
                {
                    // Check if the dynamicObject contains a property with the same name
                    if (dictionary.ContainsKey(property.Name))
                    {
                        // Get the value from dynamicObject
                        object dynamicValue = dictionary[property.Name];

                        // Check if the dynamic value is null
                        if (dynamicValue != null)
                        {
                            try
                            {
                                // Convert the dynamic value to the property type
                                object convertedValue = Convert.ChangeType(dynamicValue, property.PropertyType);

                                // Set the property value using reflection
                                property.SetValue(targetObject, convertedValue);
                            }
                            catch (InvalidCastException)
                            {
                                // Handle conversion errors (e.g., if the types are not compatible)
                                // You can choose to log or handle this differently based on your requirements.
                            }
                        }
                    }
                }
            }

            return targetObject;
        }


    }

}
