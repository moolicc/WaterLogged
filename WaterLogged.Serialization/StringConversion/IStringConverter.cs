using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Serialization.StringConversion
{
    public interface IStringConverter
    {
        bool SupportsType(Type type);
        object Convert(string input, Type type);
    }
}
