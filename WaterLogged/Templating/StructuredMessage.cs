using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    /// <summary>
    /// Represents a structures log message.
    /// </summary>
    public struct StructuredMessage
    {
        /// <summary>
        /// Gets the original source of the template.
        /// </summary>
        public string TemplateSource { get; private set; }
        /// <summary>
        /// Gets the template parsed into a series of tokens.
        /// </summary>
        public Template ParsedTemplate { get; private set; }
        /// <summary>
        /// Gets the values used by the properties in the template indexed by the token indices within the Template.
        /// </summary>
        public Dictionary<int, PropertyValue> TemplateValues { get; private set; }
        /// <summary>
        /// Gets the context values not directly included in the message but still relevant.
        /// </summary>
        public Dictionary<string, object> ContextValues { get; private set; }

        /// <summary>
        /// Instantiates a new StructuredMessage using the specified template.
        /// </summary>
        /// <param name="templateSource"></param>
        /// <param name="template"></param>
        public StructuredMessage(string templateSource, Template template)
        {
            TemplateSource = templateSource;
            ParsedTemplate = template;
            TemplateValues = new Dictionary<int, PropertyValue>();
            ContextValues = new Dictionary<string, object>();
        }
    }
}
