using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Parsing.Templater.Tokens
{
    /// <summary>
    /// Represents a property hole in a template string.
    /// </summary>
    /// <inheritdoc />
    public class PropertyHoleToken : Token
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string PropertyName { get; private set; }
        /// <summary>
        /// Gets the optional argument of the property.
        /// </summary>
        public string Argument { get; private set; }

        /// <summary>
        /// Sets the property name and argument of this property token.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="arg">The property's optional argument.</param>
        public PropertyHoleToken SetNameAndArg(string name, string arg)
        {
            PropertyName = name;
            Argument = arg;
            return this;
        }

        /// <summary>
        /// Returns the property modifier of this hole.
        /// Eg. '$', '@', or neither
        /// </summary>
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
