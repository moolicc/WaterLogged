using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    public struct PropertyValue
    {
        public object Value { get; private set; }
        public PropertyModifiers Modifier { get; private set; }

        public PropertyValue(object value, PropertyModifiers modifier)
        {
            Value = value;
            Modifier = modifier;
        }
    }
}
