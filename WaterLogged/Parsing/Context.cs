using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Parsing
{
    public class Context
    {
        public Dictionary<string, Delegate> Functions { get; private set; }

        public Context()
        {
            Functions = new Dictionary<string, Delegate>();
        }

        public virtual Delegate GetDelegate(string name)
        {
            if (!Functions.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Context doesn't contain the function '{name}'.");
            }
            return Functions[name];
        }
    }
}
