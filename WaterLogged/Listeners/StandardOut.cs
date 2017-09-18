using System;
using System.Collections.Generic;

namespace WaterLogged.Listeners
{
    /// <summary>
    /// Implements a listener which outputs to the Standard Output Stream.
    /// </summary>
    public class StandardOut : Listener
    {
        /// <summary>
        /// A Dictionary mapping tags to specific <see cref="ConsoleColor"/>s.
        /// Output foreground color will be selected based on the contents.
        /// </summary>
        public Dictionary<string, ConsoleColor> ForeColorMap { get; private set; }

        /// <summary>
        /// A Dictionary mapping tags to specific <see cref="ConsoleColor"/>s.
        /// Output background color will be selected based on the contents.
        /// </summary>
        public Dictionary<string, ConsoleColor> BackColorMap { get; private set; }

        /// <summary>
        /// Instantiates a new instance of the StandardOut listener.
        /// </summary>
        public StandardOut()
        {
            ForeColorMap = new Dictionary<string, ConsoleColor>();
            BackColorMap = new Dictionary<string, ConsoleColor>();
        }

        /// <summary>
        /// A function wrapping ForeColorMap.Add(string, ConsoleColor) for use with deserialization.
        /// </summary>
        /// <param name="tag">The tag to map to the specified color.</param>
        /// <param name="color">The color to map to the specified tag.</param>
        public void MapForeColor(string tag, ConsoleColor color)
        {
            ForeColorMap.Add(tag, color);
        }

        /// <summary>
        /// A function wrapping BackColorMap.Add(string, ConsoleColor) for use with deserialization.
        /// </summary>
        /// <param name="tag">The tag to map to the specified color.</param>
        /// <param name="color">The color to map to the specified tag.</param>
        public void MapBackColor(string tag, ConsoleColor color)
        {
            BackColorMap.Add(tag, color);
        }

        /// <summary>
        /// Outputs to the Standard Output Stream optionally using
        /// different foreground/background text colors based on the respective properties.
        /// </summary>
        /// <param name="value">The message to output.</param>
        /// <param name="tag">The message's tag.</param>
        public override void Write(string value, string tag)
        {
            var curForeColor = Console.ForegroundColor;
            var curBackColor = Console.BackgroundColor;

            if (ForeColorMap.ContainsKey(tag))
            {
                Console.ForegroundColor = ForeColorMap[tag];
            }
            if (BackColorMap.ContainsKey(tag))
            {
                Console.BackgroundColor = BackColorMap[tag];
            }

            Console.Write(value);

            Console.ForegroundColor = curForeColor;
            Console.BackgroundColor = curBackColor;
        }
    }
}
