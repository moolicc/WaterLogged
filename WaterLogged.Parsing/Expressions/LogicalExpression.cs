using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WaterLogged.Parsing.Expressions
{
    public class LogicalExpression : IExpression
    {
        public IExpression NestedExpression { get; set; }

        public LogicalExpression(IExpression expression)
        {
            NestedExpression = expression;
        }

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
