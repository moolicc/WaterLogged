using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WaterLogged.Serialization
{
    [Serializable]
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
