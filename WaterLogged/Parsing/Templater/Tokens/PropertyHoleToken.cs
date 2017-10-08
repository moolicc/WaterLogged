using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Parsing.Templater.Tokens
{
    public class PropertyHoleToken : Token
    {
        public string PropertyName { get; private set; }
        public string Argument { get; private set; }

        public PropertyHoleToken SetNameAndArg(string name, string arg)
        {
            PropertyName = name;
            Argument = arg;
            return this;
        }

        public virtual PropertyModifiers GetModifier()
        {
            return PropertyModifiers.NoneSpecified;
        }

        public override string BuildString()
        {
            if (string.IsNullOrWhiteSpace(Argument))
            {
                return "{" + PropertyName + "}";
            }
            return "{" + PropertyName + ":" + Argument + "}";
        }
    }
}
