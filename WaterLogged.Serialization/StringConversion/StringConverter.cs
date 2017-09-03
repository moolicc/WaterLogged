using System;

namespace WaterLogged.Serialization.StringConversion
{
    public class StringConverter : IStringConverter
    {
        public bool SupportsType(Type type)
        {
            return type == typeof(string) || type == typeof(char);
        }

        public object Convert(string input, Type type)
        {
            if (type == typeof(string))
            {
                return input;
            }
            else if (type == typeof(char))
            {
                if (input.Length > 0)
                {
                    return input[0];
                }
                return '\0';
            }
            return null;
        }

        public string Convert(object input)
        {
            return input.ToString();
        }
    }
}
