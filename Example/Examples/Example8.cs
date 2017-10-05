using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Formatting;
using WaterLogged.Listeners;
using WaterLogged.Serialization.Json;
using WaterLogged.Templating;

namespace Example.Examples
{
    class Example8 : ExampleBase
    {
        public override string IntroText => "This example demonstrates structured-log usage with email.";
        public override string Name => "Structured-log usage with email.";
        
        public Example8()
        {
            _log.AddSink(new TemplateRedirectSink());
            _log.AddSink(new JsonFileTemplateSink("log.json"));
            _log.AddListener(new StandardOut());
            _log.AddListener(new EmailListener(host: "localhost", fromAddress: "testing@Waterlooged.com", ssl: false, port: 25, recipients: new List<string> { "testing@test.com" }));
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
