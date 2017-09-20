using System.Collections.Generic;
using System.Linq;
using WaterLogged.Listeners;

namespace WaterLogged
{
    /// <summary>
    /// Static class containing global configuration values and logs.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Gets or sets a value indicating if a <see cref="Log"/> should automatically add a
        /// <see cref="WaterLogged.Listeners.StandardOut"/> listener upon Log creation if the DEBUG flag is defined.
        /// </summary>
        public static bool AddConsoleListenerOnDebug { get; set; }
        /// <summary>
        /// Gets or sets a value indicating if the next <see cref="Log"/> to be created will automatically become the PrimaryLog.
        /// </summary>
        public static bool NextLogIsPrimary { get; set; }
        /// <summary>
        /// Gets or sets the PrimaryLog.
        /// </summary>
        public static Log PrimaryLog
        {
            get { return _primaryLog; }
            set
            {
                if (NextLogIsPrimary)
                {
                    NextLogIsPrimary = false;
                }
                _primaryLog = value;
            }
        }
        /// <summary>
        /// A collection of Logs accessible by any type of key.
        /// </summary>
        /// <remarks>
        /// This is so that you can use an enum, for example, to access specific logs.
        /// Therefore, you could have a Logs enum with items like: "Debug", "Error" and "Network", and map these enum values to specific <see cref="Log"/>s.
        /// </remarks>
        public static Dictionary<object, Log> GlobalLogs { get; private set; }

        private static Log _primaryLog;

        static Global()
        {
            _primaryLog = null;

            AddConsoleListenerOnDebug = true;
            NextLogIsPrimary = true;
            GlobalLogs = new Dictionary<object, Log>();
        }

        internal static void LogCreated(Log log)
        {
            if (AddConsoleListenerOnDebug)
            {
#if DEBUG
                log.AddListener(new StandardOut());
#endif
            }
            if (NextLogIsPrimary)
            {
                PrimaryLog = log;
            }
        }
    }
}
