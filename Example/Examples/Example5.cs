using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Listeners;
using WaterLogged.Templating;

namespace Example.Examples
{
    class Example5 : ExampleBase
    {
        public override string IntroText => "This example demonstrates simple structured-log usage.";
        public override string Name => "Simple structured-log usage.";

        public Example5()
        {
            _log.AddSink(new TemplateRedirectSink());
            _log.AddListener(new StandardOut());
        }

        public override void Echo(string text)
        {
            _log.WriteStructured("Hello, {name}", "", text);
        }

        public override void Selected()
        {
        }
    }
}
