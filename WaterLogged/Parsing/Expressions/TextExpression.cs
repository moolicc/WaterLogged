namespace WaterLogged.Parsing.Expressions
{
    /// <summary>
    /// Implements an expression that contains plain text.
    /// </summary>
    public class TextExpression : IExpression
    {
        /// <summary>
        /// The text data.
        /// </summary>
        public string Text { get; set; }

        public TextExpression(string text)
        {
            Text = text;
        }
        
        /// <inheritdoc />
        public string Eval(Evaluator evaluator)
        {
            return Text;
        }
    }
}
