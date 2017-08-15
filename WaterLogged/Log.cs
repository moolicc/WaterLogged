using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterLogged
{
    public class Log
    {
        public bool Enabled { get; set; }
        public Formatter Formatter { get; set; }
        public Listener[] Listeners { get { return _listeners.Values.ToArray(); } }
        private Dictionary<string, Listener> _listeners;

        public Log()
        {
            _listeners = new Dictionary<string, Listener>();
        }

        public void AddListener(Listener listener)
        {
            if (listener.Log != null)
            {
                throw new InvalidOperationException("A listener may only be bound to one log at a time.");
            }

            if (string.IsNullOrWhiteSpace(listener.Name))
            {
                listener.Name = DateTime.Now.Ticks.ToString();
            }
            listener.Log = this;
            _listeners.Add(listener.Name, listener);
        }

        public bool ContainsListener(string name)
        {
            return _listeners.ContainsKey(name);
        }

        public Listener GetListener(string name)
        {
            return _listeners[name];
        }

        public void RemoveListener(string name)
        {
            _listeners[name].Log = null;
            _listeners.Remove(name);
        }

        public void ChangeListenerName(string oldName, string newName)
        {
            var oldListener = _listeners[oldName];
            _listeners.Add(newName, oldListener);
        }

        //********************************************
        // WriteLine
        //********************************************
        public void WriteLine(string value, object arg0)
        {
            WriteLine(string.Format(value, arg0));
        }

        public void WriteLine(string value, object arg0, object arg1)
        {
            WriteLine(string.Format(value, arg0, arg1));
        }

        public void WriteLine(string value, object arg0, object arg1, object arg2)
        {
            WriteLine(string.Format(value, arg0, arg1, arg2));
        }

        public void WriteLine(string value, params object[] args)
        {
            WriteLine(string.Format(value, args));
        }

        public void WriteLine(string value)
        {
            WriteTag(value + Environment.NewLine, "");
        }


        //********************************************
        // Write
        //********************************************
        public void Write(string value, object arg0)
        {
            Write(string.Format(value, arg0));
        }

        public void Write(string value, object arg0, object arg1)
        {
            Write(string.Format(value, arg0, arg1));
        }

        public void Write(string value, object arg0, object arg1, object arg2)
        {
            Write(string.Format(value, arg0, arg1, arg2));
        }

        public void Write(string value, params object[] args)
        {
            Write(string.Format(value, args));
        }

        public void Write(string value)
        {
            WriteTag(value, "");
        }


        //********************************************
        // WriteLineTag
        //********************************************
        public void WriteLineTag(string value, string tag, object arg0)
        {
            WriteLineTag(string.Format(value, arg0), tag);
        }

        public void WriteLineTag(string value, string tag, object arg0, object arg1)
        {
            WriteLineTag(string.Format(value, arg0, arg1), tag);
        }

        public void WriteLineTag(string value, string tag, object arg0, object arg1, object arg2)
        {
            WriteLineTag(string.Format(value, arg0, arg1, arg2), tag);
        }

        public void WriteLineTag(string value, string tag, params object[] args)
        {
            WriteLineTag(string.Format(value, args), tag);
        }

        public void WriteLineTag(string value, string tag)
        {
            WriteTag(value + Environment.NewLine, tag);
        }


        //********************************************
        // WriteTag
        //********************************************
        public void WriteTag(string value, string tag, object arg0)
        {
            WriteTag(string.Format(value, arg0), tag);
        }

        public void WriteTag(string value, string tag, object arg0, object arg1)
        {
            WriteTag(string.Format(value, arg0, arg1), tag);
        }

        public void WriteTag(string value, string tag, object arg0, object arg1, object arg2)
        {
            WriteTag(string.Format(value, arg0, arg1, arg2), tag);
        }

        public void WriteTag(string value, string tag, params object[] args)
        {
            WriteTag(string.Format(value, args), tag);
        }

        public void WriteTag(string value, string tag)
        {
            if (!Enabled)
            {
                return;
            }

            if (Formatter != null)
            {
                value = Formatter.Transform(this, value, tag);
            }

            lock (_listeners)
            {
                foreach (var listenerKeyValue in _listeners)
                {
                    if (listenerKeyValue.Value.Enabled && (string.IsNullOrWhiteSpace(tag) || listenerKeyValue.Value.TagFilter.Contains(tag)))
                    {
                        listenerKeyValue.Value.Write(value, tag);
                    }
                }
            }
        }
    }
}
