using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    /// <summary>
    /// Specifies that the selected member should either be included or discluded in the final message regardless of the Rules applied to the parent type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class IncludeValueAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets whether or not this member should be included in the final message.
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// Instantiates an instance of IncludeValueAttribute.
        /// </summary>
        public IncludeValueAttribute()
        {
            Value = true;
        }
    } 
}
