using System;
using System.Collections.Generic;
using System.Linq;
using WaterLogged.Templating;

namespace WaterLogged
{
    public class Log
    {
        public string Name { get; private set; }
        public bool Enabled { get; set; }
        public Formatter Formatter { get; set; }
        public Listener[] Listeners { get { return _listeners.Values.ToArray(); } }
        public TemplatedMessageSink[] Sinks { get { return _sinks.Values.ToArray(); } }

        private Dictionary<string, Listener> _listeners;
        private Dictionary<string, TemplatedMessageSink> _sinks;


        public Log()
            : this(string.Format("log{0}", DateTime.Now.Ticks))
        {
        }

        public Log(string name)
        {
            _listeners = new Dictionary<string, Listener>();
            _sinks = new Dictionary<string, TemplatedMessageSink>();
            Name = name;
            Enabled = true;
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
            _listeners.Remove(oldName);
        }


        public void AddSink(TemplatedMessageSink sink)
        {
            if (sink.Log != null)
            {
                throw new InvalidOperationException("A template message sink may only be bound to one log at a time.");
            }

            if (string.IsNullOrWhiteSpace(sink.Name))
            {
                sink.Name = DateTime.Now.Ticks.ToString();
            }
            sink.Log = this;
            _sinks.Add(sink.Name, sink);
        }

        public bool ContainsSink(string name)
        {
            return _sinks.ContainsKey(name);
        }

        public TemplatedMessageSink GetSink(string name)
        {
            return _sinks[name];
        }

        public void RemoveSink(string name)
        {
            _sinks[name].Log = null;
            _sinks.Remove(name);
        }

        public void ChangeSinkName(string oldName, string newName)
        {
            var oldSink = _sinks[oldName];
            _sinks.Add(newName, oldSink);
            _sinks.Remove(oldName);
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

            string formattedValue = value;
            if (Formatter != null)
            {
                formattedValue = Formatter.Transform(this, value, tag, new Dictionary<string, string>());
            }

            lock (_listeners)
            {
                foreach (var listenerKeyValue in _listeners)
                {
                    if (listenerKeyValue.Value.Enabled && (string.IsNullOrWhiteSpace(tag) || listenerKeyValue.Value.TagFilter.Contains(tag) || listenerKeyValue.Value.TagFilter.Length == 0))
                    {
                        if (listenerKeyValue.Value.FormatterArgs.Count > 0)
                        {
                            if (Formatter != null)
                            {
                                listenerKeyValue.Value.Write(Formatter.Transform(this, value, tag, listenerKeyValue.Value.FormatterArgs), tag);
                                continue;
                            }
                        }
                        listenerKeyValue.Value.Write(formattedValue, tag);
                    }
                }
            }
        }


        //********************************************
        // WriteObject
        //********************************************
        public void WriteObject(object value, object argument = null)
        {
            WriteObjectTag(value, "", argument);
        }

        public void WriteObject(object value, ObjectTransformer transformer, object argument = null)
        {
            WriteObjectTag(value, "", transformer, argument);
        }


        //********************************************
        // WriteObjectTag
        //********************************************
        public void WriteObjectTag(object value, string tag, object argument = null)
        {
            string result = "";
            if (ObjectTransformer.ContainsTransformer(value.GetType()))
            {
                result = ObjectTransformer.GetTransformer(value.GetType()).Transform(value, argument);
            }
            else
            {
                result = value.ToString();
            }
            WriteTag(result, tag);
        }

        public void WriteObjectTag(object value, string tag, ObjectTransformer transformer, object argument = null)
        {
            WriteTag(transformer.Transform(value, argument), tag);
        }



        public void WriteStructuredNamed(string template, string tag, params (string, object)[] holeValues)
        {
            if (!Enabled)
            {
                return;
            }
            if (Formatter != null)
            {
                template = Formatter.Transform(template, this, tag, new Dictionary<string, string>());
            }
            var message = TemplateProcessor.ProcessNamedTemplate(template, holeValues);

            lock (_sinks)
            {
                foreach (var sinkKeyValue in _sinks)
                {
                    if (sinkKeyValue.Value.Enabled && (string.IsNullOrWhiteSpace(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Contains(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Length == 0))
                    {
                        sinkKeyValue.Value.ProcessMessage(this, message, tag);
                    }
                }
            }
        }

        public void WriteStructured(string template, string tag, params object[] holeValues)
        {
            if (!Enabled)
            {
                return;
            }
            if (Formatter != null)
            {
                template = Formatter.Transform(template, this, tag, new Dictionary<string, string>());
            }
            var message = TemplateProcessor.ProcessTemplate(template, holeValues);

            lock (_sinks)
            {
                foreach (var sinkKeyValue in _sinks)
                {
                    if (sinkKeyValue.Value.Enabled && (string.IsNullOrWhiteSpace(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Contains(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Length == 0))
                    {
                        sinkKeyValue.Value.ProcessMessage(this, message, tag);
                    }
                }
            }
        }

        public void WriteStructuredParent(string template, string tag, object parentObject)
        {
            if (!Enabled)
            {
                return;
            }
            if (Formatter != null)
            {
                template = Formatter.Transform(template, this, tag, new Dictionary<string, string>());
            }
            var message = TemplateProcessor.ProcessParentedTemplate(template, parentObject);

            lock (_sinks)
            {
                foreach (var sinkKeyValue in _sinks)
                {
                    if (sinkKeyValue.Value.Enabled && (string.IsNullOrWhiteSpace(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Contains(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Length == 0))
                    {
                        sinkKeyValue.Value.ProcessMessage(this, message, tag);
                    }
                }
            }
        }

        public void WriteStructuredStaticParent(string template, string tag, Type parentType)
        {
            if (!Enabled)
            {
                return;
            }
            if (Formatter != null)
            {
                template = Formatter.Transform(template, this, tag, new Dictionary<string, string>());
            }
            
            var message = TemplateProcessor.ProcessParentedTemplate(template, parentType);

            lock (_sinks)
            {
                foreach (var sinkKeyValue in _sinks)
                {
                    if (sinkKeyValue.Value.Enabled && (string.IsNullOrWhiteSpace(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Contains(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Length == 0))
                    {
                        sinkKeyValue.Value.ProcessMessage(this, message, tag);
                    }
                }
            }
        }

        public void WriteStructuredMessage(StructuredMessage message, string tag)
        {
            if (!Enabled)
            {
                return;
            }
            lock (_sinks)
            {
                foreach (var sinkKeyValue in _sinks)
                {
                    if (sinkKeyValue.Value.Enabled && (string.IsNullOrWhiteSpace(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Contains(tag) ||
                                                       sinkKeyValue.Value.TagFilter.Length == 0))
                    {
                        sinkKeyValue.Value.ProcessMessage(this, message, tag);
                    }
                }
            }
        }
    }
}
