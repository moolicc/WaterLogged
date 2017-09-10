using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Parsing.Tokens
{
    public class Token
    {
        public int Index { get; set; }
        public string Text { get; set; }

        public Token Init(int index, string text)
        {
            Index = index;
            Text = text;
            return this;
        }
    }
}
