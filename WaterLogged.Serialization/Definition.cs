using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Serialization
{
    public enum DefinitionTypes
    {
        LogDefinition,
        ListenerDefinition,
        FormatterDefinition,
    }

    public abstract class Definition
    {
        public DefinitionTypes DefinitionType { get; private set; }
        public Dictionary<string, string> Properties { get; private set; }
        public string Id { get; set; }
        public string Type { get; set; }

        protected Definition(DefinitionTypes definitionType)
        {
            DefinitionType = definitionType;
            Properties = new Dictionary<string, string>();
        }
    }
}
