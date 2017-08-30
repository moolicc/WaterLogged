using System.Collections.Generic;

namespace WaterLogged.Serialization
{
    public class LogDefinition : Definition
    {
        public List<string> ListenerNames { get; private set; }
        public string FormatterName { get; set; }

        public LogDefinition()
            : base(DefinitionTypes.LogDefinition)
        {
            ListenerNames = new List<string>();
        }
    }
}
