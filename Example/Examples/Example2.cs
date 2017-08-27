using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged;

namespace Example.Examples
{
    class Example2 : ExampleBase
    {
        public override string IntroText => "This example demonstrates simple log usage. It echoes back input but takes advantage of formatting. Also takes advantage of color-coding tags in the StandardOut listener.";
        public override string Name => "Simple formatting usage";

        private bool _flag;

        public Example2()
        {
            var listener = new WaterLogged.Listeners.StandardOut();
            listener.BackColorMap.Add("info", ConsoleColor.Yellow);
            listener.BackColorMap.Add("error", ConsoleColor.Red);
            _log.AddListener(listener);
            _log.Formatter = new BasicFormatter("[${datetime}] [${tag}] [${builddate}] ${message}");
        }

        public override void Echo(string text)
        {
            string tag = "";
            if (_flag)
            {
                tag = "info";
            }
            else
            {
                tag = "error";
            }
            _flag = !_flag;

            _log.WriteLineTag(text, tag);
        }
    }
}
