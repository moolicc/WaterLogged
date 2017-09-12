using System;

namespace WaterLogged.Serialization.StringConversion
{
    public interface IStringConverter
    {
        bool SupportsType(Type type);
        object Convert(string input, Type type);
        string Convert(object input);
    }
}
