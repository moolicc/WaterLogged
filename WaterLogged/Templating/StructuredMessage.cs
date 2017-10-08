using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    public class StructuredMessage
    {
        public string TemplateSource { get; private set; }
        public Template ParsedTemplate { get; private set; }
        public Dictionary<int, PropertyValue> TemplateValues { get; private set; }
        public Dictionary<string, object> ContextValues { get; private set; }

        public StructuredMessage(string templateSource, Template template)
        {
            TemplateSource = templateSource;
            ParsedTemplate = template;
            TemplateValues = new Dictionary<int, PropertyValue>();
            ContextValues = new Dictionary<string, object>();
        }
    }
}
