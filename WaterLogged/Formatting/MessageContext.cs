using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Parsing;

namespace WaterLogged.Formatting
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

            Functions.Add("log", new Func<string>(() => log.Name));
            Functions.Add("tag", new Func<string>(() => tag));
            Functions.Add("message", new Func<string>(() => message));
            Functions.Add("datetime", new Func<string>(() => DateTime.Now.ToString()));
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
            Functions.Clear();
        }
    }
}