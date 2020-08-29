using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PocSecurity.Sensitive;
using PocSecurity.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PocSecurity.Filter.SensitiveFilter
{
    public class SensitiveFilterHelper
    {
        public static bool IsSensitiveClass(object value)
        {
            if (value == null) return false;

            Attribute isSensitiveClass;

            if (IsList(value))
            {
                var type = value.GetType().GetGenericArguments().Single();
                isSensitiveClass = Attribute.GetCustomAttribute(type, typeof(SensitiveClassAttribute));
            }
            else
            {
                isSensitiveClass = Attribute.GetCustomAttribute(value.GetType(), typeof(SensitiveClassAttribute));
            }

            return isSensitiveClass != null;
        }

        public static void EncryptProperties(object value, ICipherService cipherService)
        {
            if (IsSensitiveClass(value))
            {
                var properties = GetProperties(value);

                foreach (var property in properties)
                {
                    if (IsList(value))
                    {
                        foreach (var item in (IList)value)
                        {
                            var propertyValue = GetValueFromProperty(item, property);
                            property.SetValue(item, cipherService.Encrypt(propertyValue));
                        }
                    }
                    else
                    {
                        var propertyValue = GetValueFromProperty(value, property);
                        property.SetValue(value, cipherService.Encrypt(propertyValue));
                    }
                }
            }
        }

        public static void DecryptProperties(object value, ControllerParameterDescriptor parameter, ActionExecutingContext context, ICipherService cipherService)
        {
            if(IsSensitiveClass(value))
            {
                var properties = GetProperties(value);

                foreach (var property in properties)
                {
                    if (IsList(value))
                    {
                        foreach (var item in (IList)value)
                        {
                            var propertyValue = GetValueFromProperty(item, property);
                            property.SetValue(item, cipherService.Decrypt(propertyValue));
                        }
                    }
                    else
                    {
                        var propertyValue = GetValueFromProperty(value, property);
                        property.SetValue(value, cipherService.Decrypt(propertyValue));
                    }
                }
            }
            else
            {
                context.ActionArguments[parameter.Name] = cipherService.Decrypt((string)context.ActionArguments[parameter.Name]);
            }
        }

        public static IEnumerable<PropertyInfo> GetProperties(object value)
        {
            Type type;
            if (IsList(value))
                type = value.GetType().GetGenericArguments().Single();
            else
                type = value.GetType();

            return type
                .GetProperties()
                .Where(prop => prop.IsDefined(typeof(SensitiveFieldAttribute), false));
        }

        public static string GetValueFromProperty(object property, PropertyInfo propertyInfo)
        {
            return (string)propertyInfo.GetValue(property, null);
        }

        public static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
    }
}
