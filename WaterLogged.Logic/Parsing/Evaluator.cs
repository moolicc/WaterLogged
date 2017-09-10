using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Logic.Parsing
{
    public class Evaluator
    {
        public Context Context { get; set; }
        public Expressions.IExpression[] SourceTree { get; set; }


        public Evaluator(string source)
            : this(new Parser(source).Parse())
        {
            
        }

        public Evaluator(Expressions.IExpression[] sourceTree)
        {
            SourceTree = sourceTree;
        }

        public string Eval()
        {
            var resultBuilder = new StringBuilder();

            foreach (var expressionNode in SourceTree)
            {
                resultBuilder.Append(expressionNode.Eval(this));
            }

            return resultBuilder.ToString();
        }
    }
}
