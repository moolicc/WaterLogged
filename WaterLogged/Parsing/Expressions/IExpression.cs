namespace WaterLogged.Parsing.Expressions
{
    /// <summary>
    /// Represents an expression parsed from the Formatter evaluator.
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// When overridden in a derived class; evaluates the expression and returns the result.
        /// </summary>
        /// <param name="evaluator">The evaluator in use.</param>
        string Eval(Evaluator evaluator);
    }
}
