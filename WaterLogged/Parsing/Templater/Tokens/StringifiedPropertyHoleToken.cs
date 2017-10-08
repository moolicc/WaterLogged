using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Parsing.Templater.Tokens
{
    public class StringifiedPropertyHoleToken : PropertyHoleToken
    {
        public override PropertyModifiers GetModifier()
        {
            return PropertyModifiers.Stringify;
        }

        public override string BuildString()
        {
            if (string.IsNullOrWhiteSpace(Argument))
            {
                return "{@" + PropertyName + "}";
            }
            return "{@" + PropertyName + ":" + Argument + "}";
        }
    }
}
