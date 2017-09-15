using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class ParentObjectValueAttribute : Attribute
    {
        private readonly ParentObjectValueInclusion _inclusion;
        public ParentObjectValueAttribute(ParentObjectValueInclusion inclusion)
        {
            _inclusion = inclusion;
        }

        public ParentObjectValueInclusion GetInclusion()
        {
            return _inclusion;
        }
    }
}
