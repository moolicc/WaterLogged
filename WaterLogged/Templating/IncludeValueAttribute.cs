using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class IncludeValueAttribute : Attribute
    {
        public bool Value { get; set; }

        public IncludeValueAttribute()
        {
            Value = true;
        }
    } 
}
