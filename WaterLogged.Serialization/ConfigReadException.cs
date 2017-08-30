using System;

namespace WaterLogged.Serialization
{
    public class ConfigReadException : Exception
    {
        public ConfigReadException()
        {
        }

        public ConfigReadException(string message) : base(message)
        {
        }

        public ConfigReadException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
