using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Logic.Parsing
{
    public class Evaluator
    {
        public Context Context { get; set; }
        public string Source { get; private set; }
        public Tokenizer Tokenizer { get; private set; }

        public Evaluator(string source)
        {
            Context = new Context();
            Source = source;
            Tokenizer = new Tokenizer();
        }

        public string Evaluate()
        {
            var tokens = Tokenizer.EvaluateTokens(Source);
            var result = new StringBuilder();

            return result.ToString();
        }
    }
}
