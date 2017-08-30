using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WaterLogged.Serialization.Xml
{
    public class XmlReader : Reader
    {
        public Configuration Configuration { get; private set; }

        private XDocument _document;
        private XElement _rootElement;

        public override Configuration Read(string data)
        {
            Configuration = new Configuration();

            _document = XDocument.Parse(data);
            _rootElement = _document.Root;

            ParseImports();
            ParseFormatters();
            ParseListeners();

            return Configuration;
        }

        private void ParseImports()
        {
            var importElements = _rootElement.Elements("import");
            foreach (var importElement in importElements)
            {
                string importPath = importElement.GetValueOrAttribute("path");
                Configuration.Imports.Add(Reader.ReadFile(importPath));
            }
        }

        private void ParseFormatters()
        {
            var formatterElements = _rootElement.Elements("formatter");
            foreach (var formatterElement in formatterElements)
            {
                var definition = new FormatterDefinition();
                foreach (var attribute in formatterElement.Attributes())
                {
                    if (attribute.Name == "type")
                    {
                        definition.Type = attribute.Value;
                    }
                    else if (attribute.Name == "name")
                    {
                        definition.Id = attribute.Value;
                    }
                    else
                    {
                        definition.Properties.Add(attribute.Name.ToString(), attribute.Value);
                    }
                }
                if (string.IsNullOrWhiteSpace(definition.Type) || string.IsNullOrWhiteSpace(definition.Id))
                {
                    throw new ConfigReadException(
                        "Formatter is missing a necessary attribute in the configuration. Make sure 'name' and 'type' exist as attributes.");
                }
                foreach (var subElement in formatterElement.Elements())
                {
                    var paramArray = GetParameterArray(subElement);
                    definition.Properties.Add(paramArray.Item1, paramArray.Item2);
                }
                Configuration.Formatters.Add(definition.Id, definition);
            }
        }

        private void ParseListeners()
        {
            var listenerElements = _rootElement.Elements("listener");
            foreach (var listenerElement in listenerElements)
            {
                var definition = new ListenerDefinition();
                foreach (var attribute in listenerElement.Attributes())
                {
                    if (attribute.Name == "type")
                    {
                        definition.Type = attribute.Value;
                    }
                    else if (attribute.Name == "name")
                    {
                        definition.Id = attribute.Value;
                    }
                    else
                    {
                        definition.Properties.Add(attribute.Name.ToString(), attribute.Value);
                    }
                }
                if (string.IsNullOrWhiteSpace(definition.Type) || string.IsNullOrWhiteSpace(definition.Id))
                {
                    throw new ConfigReadException(
                        "Listener is missing a necessary attribute in the configuration. Make sure 'name' and 'type' exist as attributes.");
                }
                foreach (var subElement in listenerElement.Elements())
                {
                    var paramArray = GetParameterArray(subElement);
                    definition.Properties.Add(paramArray.Item1, paramArray.Item2);
                }
                Configuration.Listeners.Add(definition.Id, definition);
            }
        }

        private void ParseLogs()
        {
            var logElements = _rootElement.Elements("logs");
            foreach (var logElement in logElements)
            {
                var definition = new LogDefinition();
                foreach (var attribute in logElement.Attributes())
                {
                    if (attribute.Name == "name")
                    {
                        definition.Id = attribute.Value;
                    }
                    else if (attribute.Name == "formatter")
                    {
                        definition.FormatterName = attribute.Value;
                    }
                    else if (attribute.Name == "listeners")
                    {
                        definition.ListenerNames.AddRange(attribute.Value.Split('|'));
                    }
                    else
                    {
                        definition.Properties.Add(attribute.Name.ToString(), attribute.Value);
                    }
                }
                foreach (var subElement in logElement.Elements())
                {
                    var paramArray = GetParameterArray(subElement);
                    definition.Properties.Add(paramArray.Item1, paramArray.Item2);
                }
                Configuration.Logs.Add(definition.Id, definition);
            }
        }

        public (string, string) GetParameterArray(XElement parent)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var itemElement in parent.Elements())
            {
                builder.Append(itemElement.GetValueOrAttribute("value"));
                builder.Append("|");
            }
            return (parent.Name.ToString(), builder.ToString().TrimEnd('|'));
        }
    }
}