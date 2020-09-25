using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;

namespace PocSecurityDotNetFramework.Http
{
    [TypeConverter(typeof(Converter))]
    public struct Sensitive
    {
        public string Value { get; }

        public Sensitive(int value)
        {
            Value = value.ToString();
        }

        public Sensitive(string value)
        {
            Value = value;
        }

        public static implicit operator Sensitive(int number)
        {
            return new Sensitive(number);
        }

        public static implicit operator Sensitive(string str)
        {
            return new Sensitive(str);
        }

        public static implicit operator int(Sensitive type)
        {
            int.TryParse(type.Value, out int result);
            return result;
        }

        public static implicit operator string(Sensitive type)
        {
            return type.Value;
        }

        public override string ToString()
        {
            return Value;
        }

        public bool HasValue()
        {
            string str = this;
            int num = this;

            return !string.IsNullOrEmpty(str) || num != 0;
        }

        public class Converter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(string);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                return value.ToString();
            }
        }
    }
}