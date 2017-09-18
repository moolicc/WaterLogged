using System;
using System.Reflection;

namespace WaterLogged.Parsing.Expressions
{
    /// <summary>
    /// Implements an expression to evaluate mathematical and boolean expressions using NCalc in the form '#{1 + 1}'.
    /// </summary>
    public class LogicalExpression : IExpression
    {
        /// <summary>
        /// The underlying expression to evaluate.
        /// </summary>
        public IExpression NestedExpression { get; set; }

        public LogicalExpression(IExpression expression)
        {
            NestedExpression = expression;
        }

        /// <inheritdoc />
        public string Eval(Evaluator evaluator)
        {
            var expression = new NCalc.Expression(NestedExpression.Eval(evaluator));
            if (expression.HasErrors())
            {
                return "[ERROR]";
            }
            expression.EvaluateParameter += (name, args) =>
            {
                Delegate func = null;
                try
                {
                    func = evaluator.Context.GetDelegate(name);
                }
                catch
                {
                    throw new InvalidOperationException("Invalid logical operation.");
                }

                if (func.GetMethodInfo().GetParameters().Length == 0)
                {
                    args.HasResult = true;
                    args.Result = func.DynamicInvoke();
                }
                else
                {
                    throw new InvalidOperationException("Invalid logical operation.");
                }
            };
            return expression.Evaluate().ToString();
        }
    }
}
