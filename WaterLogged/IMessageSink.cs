using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged
{
    public interface IMessageSink
    {
        /// <summary>
        /// Gets or sets a value indicating if this <see cref="IMessageSink"/> implementation is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a filter that filters messages that will be output through this sink.
        /// </summary>
        FilterManager FilterManager { get; set; }

        /// <summary>
        /// Gets the <see cref="Log"/> that owns this MessageSink.
        /// </summary>
        Log Log { get; set; }

        /// <summary>
        /// When overridden in a derived class; Handles an output message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="tag">The message's tag.</param>
        void ProcessMessage(StructuredMessage message, string tag);
    }
}
