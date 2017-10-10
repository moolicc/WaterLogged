using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Parsing.Templater.Tokens
{
    /// <summary>
    /// Represents a property hole with the '@' propert modifier.
    /// </summary>
    /// <inheritdoc />
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
