using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ParentObjectAttribute : Attribute
    {
        private readonly ParentObjectResolveRules _rules;

        public ParentObjectAttribute(ParentObjectResolveRules rules)
        {
            _rules = rules;
        }

        public ParentObjectResolveRules GetRules()
        {
            return _rules;
        }
    }
}
