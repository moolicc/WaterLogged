using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Parsing.Expressions
{
    public class ComboExpression : IExpression
    {
        public IExpression Left { get; private set; }
        public IExpression Right { get; private set; }

        public ComboExpression(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        public string Eval(Evaluator evaluator)
        {
            return Left.Eval(evaluator) + Right.Eval(evaluator);
        }
    }
}
