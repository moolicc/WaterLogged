using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public class FormatData
    {
        public Log Log { get; private set; }
        public Formatter Formatter { get; private set; }
        public string Message { get; private set; }
        public string Tag { get; private set; }
        public string Argument { get; private set; }

        public FormatData(Log log, Formatter formatter, string message, string tag, string argument)
        {
            Log = log;
            Formatter = formatter;
            Message = message;
            Tag = tag;
            Argument = argument;
        }
    }
}
