using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Parsing.Templater.Tokens
{
    public abstract class Token
    {
        public int Index { get; private set; }

        public Token SetIndex(int index)
        {
            Index = index;
            return this;
        }

        public abstract string BuildString();
    }
}
