using System.Collections.Generic;

namespace WaterLogged
{
    /// <summary>
    /// Base class for Log message formatters.
    /// </summary>
    public abstract class Formatter
    {
        /// <summary>
        /// When overridden in a derived class; returns a value indicating if this <see cref="Formatter"/> implementation supports templated messages.
        /// </summary>
        public abstract bool SupportsTemplating { get; }

        /// <summary>
        /// When overridden in a derived class; transforms a template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="log">The log.</param>
        /// <param name="tag">The message's tag.</param>
        /// <param name="overrides">Optional arguments.</param>
        public virtual string Transform(string template, Log log, string tag, Dictionary<string, string> overrides)
        {
            return Transform(log, template, tag, overrides);
        }

        /// <summary>
        /// When overridden in a derived class; transforms a message.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="input">The message.</param>
        /// <param name="tag">The message's tag.</param>
        /// <param name="overrides">Optional arguments.</param>
        public abstract string Transform(Log log, string input, string tag, Dictionary<string, string> overrides);
    }
}
