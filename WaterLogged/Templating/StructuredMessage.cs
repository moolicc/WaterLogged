using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    public class StructuredMessage
    {
        public string EntryName { get; set; }
        public string Template { get; set; }
        public Dictionary<Hole, HoleValue> Values { get; private set; }
        public Dictionary<string, object> ContextValues { get; private set; }
        public DateTime CreationDate { get; private set; }

        public StructuredMessage()
            : this("", "entry")
        {
        }

        public StructuredMessage(string template)
            : this(template, "entry")
        {
        }

        public StructuredMessage(string template, string entryName)
        {
            EntryName = entryName;
            Template = template;
            Values = new Dictionary<Hole, HoleValue>();
            ContextValues = new Dictionary<string, object>();
            CreationDate = DateTime.Now;
        }

        public StructuredMessage Write(Log log)
        {
            return Write(log, "");
        }

        public StructuredMessage Write(Log log, string tag)
        {
            log.WriteStructuredMessage(this, tag);
            return this;
        }

        public StructuredMessage WithEntryName(string entryName)
        {
            EntryName = entryName;
            return this;
        }

        public StructuredMessage WithTemplate(string template)
        {
            Template = template;
            return this;
        }

        public StructuredMessage WithContext(string name, object value)
        {
            ContextValues.Add(name, value);
            return this;
        }

        public StructuredMessage WithContext(params (string name, object value)[] values)
        {
            foreach (var value in values)
            {
                ContextValues.Add(value.name, value.value);
            }
            return this;
        }

        public StructuredMessage WithContext(Dictionary<string, object> values)
        {
            foreach (var value in values)
            {
                ContextValues.Add(value.Key, value.Value);
            }
            return this;
        }
    }
}
