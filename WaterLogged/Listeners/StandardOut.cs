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
