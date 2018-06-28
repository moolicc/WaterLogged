using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Output
{
    /// <summary>
    /// Represents both a <see cref="IListener"/> and <see cref="IMessageSink"/>.
    /// </summary>
    public abstract class ListenerSink : IListener, IMessageSink
    {
        /// <summary>
        /// Gets or sets a value indicating if this Listener/Sink is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a filter that filters messages that will be output through this Listener/Sink.
        /// </summary>
        public FilterManager FilterManager
        {
            get => _filterManager;
            set => _filterManager = value ?? throw new ArgumentNullException(nameof(FilterManager), "You cannot have a null filter.");
        }

        /// <summary>
        /// Gets the <see cref="Log"/> that owns this Listener/Sink.
        /// </summary>
        public Log Log { get; set; }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> which holds arguments to pass to the log's formatter.
        /// </summary>
        public Dictionary<string, string> FormatterArgs { get; private set; }
        
        private FilterManager _filterManager;

        protected ListenerSink()
        {
            Enabled = true;
            FilterManager = new FilterManager();
            FormatterArgs = new Dictionary<string, string>();
        }

        public virtual void ProcessMessage(StructuredMessage message, string tag)
        {
        }

        public virtual void Write(string value, string tag)
        {
        }
    }
}
