using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Serialization
{
    public class Configuration
    {
        public List<Configuration> Imports { get; private set; }
        public Dictionary<string, FormatterDefinition> Formatters { get; private set; }

        public Configuration()
        {
            Imports = new List<Configuration>();
            Formatters = new Dictionary<string, FormatterDefinition>();
        }
    }
}
