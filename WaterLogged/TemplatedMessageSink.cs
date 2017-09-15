using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged
{
    public abstract class TemplatedMessageSink
    {
        public bool Enabled { get; set; }
        public string[] TagFilter { get; set; }
        public string Name { get; internal set; }
        public Log Log { get; internal set; }

        protected TemplatedMessageSink()
        {
            Enabled = true;
            TagFilter = new string[0];
        }

        public void SetName(string newName)
        {
            if (Log != null)
            {
                Log.ChangeSinkName(Name, newName);
            }
            Name = newName;
        }

        public abstract void ProcessMessage(Log log, StructuredMessage message, string tag);
    }
}
