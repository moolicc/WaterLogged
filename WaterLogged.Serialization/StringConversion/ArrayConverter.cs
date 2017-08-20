using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Serialization.StringConversion
{
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
    }
}
