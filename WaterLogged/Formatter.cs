using System.Collections.Generic;

namespace WaterLogged
{
    public abstract class Formatter
    {
        public abstract bool SupportsTemplating { get; }

        public virtual string Transform(string template, Log log, string tag, Dictionary<string, string> overrides)
        {
            return Transform(log, template, tag, overrides);
        }

        public abstract string Transform(Log log, string input, string tag, Dictionary<string, string> overrides);
    }
}
