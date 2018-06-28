using System;
using System.Collections.Generic;
using System.Reflection;

namespace WaterLogged.Serialization
{
    public class Configuration
    {
        public string FilePath { get; set; }
        public List<Configuration> Imports { get; private set; }
        public Dictionary<string, FormatterDefinition> Formatters { get; private set; }
        public Dictionary<string, ListenerDefinition> Listeners { get; private set; }
        public Dictionary<string, SinkDefinition> Sinks { get; private set; }
        public Dictionary<string, LogDefinition> Logs { get; private set; }

        public static Configuration FromLogs(Log[] logs)
        {
            var config = new Configuration();
            foreach (var log in logs)
            {
                var logDefinition = GetLogDefinition(log);

                foreach (var logListener in log.Listeners)
                {
                    var listenerDefinition = GetListenerDefinition(logListener);
                    logDefinition.ListenerNames.Add(listenerDefinition.Id);
                    config.Listeners.Add(listenerDefinition.Id, listenerDefinition);
                }
                foreach (var logSink in log.Sinks)
                {
                    var sinkDefinition = GetSinkDefinition(logSink);
                    logDefinition.SinkNames.Add(sinkDefinition.Id);
                    config.Sinks.Add(sinkDefinition.Id, sinkDefinition);
                }
                var formatterDefinition = GetFormatterDefinition(log.Formatter);
                config.Formatters.Add(formatterDefinition.Id, formatterDefinition);
                logDefinition.FormatterName = formatterDefinition.Id;

                config.Logs.Add(logDefinition.Id, logDefinition);
            }
            return config;
        }

        private static LogDefinition GetLogDefinition(Log log)
        {
            var definition = new LogDefinition();
            definition.Id = log.Name;
            definition.Type = log.GetType().AssemblyQualifiedName;
            LoadPropertyValues(definition, log);
            return definition;
        }
        
        private static FormatterDefinition GetFormatterDefinition(Formatter formatter)
        {
            var definition = new FormatterDefinition();
            definition.Id = string.Format("formatter{0}", DateTime.Now.Ticks);
            definition.Type = formatter.GetType().AssemblyQualifiedName;
            LoadPropertyValues(definition, formatter);
            return definition;
        }

        private static ListenerDefinition GetListenerDefinition(Listener listener)
        {
            var definition = new ListenerDefinition();
            definition.Id = string.Format("listener{0}", DateTime.Now.Ticks);
            definition.Type = listener.GetType().AssemblyQualifiedName;
            LoadPropertyValues(definition, listener);
            return definition;
        }

        private static SinkDefinition GetSinkDefinition(MessageSink sink)
        {
            var definition = new SinkDefinition();
            definition.Id = string.Format("sink{0}", DateTime.Now.Ticks);
            definition.Type = sink.GetType().AssemblyQualifiedName;
            LoadPropertyValues(definition, sink);
            return definition;
        }

        private static void LoadPropertyValues(Definition definition, object value)
        {
            Type type = value.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }
                definition.Properties.Add(property.Name, StringConversion.Converter.Convert(property.GetValue(value)));
            }
            foreach (var field in fields)
            {
                definition.Properties.Add(field.Name, StringConversion.Converter.Convert(field.GetValue(value)));
            }
        }

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

        public MessageSink ResolveSink(string name)
        {
            SinkDefinition sink = Sinks[name];
            TypeCreator creator = new TypeCreator(sink.Type);

            foreach (var formatterProperty in sink.Properties)
            {
                creator.MemberValues.Add(formatterProperty.Key, formatterProperty.Value);
            }

            return (MessageSink)creator.Create();
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
