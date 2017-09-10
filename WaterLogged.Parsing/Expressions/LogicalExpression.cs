using System;
using System.Collections.Generic;
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
            return new NCalc.Expression(NestedExpression.Eval(evaluator)).Evaluate().ToString();
        }
    }
}
