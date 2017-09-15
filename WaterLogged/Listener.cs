using System.Collections.Generic;
using WaterLogged.Templating;

namespace WaterLogged
{
    public abstract class Listener
    {
        public bool Enabled { get; set; }
        public string[] TagFilter { get; set; }
        public Log Log { get; internal set; }
        public string Name { get; internal set; }
        public Dictionary<string, string> FormatterArgs { get; private set; }

        protected Listener()
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
            Name = newName;
        }

        public abstract void Write(string value, string tag);
    }
}
