using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged
{
    /// <summary>
    /// Base class for MessageSink implementations.
    /// </summary>
    public abstract class MessageSink : IMessageSink
    {
        /// <summary>
        /// Gets or sets a value indicating if this <see cref="MessageSink"/> implementation is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a filter that filters messages that will be output through this sink.
        /// </summary>
        public FilterManager FilterManager
        {
            get => _filterManager;
            set => _filterManager = value ?? throw new ArgumentNullException(nameof(FilterManager), "You cannot have a null filter.");
        }

        /// <summary>
        /// Message sinks don't support formatting. This will throw a <see cref="NotImplementedException"/>.
        /// </summary>
        public Dictionary<string, string> FormatterArgs => throw new NotImplementedException("Message sinks don't support formatting.");

        /// <summary>
        /// Gets the <see cref="Log"/> that owns this MessageSink.
        /// </summary>
        public Log Log { get; set; }


        private FilterManager _filterManager;

        protected MessageSink()
        {
            Enabled = true;
            FilterManager = new FilterManager();
        }

        /// <summary>
        /// When overridden in a derived class; Handles an output message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="tag">The message's tag.</param>
        public abstract void ProcessMessage(StructuredMessage message, string tag);
    }
}
