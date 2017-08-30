using System;
using System.Text;
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
            ParseLogs();

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
                var definition = ParseFormatter(formatterElement);
                Configuration.Formatters.Add(definition.Id, definition);
            }
        }

        private void ParseListeners()
        {
            var listenerElements = _rootElement.Elements("listener");
            foreach (var listenerElement in listenerElements)
            {
                var definition = ParseListener(listenerElement);
                Configuration.Listeners.Add(definition.Id, definition);
            }
        }
        
        private void ParseLogs()
        {
            var logElements = _rootElement.Elements("logs");
            foreach (var logElement in logElements)
            {
                var definition = ParseLog(logElement);
                Configuration.Logs.Add(definition.Id, definition);
            }
        }


        private ListenerDefinition ParseListener(XElement element)
        {
            return ParseDefinition(new ListenerDefinition(), element);
        }

        private FormatterDefinition ParseFormatter(XElement element)
        {
            return ParseDefinition(new FormatterDefinition(), element);
        }

        private LogDefinition ParseLog(XElement element)
        {
            var definition = new LogDefinition();
            foreach (var attribute in element.Attributes())
            {
                if (attribute.Name == "name")
                {
                    definition.Id = attribute.Value;
                }
                else
                {
                    definition.Properties.Add(attribute.Name.ToString(), attribute.Value);
                }
            }
            foreach (var subElement in element.Elements())
            {
                if (subElement.Name.ToString().Equals("listener", StringComparison.OrdinalIgnoreCase))
                {
                    var refInfo = subElement.GetAttribute("ref");
                    string name = GetTempListenerName();

                    if (refInfo.Item1)
                    {
                        name = refInfo.Item2.Value;
                    }
                    else
                    {
                        var listenerDefinition = ParseListener(subElement);
                        listenerDefinition.Id = name;
                        Configuration.Listeners.Add(name, listenerDefinition);
                    }

                    definition.ListenerNames.Add(name);
                    continue;
                }
                if (subElement.Name.ToString().Equals("formatter", StringComparison.OrdinalIgnoreCase))
                {
                    var refInfo = subElement.GetAttribute("ref");
                    string name = GetTempFormatterName();

                    if (refInfo.Item1)
                    {
                        name = refInfo.Item2.Value;
                    }
                    else
                    {
                        var formatterDefinition = ParseFormatter(subElement);
                        formatterDefinition.Id = name;
                        Configuration.Formatters.Add(name, formatterDefinition);
                    }

                    definition.FormatterName = name;
                    continue;
                }
                var paramArray = GetParameterArray(subElement);
                definition.Properties.Add(paramArray.Item1, paramArray.Item2);
            }
            return definition;
        }

        private T ParseDefinition<T>(T definitionBase, XElement element) where T : Definition
        {
            foreach (var attribute in element.Attributes())
            {
                if (attribute.Name == "type")
                {
                    definitionBase.Type = attribute.Value;
                }
                else if (attribute.Name == "name")
                {
                    definitionBase.Id = attribute.Value;
                }
                else
                {
                    definitionBase.Properties.Add(attribute.Name.ToString(), attribute.Value);
                }
            }
            if (string.IsNullOrWhiteSpace(definitionBase.Type) || string.IsNullOrWhiteSpace(definitionBase.Id))
            {
                throw new ConfigReadException(
                    definitionBase.DefinitionType.ToString() + " is missing a necessary attribute in the configuration. Make sure 'name' and 'type' exist as attributes.");
            }
            foreach (var subElement in element.Elements())
            {
                var paramArray = GetParameterArray(subElement);
                definitionBase.Properties.Add(paramArray.Item1, paramArray.Item2);
            }
            return definitionBase;
        }


        private string GetTempListenerName()
        {
            return string.Format("listener{0}", DateTime.Now.Ticks);
        }

        private string GetTempFormatterName()
        {
            return string.Format("formatter{0}", DateTime.Now.Ticks);
        }
        

        private (string, string) GetParameterArray(XElement parent)
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