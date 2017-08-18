using System;
using System.Collections.Generic;
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
                    throw new ConfigReadException("Formatter is missing a necessary attribute in the configuration. Make sure 'name' and 'type' exist as attributes.");
                }
                Configuration.Formatters.Add(definition.Id, definition);
            }
        }
    }
}
