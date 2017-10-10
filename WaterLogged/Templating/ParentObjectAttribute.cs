using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
    public sealed class ParentObjectAttribute : Attribute
    {
        public ParentObjectRules Rules { get => _rules; }

        private ParentObjectRules _rules;

        public ParentObjectAttribute(ParentObjectRules rules)
        {
            _rules = rules;
        }
    }
}
