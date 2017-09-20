using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged
{
    /// <summary>
    /// Base class for TemplatedMessageSink implementations.
    /// </summary>
    public abstract class TemplatedMessageSink
    {
        /// <summary>
        /// Gets or sets a value indicating if this <see cref="TemplatedMessageSink"/> implementation is enabled.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets an array of tags to whitelist.
        /// </summary>
        public string[] TagFilter { get; set; }
        /// <summary>
        /// Gets the name of this TemplatedMessageSink.
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// Gets the <see cref="Log"/> that owns this TemplatedMessageSink.
        /// </summary>
        public Log Log { get; internal set; }

        protected TemplatedMessageSink()
        {
            Enabled = true;
            TagFilter = new string[0];
        }

        /// <summary>
        /// Sets the name of this TemplatedMessageSink.
        /// </summary>
        /// <param name="newName">The new name to give this TemplatedMessageSink.</param>
        public void SetName(string newName)
        {
            if (Log != null)
            {
                Log.ChangeSinkName(Name, newName);
            }
            Name = newName;
        }

        /// <summary>
        /// When overridden in a derived class; Handles an output message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="tag">The message's tag.</param>
        public abstract void ProcessMessage(StructuredMessage message, string tag);
    }
}
