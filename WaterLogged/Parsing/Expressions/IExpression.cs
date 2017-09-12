using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Parsing.Expressions
{
    public interface IExpression
    {
        string Eval(Evaluator evaluator);
    }
}
