using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Parsing
{
    public class Context
    {
        public Dictionary<string, object> Variables { get; private set; }
        public Dictionary<string, Delegate> Functions { get; private set; }

        public Context()
        {
            Variables = new Dictionary<string, object>();
            Functions = new Dictionary<string, Delegate>();
        }

        public virtual object GetVariable(string name)
        {
            if (!Variables.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Context doesn't contain the variable '${name}'.");
            }
            return Variables[name];
        }

        public virtual Delegate GetDelegate(string name)
        {
            if (!Functions.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Context doesn't contain the function '${name}'.");
            }
            return Functions[name];
        }
    }
}
