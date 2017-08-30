using System;
using System.Collections.Generic;

namespace WaterLogged.Listeners
{
    public class StandardOut : Listener
    {
        public Dictionary<string, ConsoleColor> ForeColorMap { get; private set; }
        public Dictionary<string, ConsoleColor> BackColorMap { get; private set; }

        public StandardOut()
        {
            ForeColorMap = new Dictionary<string, ConsoleColor>();
            BackColorMap = new Dictionary<string, ConsoleColor>();
        }

        public void MapForeColor(string tag, ConsoleColor color)
        {
            ForeColorMap.Add(tag, color);
        }

        public void MapBackColor(string tag, ConsoleColor color)
        {
            BackColorMap.Add(tag, color);
        }

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
