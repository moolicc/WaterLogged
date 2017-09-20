using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterLogged.Serialization.StringConversion
{
    /// <inheritdoc />
    public class ArrayConverter : IStringConverter
    {
        public bool SupportsType(Type type)
        {
            return type.IsArray;
        }

        public object Convert(string input, Type type)
        {
            if (type.IsArray)
            {
                List<object> list = new List<object>();
                string[] parts = input.Split('|');
                foreach (var part in parts)
                {
                    list.Add(Converter.Convert(part, type.GetElementType()));
                }
                return list.ToArray();
            }
            return null;
        }

        public string Convert(object input)
        {
            var array = (object[]) input;
            StringBuilder builder = new StringBuilder();

            foreach (var item in array)
            {
                builder.Append(Converter.Convert(item));
                builder.Append('|');
            }

            return builder.ToString().TrimEnd('|');
        }
    }
}
