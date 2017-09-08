using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Logic.Parsing
{
    public class Tokenizer
    {
        private string _expression;
        private int _index;

        //$ means interpret as method
        //# means interpret as number (or bool)
        //% means interpret as string
        //Nothing means interpret as string

        public Tokens.Token[] EvaluateTokens(string expression)
        {
            
        }
    }
}
