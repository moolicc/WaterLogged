using System;

namespace WaterLogged.Serialization
{
    public abstract class LogWriter
    {
        public abstract void BeginWriteFormatter(Formatter formatter);
        public abstract void EndWriteFormatter(Formatter formatter);
        public abstract void BeginWriteLog(Log log);
        public abstract void BeginWriteListener(Listener listener);
        public abstract void EndWriteListener(Listener listener);
        public abstract void EndWriteLog(Log log);
    }
}
