using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    /// <summary>
    /// Represents the value of a property in a message.
    /// </summary>
    public struct PropertyValue
    {
        /// <summary>
        /// Gets the property's value.
        /// </summary>
        public object Value { get; private set; }
        /// <summary>
        /// Gets the property's prefix modifier.
        /// </summary>
        public PropertyModifiers Modifier { get; private set; }

        /// <summary>
        /// Instantiates a new PropertyValue with the specified value and prefix modifier.
        /// </summary>
        public PropertyValue(object value, PropertyModifiers modifier)
        {
            Value = value;
            Modifier = modifier;
        }
    }
}
