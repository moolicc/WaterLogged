using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    public class StructuredMessage
    {
        public string Template { get; set; }
        public Dictionary<Hole, HoleValue> Values { get; private set; }
        public Dictionary<string, object> UnusedValues { get; private set; }
        public DateTime CreationDate { get; private set; }

        public StructuredMessage()
            : this("")
        {
        }

        public StructuredMessage(string template)
        {
            Template = template;
            Values = new Dictionary<Hole, HoleValue>();
            UnusedValues = new Dictionary<string, object>();
            CreationDate = DateTime.Now;
        }
    }
}
