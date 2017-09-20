using System;

namespace WaterLogged.Serialization.StringConversion
{
    /// <inheritdoc />
    public class BoolConverter : IStringConverter
    {
        public bool SupportsType(Type type)
        {
            return type == typeof(bool);
        }

        public object Convert(string input, Type type)
        {
            if (string.Equals(input, "yes", StringComparison.OrdinalIgnoreCase)
                || string.Equals(input, "on", StringComparison.OrdinalIgnoreCase)
                || string.Equals(input, "true", StringComparison.OrdinalIgnoreCase)
                || string.Equals(input, "1"))
            {
                return true;
            }
            return false;
        }

        public string Convert(object input)
        {
            return input.ToString();
        }
    }
}
