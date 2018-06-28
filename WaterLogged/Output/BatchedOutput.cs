using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaterLogged.Templating;

namespace WaterLogged.Output
{
    /// <summary>
    /// Represents a listener/sink that batches writes.
    /// </summary>
    public abstract class BatchedOutput : ListenerSink
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
                _batchedTemplated = new (StructuredMessage Message, string Tag)[value];
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
        /// <summary>
        /// A value indicating the number of currently batched templated messages.
        /// </summary>
        protected int _curBatchedTemplatedItems;

        private int __batchCount;
        private (string Message, string Tag)[] _batched;
        private (StructuredMessage Message, string Tag)[] _batchedTemplated;

        /// <summary>
        /// Instantiates the BatchedListener.
        /// </summary>
        /// <param name="batchCount">The number of items to batch before dumping.</param>
        /// <param name="threadedDump">A value indicating if dumps should occur on different threads.</param>
        protected BatchedOutput(int batchCount, bool threadedDump)
        {
            BatchCount = batchCount;
            _threadedDump = threadedDump;
            _curBatchedItems = 0;
            _curBatchedTemplatedItems = 0;
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

        ///<inheritdoc />
        public override void ProcessMessage(StructuredMessage message, string tag)
        {
            _batchedTemplated[_curBatchedTemplatedItems] = (message, tag);
            _curBatchedTemplatedItems++;
            if(_curBatchedTemplatedItems >= __batchCount)
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
        /// When overridden in a derived class, outputs a templated message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="tag"></param>
        protected abstract void DumpMessage(StructuredMessage message, string tag);

        /// <summary>
        /// Dumps the currently batched messages.
        /// </summary>
        public void Dump()
        {
            var collection = _batched;
            var templatedCollection = _batchedTemplated;
            if(_threadedDump)
            {
                collection = new (string Message, string Tag)[_batched.Length];
                templatedCollection = new (StructuredMessage Message, string Tag)[_batchedTemplated.Length];
                Array.Copy(_batched, collection, _batched.Length);
                Array.Copy(_batchedTemplated, templatedCollection, _batchedTemplated.Length);

                _curBatchedItems = 0;
                _curBatchedTemplatedItems = 0;

                Task.Factory.StartNew((o) => DoDump(((string Message, string Tag)[])o), collection);
                Task.Factory.StartNew((o) => DoTemplatedDump(((StructuredMessage Message, string Tag)[])o), templatedCollection);
                return;
            }
            DoDump(collection);
            DoTemplatedDump(templatedCollection);
            _batched = new (string Message, string Tag)[__batchCount];
            _batchedTemplated = new (StructuredMessage Message, string Tag)[__batchCount];
        }

        private void DoDump((string Message, string Tag)[] items)
        {
            foreach(var item in items)
            {
                DumpMessage(item.Message, item.Tag);
            }
        }

        private void DoTemplatedDump((StructuredMessage Message, string Tag)[] items)
        {
            foreach(var item in items)
            {
                DumpMessage(item.Message, item.Tag);
            }
        }
    }
}
