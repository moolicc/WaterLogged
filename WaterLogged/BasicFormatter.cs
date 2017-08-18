using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public class BasicFormatter : Formatter
    {
        public string Format { get; set; }

        public BasicFormatter()
            : this("[{log}] [{tag}] [{date}] {message}")
        {
            
        }

        public BasicFormatter(string format)
        {
            Format = format;
        }

        public override string Transform(Log log, string input, string tag)
        {
            //0 = log name
            //1 = message
            //2 = tag
            //3 = date
            string format = Format;
            format = format.Replace("{log}", "{0}");
            format = format.Replace("{message}", "{1}");
            format = format.Replace("{tag}", "{2}");
            format = format.Replace("{date", "{3");

            return string.Format(format, log.Name, input, tag, DateTime.Now);
        }
    }
}
