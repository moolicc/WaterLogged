using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Filters;
using WaterLogged.Formatting;

namespace WaterLogged
{
    public class LogBuilder
    {
        public enum Contexts
        {
            Log,
            Listener,
            Sink,
        }

        public Contexts Context { get; set; }
        public FilterManager LogFilter { get; set; }
        public LogicalFormatter Formatter { get; private set; }
        public Listener LastListener { get; private set; }
        public MessageSink LastSink { get; private set; }

        private string _logName;
        private List<Listener> _listeners;
        private List<MessageSink> _messageSinks;

        private object _globalKey;
        private bool _globalPrimary;
        

        public LogBuilder()
        {
            _logName = "";
            _listeners = new List<Listener>();
            _messageSinks = new List<MessageSink>();
            Context = Contexts.Log;
            Formatter = new LogicalFormatter();
            LogFilter = new FilterManager();

            _globalKey = null;
            _globalPrimary = false;
        }

        public LogBuilder WithListener(Listener listener)
        {
            _listeners.Add(listener);
            LastListener = listener;
            Context = Contexts.Listener;
            return this;
        }

        public LogBuilder WithSink(MessageSink sink)
        {
            _messageSinks.Add(sink);
            LastSink = sink;
            Context = Contexts.Sink;
            return this;
        }

        public LogBuilder WithFormatString(string format)
        {
            Formatter.Format = format;
            return this;
        }

        public LogBuilder WithFormatVariable(string key, string value)
        {
            Formatter.Variables.Add(key, value);
            return this;
        }

        public LogBuilder WithFormatFunc(string name, Delegate func)
        {
            Formatter.BaseContext.Functions.Add(name, func);
            return this;
        }

        public LogBuilder Log(string name = "")
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _logName = name;
            }
            Context = Contexts.Log;
            return this;
        }

        public LogBuilder WithName(string name)
        {
            switch (Context)
            {
                case Contexts.Log:
                    _logName = name;
                    break;
            }
            return this;
        }

        public LogBuilder WithFilter(IFilter filter)
        {
            switch (Context)
            {
                case Contexts.Log:
                    LogFilter.Filters.Add(filter);
                    break;
                case Contexts.Listener:
                    LastListener.FilterManager.Filters.Add(filter);
                    break;
                case Contexts.Sink:
                    LastSink.FilterManager.Filters.Add(filter);
                    break;
            }
            return this;
        }

        public LogBuilder WithFilter(FilterPredicate filter)
        {
            switch (Context)
            {
                case Contexts.Log:
                    LogFilter.Filters.Add(new DelegatedFilter(filter));
                    break;
                case Contexts.Listener:
                    LastListener.FilterManager.Filters.Add(new DelegatedFilter(filter));
                    break;
                case Contexts.Sink:
                    LastSink.FilterManager.Filters.Add(new DelegatedFilter(filter));
                    break;
            }
            return this;
        }

        public LogBuilder WithTemplatedFilter(ITemplatedMessageFilter filter)
        {
            switch (Context)
            {
                case Contexts.Log:
                    LogFilter.TemplatedFilters.Add(filter);
                    break;
                case Contexts.Listener:
                    LastListener.FilterManager.TemplatedFilters.Add(filter);
                    break;
                case Contexts.Sink:
                    LastSink.FilterManager.TemplatedFilters.Add(filter);
                    break;
            }
            return this;
        }

        public LogBuilder WithTemplatedFilter(TemplatedFilterPredicate filter)
        {
            switch (Context)
            {
                case Contexts.Log:
                    LogFilter.TemplatedFilters.Add(new DelegatedFilter(filter));
                    break;
                case Contexts.Listener:
                    LastListener.FilterManager.TemplatedFilters.Add(new DelegatedFilter(filter));
                    break;
                case Contexts.Sink:
                    LastSink.FilterManager.TemplatedFilters.Add(new DelegatedFilter(filter));
                    break;
            }
            return this;
        }

        public LogBuilder WithGlobalKey(object key)
        {
            _globalKey = key;
            return this;
        }

        public LogBuilder AsGlobalPrimary()
        {
            _globalPrimary = true;
            return this;
        }


        public Log Build()
        {
            if (_globalPrimary)
            {
                Global.NextLogIsPrimary = true;
            }
            WaterLogged.Log log;
            if (string.IsNullOrWhiteSpace(_logName))
            {
                log = new Log();
            }
            else
            {
                log = new Log(_logName);
            }
            log.FilterManager = LogFilter;
            log.Formatter = Formatter;

            foreach (var listener in _listeners)
            {
                log.AddListener(listener);
            }

            foreach (var sink in _messageSinks)
            {
                log.AddSink(sink);
            }

            if (_globalKey != null)
            {
                Global.GlobalLogs.Add(_globalKey, log);
            }
            return log;
        }
    }
}
