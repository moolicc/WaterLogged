using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Parsing.Templater.Tokens
{
    public class LiteralToken : Token
    {
        public string Text { get; private set; }

        public LiteralToken SetText(string text)
        {
            Text = text;
            return this;
        }
    }
}
