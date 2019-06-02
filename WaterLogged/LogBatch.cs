using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaterLogged
{
    /// <summary>
    /// Handles multithreaded log writing.
    /// </summary>
    public static class LogBatch
    {
        /// <summary>
        /// Gets or sets a value representing the interval at which the pool should push out messages.
        /// </summary>
        public static int PushDelay { get; set; }
        /// <summary>
        /// Gets or sets a value representing the interval at which the pool should push out consecutive messages.
        /// </summary>
        public static int PushConsecutiveDelay { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether or not the multi-threaded pool should be used.
        /// </summary>
        public static bool UsePool
        {
            get => _usePool;
            set
            {
                if(value == _usePool)
                {
                    return;
                }
                _usePool = value;
                if(_usePool)
                {
                    _taskCancel = new CancellationTokenSource();
                    _task = Task.Factory.StartNew(Run, _taskCancel.Token);
                }
                else
                {
                    _taskCancel.Cancel();
                    _task.Wait();
                    _task.Dispose();
                    _taskCancel.Dispose();
                    _task = null;
                    _taskCancel = null;
                }
            }
        }

        private static bool _usePool;
        private static Task _task;
        private static CancellationTokenSource _taskCancel;
        private static ConcurrentQueue<StagedMessage> _staged;
        private static ConcurrentQueue<StagedTemplatedMessage> _stagedTemplated;

        static LogBatch()
        {
            PushDelay = 100;
            PushConsecutiveDelay = 10;
            _usePool = false;
            _staged = new ConcurrentQueue<StagedMessage>();
            _stagedTemplated = new ConcurrentQueue<StagedTemplatedMessage>();
        }

        internal static bool StageMessage(Log log, string message, string tag)
        {
            if(!UsePool)
            {
                return false;
            }
            _staged.Enqueue(new StagedMessage(log, message, tag));
            return true;
        }

        internal static bool StageMessage(Log log, Templating.StructuredMessage message, string tag)
        {
            if(!UsePool)
            {
                return false;
            }
            _stagedTemplated.Enqueue(new StagedTemplatedMessage(log, message, tag));
            return true;
        }

        private static void Run()
        {
            try
            {
                int delay = PushDelay;
                while(_usePool)
                {
                    Thread.Sleep(delay);
                    delay = PushDelay;

                    if(HandleMessages())
                    {
                        delay = PushConsecutiveDelay;
                    }
                    if(HandleTemplatedMessages())
                    {
                        delay = PushConsecutiveDelay;
                    }
                }
            }
            catch(TaskCanceledException ex)
            {
            }
        }

        private static bool HandleMessages()
        {
            if(_staged.Count == 0 || !_staged.TryDequeue(out var message))
            {
                return false;
            }
            message.Log.PushMessage(message.Message, message.Tag);
            return true;
        }

        private static bool HandleTemplatedMessages()
        {
            if(_stagedTemplated.Count == 0 || !_stagedTemplated.TryDequeue(out var message))
            {
                return false;
            }
            message.Log.WriteStructuredMessage(message.Message, message.Tag);
            return true;
        }

        private class StagedMessage
        {
            public Log Log { get; set; }
            public string Message { get; set; }
            public string Tag { get; set; }

            public StagedMessage(Log log, string message, string tag)
            {
                Message = message;
                Log = log;
                Tag = tag;
            }
        }

        private class StagedTemplatedMessage
        {
            public Log Log { get; set; }
            public Templating.StructuredMessage Message { get; set; }
            public string Tag { get; set; }

            public StagedTemplatedMessage(Log log, Templating.StructuredMessage message, string tag)
            {
                Message = message;
                Log = log;
                Tag = tag;
            }
        }
    }
}
