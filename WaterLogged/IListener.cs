using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public interface IListener : IOutput
    {
        /// <summary>
        /// When overridden in a derived class; Handles an output message.
        /// </summary>
        /// <param name="value">The message.</param>
        /// <param name="tag">The message's tag.</param>
        void Write(string value, string tag);
    }
}
