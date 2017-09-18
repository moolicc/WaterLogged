using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Parsing;

namespace WaterLogged.Formatting
{
    /// <summary>
    /// Immutable context used when formatting a log message.
    /// </summary>
    public class MessageContext : Context, IDisposable
    {
        /// <summary>
        /// The base context to resolve functions from. If a function can't be
        /// resolved from this MessageContext alone, the specified BaseContext will be queried.
        /// </summary>
        public Context BaseContext { get; private set; }
        /// <summary>
        /// The acting log of the message that is being formatted.
        /// </summary>
        public Log Log { get; private set; }
        /// <summary>
        /// The tag of the message that is being formatted.
        /// </summary>
        public string Tag { get; private set; }
        /// <summary>
        /// The message that is being formatted.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Instantiates a new MessageContext.
        /// </summary>
        /// <param name="baseContext">The base context.</param>
        /// <param name="log">The acting log.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="message">The message.</param>
        public MessageContext(Context baseContext, Log log, string tag, string message)
        {
            BaseContext = baseContext;
            Log = log;
            Tag = tag;
            Message = message;

            Functions.Add("log", new Func<string>(() => log.Name));
            Functions.Add("tag", new Func<string>(() => tag));
            Functions.Add("message", new Func<string>(() => message));
            Functions.Add("datetime", new Func<string>(() => DateTime.Now.ToString()));
        }

        /// <summary>
        /// Returns the function with the specified name as a Delegate.
        /// </summary>
        /// <param name="name">The name of the function to retrieve.</param>
        public override Delegate GetDelegate(string name)
        {
            if (Functions.ContainsKey(name))
            {
                return Functions[name];
            }
            if (BaseContext.Functions.ContainsKey(name))
            {
                return BaseContext.Functions[name];
            }
            return base.GetDelegate(name);
        }

        /// <summary>
        /// Clears all lists and sets eveything to null to ensure cleanup.
        /// </summary>
        public void Dispose()
        {
            BaseContext = null;
            Log = null;
            Tag = null;
            Message = null;
            Functions.Clear();
        }
    }
}