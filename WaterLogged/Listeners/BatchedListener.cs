using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WaterLogged.Listeners
{
    /// <summary>
    /// Represents a listener that batches writes.
    /// </summary>
    public abstract class BatchedListener : Listener
    {
        /// <summary>
        /// Gets or sets the number of items to batch before dumping.
        /// </summary>
        protected int BatchCount
        {
            get => __batchCount;
            set
            {
                if(__batchCount == value)
                {
                    return;
                }
                __batchCount = value;
                _batched = new (string Message, string Tag)[value];
            }
        }

        /// <summary>
        /// A value indicating if dumps should occur on a different thread.
        /// </summary>
        protected bool _threadedDump;
        /// <summary>
        /// A value indicating the number of currently batched messages.
        /// </summary>
        protected int _curBatchedItems;

        private int __batchCount;
        private (string Message, string Tag)[] _batched;

        /// <summary>
        /// Instantiates the BatchedListener.
        /// </summary>
        /// <param name="batchCount">The number of items to batch before dumping.</param>
        /// <param name="threadedDump">A value indicating if dumps should occur on different threads.</param>
        protected BatchedListener(int batchCount, bool threadedDump)
        {
            BatchCount = batchCount;
            _threadedDump = threadedDump;
            _curBatchedItems = 0;
        }

        ///<inheritdoc />
        public override void Write(string value, string tag)
        {
            _batched[_curBatchedItems] = (value, tag);
            _curBatchedItems++;
            if(_curBatchedItems >= __batchCount)
            {
                Dump();
            }
        }

        /// <summary>
        /// When overridden in a derived class, outputs a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="tag"></param>
        protected abstract void DumpMessage(string message, string tag);

        /// <summary>
        /// Dumps the currently batched messages.
        /// </summary>
        public void Dump()
        {
            var collection = _batched;
            if(_threadedDump)
            {
                collection = new (string Message, string Tag)[_batched.Length];
                Array.Copy(_batched, collection, _batched.Length);
                _curBatchedItems = 0;
                Task.Factory.StartNew((o) => DoDump(((string Message, string Tag)[])o), collection);
                return;
            }
            DoDump(collection);
            _batched = new (string Message, string Tag)[__batchCount];
        }

        private void DoDump((string Message, string Tag)[] items)
        {
            foreach(var item in items)
            {
                DumpMessage(item.Message, item.Tag);
            }
        }
    }
}
