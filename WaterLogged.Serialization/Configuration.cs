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
            LogDefinition log = Logs[name];
            TypeCreator creator = new TypeCreator(log.Type);

            foreach (var formatterProperty in log.Properties)
            {
                creator.MemberValues.Add(formatterProperty.Key, formatterProperty.Value);
            }

            var value = (Log)creator.Create();

            foreach (var logListenerName in log.ListenerNames)
            {
                if (Listeners.ContainsKey(logListenerName))
                {
                    value.AddListener(ResolveListener(logListenerName));
                    continue;
                }
                else
                {
                    bool added = false;
                    foreach (var configuration in Imports)
                    {
                        if (configuration.Listeners.ContainsKey(logListenerName))
                        {
                            value.AddListener(configuration.ResolveListener(logListenerName));
                            added = true;
                            break;
                        }
                    }
                    if (added)
                    {
                        continue;
                    }
                }
                throw new KeyNotFoundException("Listener '" + logListenerName + "' not found in configuration or imports.");
            }

            if (Formatters.ContainsKey(log.FormatterName))
            {
                value.Formatter = ResolveFormatter(log.FormatterName);
            }
            else
            {
                foreach (var configuration in Imports)
                {
                    if (configuration.Formatters.ContainsKey(log.FormatterName))
                    {
                        value.Formatter = configuration.ResolveFormatter(log.FormatterName);
                        break;
                    }
                }
            }

            return value;
        }
    }
}
