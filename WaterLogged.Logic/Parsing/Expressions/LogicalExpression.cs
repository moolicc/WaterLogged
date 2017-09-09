using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Logic.Parsing.Expressions
{
    public class LogicalExpression : IExpression
    {
        public string Expression { get; set; }

        public LogicalExpression(string expression)
        {
            Expression = expression;
        }

        public string Eval(Evaluator evaluator)
        {
            return new NCalc.Expression(Expression).Evaluate().ToString();
        }
    }
}
