using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WaterLogged.Serialization.StringConversion
{
    public class NumericConverters : IStringConverter
    {
        public bool SupportsType(Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte)
                   || type == typeof(short) || type == typeof(ushort)
                   || type == typeof(int) || type == typeof(uint)
                   || type == typeof(long) || type == typeof(ulong)
                   || type == typeof(float) || type == typeof(double)
                   || type == typeof(decimal);
        }

        public object Convert(string input, Type type)
        {
            if (type == typeof(byte))
            {
                return byte.Parse(input, NumberStyles.Integer | NumberStyles.AllowHexSpecifier | NumberStyles.AllowThousands);
            }
            if (type == typeof(sbyte))
            {
                return sbyte.Parse(input, NumberStyles.Integer | NumberStyles.AllowHexSpecifier | NumberStyles.AllowThousands);
            }
            if (type == typeof(short))
            {
                return short.Parse(input, NumberStyles.Integer | NumberStyles.AllowHexSpecifier | NumberStyles.AllowThousands);
            }
            if (type == typeof(ushort))
            {
                return ushort.Parse(input, NumberStyles.Integer | NumberStyles.AllowHexSpecifier | NumberStyles.AllowThousands);
            }
            if (type == typeof(int))
            {
                return int.Parse(input, NumberStyles.Integer | NumberStyles.AllowHexSpecifier | NumberStyles.AllowThousands);
            }
            if (type == typeof(uint))
            {
                return uint.Parse(input, NumberStyles.Integer | NumberStyles.AllowHexSpecifier | NumberStyles.AllowThousands);
            }
            if (type == typeof(long))
            {
                return long.Parse(input, NumberStyles.Integer | NumberStyles.AllowHexSpecifier | NumberStyles.AllowThousands);
            }
            if (type == typeof(ulong))
            {
                return ulong.Parse(input, NumberStyles.Integer | NumberStyles.AllowHexSpecifier | NumberStyles.AllowThousands);
            }
            if (type == typeof(float))
            {
                return float.Parse(input, NumberStyles.Number | NumberStyles.AllowHexSpecifier);
            }
            if (type == typeof(double))
            {
                return double.Parse(input, NumberStyles.Number | NumberStyles.AllowHexSpecifier);
            }
            if (type == typeof(decimal))
            {
                return decimal.Parse(input, NumberStyles.Number | NumberStyles.AllowHexSpecifier);
            }
            return null;
        }
    }
}
