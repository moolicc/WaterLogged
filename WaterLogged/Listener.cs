using System;
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
        /// Gets or sets a filter that filters messages that will be output through this listener.
        /// </summary>
        public FilterManager FilterManager
        {
            get => _filterManager;
            set => _filterManager = value ?? throw new ArgumentNullException(nameof(FilterManager), "You cannot have a null filter.");
        }

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
        
        private FilterManager _filterManager;

        protected Listener()
        {
            Enabled = true;
            FilterManager = new FilterManager();
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
