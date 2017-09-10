using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Parsing;

namespace WaterLogged.Supplement
{
    public class MessageContext : Context, IDisposable
    {
        public Context BaseContext { get; private set; }
        public Log Log { get; private set; }
        public string Tag { get; private set; }
        public string Message { get; private set; }

        public MessageContext(Context baseContext, Log log, string tag, string message)
        {
            BaseContext = baseContext;
            Log = log;
            Tag = tag;
            Message = message;

            Variables.Add("log", log.Name);
            Variables.Add("tag", tag);
            Variables.Add("message", message);
            Variables.Add("datetime", DateTime.Now);
        }

        public override object GetVariable(string name)
        {
            if (Variables.ContainsKey(name))
            {
                return Variables[name];
            }
            if (BaseContext.Variables.ContainsKey(name))
            {
                return BaseContext.Variables[name];
            }
            return base.GetVariable(name);
        }

        public override Delegate GetDelegate(string name)
        {
            if (Functions.ContainsKey(name))
            {
                return Functions[name];
            }
            if (BaseContext.Functions.ContainsKey(name))
            {
                return BaseContext.Functions[name];
            }
            return base.GetDelegate(name);
        }

        public void Dispose()
        {
            BaseContext = null;
            Log = null;
            Tag = null;
            Message = null;
            Variables.Clear();
            Functions.Clear();
        }
    }
}
