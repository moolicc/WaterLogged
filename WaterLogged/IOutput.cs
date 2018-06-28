using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public interface IOutput
    {
        /// <summary>
        /// Gets or sets a value indicating if this <see cref="IOutput"/> implementation is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a filter that filters messages that will be output through this <see cref="IOutput"/> item.
        /// </summary>
        FilterManager FilterManager { get; set; }

        /// <summary>
        /// Gets the <see cref="Log"/> that owns this <see cref="IOutput"/> item.
        /// </summary>
        Log Log { get; set; }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey,TValue}"/> which holds arguments to pass to the log's formatter.
        /// </summary>
        Dictionary<string, string> FormatterArgs { get; }
    }
}
