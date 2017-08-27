using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public class Formatter
    {
        public virtual string Transform(Log log, string input, string tag, Dictionary<string, string> overrides)
        {
            return input;
        }
    }
}
