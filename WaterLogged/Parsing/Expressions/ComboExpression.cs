namespace WaterLogged.Parsing.Expressions
{
    /// <summary>
    /// Implements the combination of two expressions. Evaluates to the form 'a + b'.
    /// </summary>
    public class ComboExpression : IExpression
    {
        /// <summary>
        /// The first expression to combine.
        /// </summary>
        public IExpression Left { get; private set; }
        /// <summary>
        /// The second expression to combine.
        /// </summary>
        public IExpression Right { get; private set; }

        public ComboExpression(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        /// <inheritdoc />
        public string Eval(Evaluator evaluator)
        {
            return Left.Eval(evaluator) + Right.Eval(evaluator);
        }
    }
}
