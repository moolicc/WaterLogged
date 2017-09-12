using System;

namespace WaterLogged.Serialization.StringConversion
{
    public class DateTimeConverter : IStringConverter
    {
        public bool SupportsType(Type type)
        {
            return type == typeof(DateTime) || type == typeof(TimeSpan);
        }

        public object Convert(string input, Type type)
        {
            if (type == typeof(DateTime))
            {
                return DateTime.Parse(input);
            }
            else if (type == typeof(TimeSpan))
            {
                return TimeSpan.Parse(input);
            }
            return null;
        }

        public string Convert(object input)
        {
            return input.ToString();
        }
    }
}
