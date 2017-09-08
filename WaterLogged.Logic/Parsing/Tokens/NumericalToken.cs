using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Logic.Parsing.Tokens
{
    public class NumericalToken : DataToken
    {
        public string Contents { get; set; }

        public override object GetValue()
        {
            return new NCalc.Expression(Contents).Evaluate();
        }
    }
}
