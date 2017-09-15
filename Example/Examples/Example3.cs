using System;
using WaterLogged;
using WaterLogged.Formatting;

namespace Example.Examples
{
    class Example3 : ExampleBase
    {
        public override string IntroText => "This example demonstrates logical log usage. It echoes back input but takes advantage of logic-based formatting.";
        public override string Name => "Logical formatting usage";

        private bool _flag;

        public Example3()
        {
            var listener = new WaterLogged.Listeners.StandardOut();
            _log.AddListener(listener);
            _log.Formatter = new LogicalFormatter("${message}");
        }

        public override void Echo(string text)
        {
            _log.WriteLine(text);
        }

        public override void Selected()
        {
        }
    }
}
