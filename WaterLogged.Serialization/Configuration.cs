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
            FormatterDefinition formatter = Formatters[name];
            TypeCreator creator = new TypeCreator(formatter.Type);

            foreach (var formatterProperty in formatter.Properties)
            {
                creator.MemberValues.Add(formatterProperty.Key, formatterProperty.Value);
            }

            return (Formatter)creator.Create();
        }

        public Listener ResolveListener(string name)
        {
            ListenerDefinition listener = Listeners[name];
            TypeCreator creator = new TypeCreator(listener.Type);

            foreach (var formatterProperty in listener.Properties)
            {
                creator.MemberValues.Add(formatterProperty.Key, formatterProperty.Value);
            }

            return (Listener)creator.Create();
        }

        public Log ResolveLog(string name)
        {
            return null;
        }
    }
}
