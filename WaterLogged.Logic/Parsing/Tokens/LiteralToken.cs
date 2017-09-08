using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Logic.Parsing.Tokens
{
    public class LiteralToken : DataToken
    {
        public string Value { get; set; }

        public override object GetValue()
        {
            return Value.ToString();
        }
    }
}
