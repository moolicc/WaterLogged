using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WaterLogged.Serialization.Json
{
    public class JsonConfigReader : Reader
    {
        private Configuration _configuration;
        private JObject _root;

        public override Configuration Read(string data)
        {
            _root = (JObject)JToken.Parse(data);
            _configuration = new Configuration();

            ParseImports();
            ParseFormatters();
            ParseListeners();
            ParseSinks();
            ParseLogs();

            return _configuration;
        }

        private void ParseImports()
        {
            if (!(_root["imports"] is JArray importNodes))
            {
                return;
            }

            foreach (var importNode in importNodes)
            {
                string importPath = importNode["path"].Value<string>();
                _configuration.Imports.Add(Reader.ReadFile(importPath));
            }
        }

        private void ParseFormatters()
        {
            if (!(_root["formatters"] is JObject formatterNodes))
            {
                return;
            }
            foreach (var formatterNode in formatterNodes)
            {
                var definition = new FormatterDefinition();
                definition = ParseDefinition(definition, formatterNode.Value);
                _configuration.Formatters.Add(definition.Id, definition);
            }
        }

        private void ParseListeners()
        {
            if (!(_root["listeners"] is JObject listenerNodes))
            {
                return;
            }
            foreach (var listenerNode in listenerNodes)
            {
                var definition = new ListenerDefinition();
                definition = ParseDefinition(definition, listenerNode.Value);
                _configuration.Listeners.Add(definition.Id, definition);
            }
        }

        private void ParseSinks()
        {
            if (!(_root["sinks"] is JObject sinkNodes))
            {
                return;
            }
            foreach (var sinkNode in sinkNodes)
            {
                var definition = new SinkDefinition();
                definition = ParseDefinition(definition, sinkNode.Value);
                _configuration.Sinks.Add(definition.Id, definition);
            }
        }

        private void ParseLogs()
        {
            if (!(_root["logs"] is JObject logNodes))
            {
                return;
            }

            foreach (var logNode in logNodes)
            {
                var definition = ParseLogDefinition((JObject)logNode.Value);
                _configuration.Logs.Add(definition.Id, definition);
            }
        }

        private LogDefinition ParseLogDefinition(JObject logNode)
        {
            var definition = new LogDefinition();
            definition = ParseDefinition(definition, logNode, "", "formatter", "listeners", "sinks");

            var formatterNode = logNode["formatter"];
            if (formatterNode != null)
            {
                if (formatterNode.Type == JTokenType.String)
                {
                    definition.FormatterName = formatterNode.Value<string>();
                }
                else
                {
                    var formatterDefinition = new FormatterDefinition();
                    string name = definition.Id + "_formatter";
                    formatterDefinition = ParseDefinition(formatterDefinition, formatterNode, name);

                    _configuration.Formatters.Add(name, formatterDefinition);
                    definition.FormatterName = name;
                }
            }

            var listenerNodes = logNode["listeners"];
            if (listenerNodes != null)
            {
                foreach (var listenerNode in listenerNodes)
                {
                    if (listenerNode.Type == JTokenType.String)
                    {
                        definition.ListenerNames.Add(formatterNode.Value<string>());
                    }
                    else
                    {
                        var name = definition.Id + "_listener" + DateTime.Now.Ticks;
                        var listenerDefinition = new ListenerDefinition();
                        listenerDefinition = ParseDefinition(listenerDefinition, listenerNode, name);

                        _configuration.Listeners.Add(name, listenerDefinition);
                        definition.ListenerNames.Add(name);
                    }
                }
            }

            var sinkNodes = logNode["sinks"];
            if (sinkNodes != null)
            {
                foreach (var sinkNode in sinkNodes)
                {
                    if (sinkNode.Type == JTokenType.String)
                    {
                        
                    }
                    else
                    {
                        var name = definition.Id + "_sink" + DateTime.Now.Ticks;
                        var sinkDefinition = new SinkDefinition();
                        sinkDefinition = ParseDefinition(sinkDefinition, sinkNode, name);

                        _configuration.Sinks.Add(name, sinkDefinition);
                        definition.SinkNames.Add(name);
                    }
                }
            }

            return definition;
        }

        private T ParseDefinition<T>(T template, JToken item, string forcedName = "", params string[] exclusions) where T : Definition
        {
            if (!item.HasValues)
            {
                throw new InvalidOperationException("Invalid json configuration.");
            }

            string type = "";
            if (item.Type == JTokenType.Property)
            {
                var propertyItem = (JProperty) item;
                var typeItem = propertyItem.Value;
                type = typeItem.Value<string>("type");
            }
            else
            {
                type = item["type"].Value<string>();
            }
            template.Type = type;



            if (!string.IsNullOrWhiteSpace(forcedName))
            {
                template.Id = forcedName;
            }
            else
            {
                var nameItem = item["name"];
                template.Id = nameItem.Value<string>();
            }

            var excludedProperties = new List<string> {"name", "type"};
            excludedProperties.AddRange(exclusions);
            foreach (var property in ReadProperties(item, excludedProperties.ToArray()))
            {
                template.Properties.Add(property.Key, property.Value);
            }
            return template;
        }

        private Dictionary<string, string> ReadProperties(JToken parent, params string[] exlusions)
        {
            var properties = new Dictionary<string, string>();

            if (!parent.HasValues)
            {
                return properties;
            }

            foreach (var child in parent.Children<JProperty>())
            {
                if (exlusions.Contains(child.Name))
                {
                    continue;
                }
                if (child.Children().Count() > 1)
                {
                    var builder = new StringBuilder();
                    var childProperties = ReadProperties(child);
                    foreach (var childProperty in childProperties)
                    {
                        builder.Append(childProperty.Value + "|");
                    }
                    properties.Add(child.Name, builder.ToString().TrimEnd('|'));
                    continue;
                }
                if (child.Type == JTokenType.Property)
                {
                    properties.Add(child.Name, child.Value.Value<string>());
                }
                else
                {
                    properties.Add(child.Name, child.Value<string>());
                }
            }

            return properties;
        }
    }
}
