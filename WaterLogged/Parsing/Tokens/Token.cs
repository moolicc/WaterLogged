namespace WaterLogged.Parsing.Tokens
{
    /// <summary>
    /// Represents a token in a format string.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The index in the format string of this token.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// The actual raw-text of this token.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Initializes a token with the specified index and the specified text.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="text">The text.</param>
        public Token Init(int index, string text)
        {
            Index = index;
            Text = text;
            return this;
        }
    }
}
