using PocSecurityDotNetFramework.Sensitive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PocSecurityDotNetFramework.Http
{
    public class SensitiveAnnotationHelper
    {
        public bool IsSensitiveClass<T>(T obj)
        {
            if (obj == null)
            {
                return false;
            }

            var isSensitiveType = false;

            if (IsList(obj))
            {
                var type = obj.GetType().GetGenericArguments().SingleOrDefault();
                isSensitiveType = IsSensitiveType(type);
            }
            else
            {
                isSensitiveType = IsSensitiveType(obj.GetType());
            }

            return isSensitiveType;
        }

        public bool IsSensitiveType(Type type)
        {
            var sensitiveClassAttribute = Attribute.GetCustomAttribute(type, typeof(SensitiveClassAttribute));
            return sensitiveClassAttribute != null;
        }

        public bool IsList<T>(T obj)
        {
            if (obj == null) return false;
            return obj is IList &&
                   obj.GetType().IsGenericType &&
                   obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public IEnumerable<PropertyInfo> GetSensitiveProperties<T>(T value)
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

        public IEnumerable<PropertyInfo> GetSensitiveProperties<T>(Type type)
        {
            return type
                .GetProperties()
                .Where(prop => prop.IsDefined(typeof(SensitiveFieldAttribute), false));
        }
    }
}