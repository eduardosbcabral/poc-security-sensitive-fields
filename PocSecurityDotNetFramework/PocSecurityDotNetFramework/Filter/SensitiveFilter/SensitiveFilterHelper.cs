using PocSecurityDotNetFramework.Sensitive;
using PocSecurityDotNetFramework.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

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

        public static void EncryptProperties(object value, string secret)
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
                            var encrypted = RijndaelCipherService.Encrypt(propertyValue, secret);
                            var encoded = HttpUtility.UrlEncode(encrypted);
                            property.SetValue(item, encoded);
                        }
                    }
                    else
                    {
                        var propertyValue = GetValueFromProperty(value, property);
                        var encrypted = RijndaelCipherService.Encrypt(propertyValue, secret);
                        var encoded = HttpUtility.UrlEncode(encrypted);
                        property.SetValue(value, encoded);
                    }
                }
            }
        }

        public static void DecryptProperties(object value, string parameterName, HttpActionContext context, string secret)
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
                            var decoded = HttpUtility.UrlDecode(propertyValue);
                            var decrypted = RijndaelCipherService.Decrypt(decoded, secret);
                            property.SetValue(item, decrypted);
                        }
                    }
                    else
                    {
                        var propertyValue = GetValueFromProperty(value, property);
                        var decoded = HttpUtility.UrlDecode(propertyValue);
                        var decrypted = RijndaelCipherService.Decrypt(decoded, secret);
                        property.SetValue(value, decrypted);
                    }
                }
            }
            else
            {
                context.ActionArguments[parameterName] = HttpUtility.UrlEncode(RijndaelCipherService.Decrypt((string)context.ActionArguments[parameterName], secret));
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
