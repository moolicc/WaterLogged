using System;
using System.Collections.Generic;

namespace WaterLogged.Parsing
{
    /// <summary>
    /// Represents a function resolver for the formatter evaluator.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// The available functions.
        /// </summary>
        public Dictionary<string, Delegate> Functions { get; private set; }

        /// <summary>
        /// Instantiates a Context.
        /// </summary>
        public Context()
        {
            Functions = new Dictionary<string, Delegate>();
        }

        /// <summary>
        /// Returns the function with the specified name.
        /// </summary>
        /// <param name="name">The name of the function to return.</param>
        public virtual Delegate GetDelegate(string name)
        {
            if (!Functions.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Context doesn't contain the function '{name}'.");
            }
            return Functions[name];
        }
    }
}
