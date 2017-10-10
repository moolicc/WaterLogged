using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Parsing.Templater.Tokens
{
    /// <summary>
    /// Represents text.
    /// </summary>
    /// <inheritdoc />
    public class LiteralToken : Token
    {
        /// <summary>
        /// Gets the text of the literal this token represents.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Sets the text of this literal token.
        /// </summary>
        /// <param name="text">The text to set.</param>
        public LiteralToken SetText(string text)
        {
            Text = text;
            return this;
        }

        public override string BuildString()
        {
            return Text;
        }
    }
}
