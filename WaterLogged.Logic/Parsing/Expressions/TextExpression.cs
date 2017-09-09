using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Logic.Parsing.Expressions
{
    public class TextExpression : IExpression
    {
        public string Text { get; set; }

        public TextExpression(string text)
        {
            Text = text;
        }

        public string Eval(Evaluator evaluator)
        {
            return Text;
        }
    }
}
