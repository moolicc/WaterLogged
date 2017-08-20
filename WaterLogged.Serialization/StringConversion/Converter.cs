using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Serialization.StringConversion
{
    public static class Converter
    {
        public static List<IStringConverter> Converters { get; private set; }

        static Converter()
        {
            Converters = new List<IStringConverter>();
        }

        public static object Convert(string input, Type target)
        {
            foreach (var stringConverter in Converters)
            {
                if (stringConverter.SupportsType(target))
                {
                    return stringConverter.Convert(input, target);
                }
            }
            throw new KeyNotFoundException("Failed to find a StringConverter for the specified target type.");
        }
    }
}
