using System.Collections.Generic;
using WaterLogged.Templating;

namespace WaterLogged
{
    /// <summary>
    /// Base class for Listener implementations.
    /// </summary>
    public abstract class Listener
    {
        /// <summary>
        /// Gets or sets a value indicating if this <see cref="Listener"/> implementation is enabled.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets an array of tags to whitelist.
        /// </summary>
        public string[] TagFilter { get; set; }
        /// <summary>
        /// Gets the <see cref="Log"/> that owns this Listener.
        /// </summary>
        public Log Log { get; internal set; }
        /// <summary>
        /// Gets the name of this Listener.
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> which holds arguments to pass to the log's formatter.
        /// </summary>
        public Dictionary<string, string> FormatterArgs { get; private set; }
        
        protected Listener()
        {
            Enabled = true;
            TagFilter = new string[0];
            FormatterArgs = new Dictionary<string, string>();
        }

        /// <summary>
        /// Sets the name of this Listener.
        /// </summary>
        /// <param name="newName">The new name to give this Listener.</param>
        public void SetName(string newName)
        {
            if (Log != null)
            {
                Log.ChangeListenerName(Name, newName);
                Name = newName;
            }
            Name = newName;
        }

        /// <summary>
        /// When overridden in a derived class; Handles an output message.
        /// </summary>
        /// <param name="value">The message.</param>
        /// <param name="tag">The message's tag.</param>
        public abstract void Write(string value, string tag);
    }
}
