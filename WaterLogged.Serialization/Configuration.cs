using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Serialization
{
    public class Configuration
    {
        public string FilePath { get; set; }
        public List<Configuration> Imports { get; private set; }
        public Dictionary<string, FormatterDefinition> Formatters { get; private set; }
        public Dictionary<string, ListenerDefinition> Listeners { get; private set; }
        public Dictionary<string, LogDefinition> Logs { get; private set; }

        public Configuration()
        {
            Imports = new List<Configuration>();
            Formatters = new Dictionary<string, FormatterDefinition>();
            Listeners = new Dictionary<string, ListenerDefinition>();
            Logs = new Dictionary<string, LogDefinition>();
        }
        
        public Formatter ResolveFormatter(string name)
        {
            return null;
        }

        public Listener ResolveListener(string name)
        {
            return null;
        }

        public Log ResolveLog(string name)
        {
            return null;
        }
    }
}
