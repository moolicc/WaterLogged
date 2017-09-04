using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public class LogicFormatter : Formatter
    {
        public string Format { get; set; }

        public override string Transform(Log log, string input, string tag, Dictionary<string, string> overrides)
        {
            return base.Transform(log, input, tag, overrides);
        }
    }
}
