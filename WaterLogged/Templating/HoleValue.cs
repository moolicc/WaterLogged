using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    public class HoleValue
    {
        public Hole Hole { get; private set; }
        public object Value { get; private set; }

        public HoleValue(Hole hole, object value)
        {
            Hole = hole;
            Value = value;
        }
    }
}
