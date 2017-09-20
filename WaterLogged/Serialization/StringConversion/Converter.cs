using System;
using System.Collections.Generic;

namespace WaterLogged.Serialization.StringConversion
{
    /// <summary>
    /// Implements functions for converting objects to and from strings.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// A list of converters to query when a conversion occurs.
        /// </summary>
        public static List<IStringConverter> Converters { get; private set; }
        
        static Converter()
        {
            Converters = new List<IStringConverter>
            {
                new ArrayConverter(),
                new BoolConverter(),
                new DateTimeConverter(),
                new NumericConverters(),
                new StringConverter()
            };
        }

        /// <summary>
        /// Returns a strongly-typed object based on a string-representation of an object.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="target">The type to return.</param>
        /// <exception cref="KeyNotFoundException"></exception>
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

        /// <summary>
        /// Returns a string representation of the specified strongly-typed object.
        /// </summary>
        /// <param name="input">The object to convert.</param>
        /// <exception cref="KeyNotFoundException"></exception>
        public static string Convert(object input)
        {
            foreach (var stringConverter in Converters)
            {
                if (stringConverter.SupportsType(input.GetType()))
                {
                    return stringConverter.Convert(input);
                }
            }
            throw new KeyNotFoundException("Failed to find a StringConverter for the specified type.");
        }
    }
}
