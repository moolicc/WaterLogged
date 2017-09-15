using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    public enum HolePrefix
    {
        /// <summary>
        /// The destructuring operator (@) in front of will serialize the object passed in, rather than convert it using ToString().
        /// (https://github.com/messagetemplates/messagetemplates-csharp/blob/master/README.md#message-template-syntax)
        /// </summary>
        Destructuring,
        /// <summary>
        /// The stringification operator ($) will convert the property value to a string before any other processing takes place, regardless of its type or implemented interfaces.
        /// (https://github.com/messagetemplates/messagetemplates-csharp/blob/master/README.md#message-template-syntax)
        /// </summary>
        Stringification,
        None,
    }
}
