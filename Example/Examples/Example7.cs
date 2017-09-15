using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Formatting;
using WaterLogged.Listeners;
using WaterLogged.Serialization.Json;
using WaterLogged.Templating;

namespace Example.Examples
{
    class Example7 : ExampleBase
    {
        public override string IntroText => "This example demonstrates structured-log usage with json serialization.";
        public override string Name => "Structured-log usage with serialization.";
        
        public Example7()
        {
            _log.AddSink(new TemplateRedirectSink());
            _log.AddSink(new JsonFileTemplateSink("log.json"));
            _log.AddListener(new StandardOut());
        }

        public override void Echo(string text)
        {
            _log.WriteStructured("Hello, {name}", "", text);
        }

        public override void Selected()
        {
            Console.WriteLine("Enter a name:");
        }
    }
}
