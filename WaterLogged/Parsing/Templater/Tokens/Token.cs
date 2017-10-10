using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Parsing.Templater.Tokens
{
    /// <summary>
    /// Represents a token in a template string.
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        /// Gets the position in the template string this token starts at.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Sets the index of the position in the template string this token starts at.
        /// </summary>
        /// <param name="index">The index.</param>
        public Token SetIndex(int index)
        {
            Index = index;
            return this;
        }

        /// <summary>
        /// When overridden in a derived class; Transforms this token back into its string equivalent and returns the resulting string.
        /// </summary>
        public abstract string BuildString();
    }
}
