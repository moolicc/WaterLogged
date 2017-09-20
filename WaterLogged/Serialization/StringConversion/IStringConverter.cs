using System;

namespace WaterLogged.Serialization.StringConversion
{
    /// <summary>
    /// Specifies a definition for an object/string converter.
    /// </summary>
    public interface IStringConverter
    {
        /// <summary>
        /// Returns true if the specified type can be converted by this stringconverter.
        /// </summary>
        /// <param name="type">The type to check.</param>
        bool SupportsType(Type type);
        /// <summary>
        /// Returns a strongly-typed object based on a string-representation of an object.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="type">The type to return.</param>
        object Convert(string input, Type type);
        /// <summary>
        /// Returns a string representation of the specified strongly-typed object.
        /// </summary>
        /// <param name="input">The object to convert.</param>
        string Convert(object input);
    }
}
