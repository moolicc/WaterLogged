using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    /// <summary>
    /// Represents the modifier prefix on property holes.
    /// </summary>
    public enum PropertyModifiers
    {
        /// <summary>
        /// No prefix specified.
        /// </summary>
        NoneSpecified,
        /// <summary>
        /// Specifies that a property should always be converted into a string.
        /// </summary>
        Stringify,
        /// <summary>
        /// Specifies that a property should always be serialized into the message.
        /// </summary>
        Serialize,
    }
}
