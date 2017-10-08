using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Parsing.Templater.Tokens
{
    public class SerializePropertyHoleToken : PropertyHoleToken
    {
        public override PropertyModifiers GetModifier()
        {
            return PropertyModifiers.Serialize;
        }

        public override string BuildString()
        {
            if (string.IsNullOrWhiteSpace(Argument))
            {
                return "{$" + PropertyName + "}";
            }
            return "{$" + PropertyName + ":" + Argument + "}";
        }
    }
}
