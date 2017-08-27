using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public class Listener
    {
        public Log Log { get; internal set; }
        public string Name { get; internal set; }
        public bool Enabled { get; set; }
        public string[] TagFilter { get; set; }
        public Dictionary<string, string> FormatterArgs { get; private set; }

        public Listener()
        {
            Enabled = true;
            TagFilter = new string[0];
            FormatterArgs = new Dictionary<string, string>();
        }

        public void SetName(string newName)
        {
            if (Log != null)
            {
                Log.ChangeListenerName(Name, newName);
                Name = newName;
            }
        }

        public virtual void Write(string value, string tag)
        {
        }
    }
}
