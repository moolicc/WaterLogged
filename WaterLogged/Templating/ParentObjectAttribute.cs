using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    /// <summary>
    /// Specified a set of rules that message parsing follows.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
    public sealed class ParentObjectAttribute : Attribute
    {
        /// <summary>
        /// The rules that message parsing should follow.
        /// </summary>
        public ParentObjectRules Rules { get => _rules; }

        private ParentObjectRules _rules;

        /// <summary>
        /// Instantiates a new instance of ParentObjectAttribute, with the specified Rules.
        /// </summary>
        /// <param name="rules">The rules to use when parsing a message using this type as its parent.</param>
        public ParentObjectAttribute(ParentObjectRules rules)
        {
            _rules = rules;
        }
    }
}
