using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Formatting;
using WaterLogged.Listeners;
using WaterLogged.Templating;

namespace Example.Examples
{
    class Example6 : ExampleBase
    {
        public override string IntroText => "This example demonstrates structured-log usage combined with logical-formatting.";
        public override string Name => "Structured-log usage with formatting.";

        private bool _tagSwitch;
        private string _tag;

        public Example6()
        {
            //TODO:
            //_log.AddSink(new TemplateRedirectSink());
            _log.AddListener(new StandardOut());
            _log.Formatter = new LogicalFormatter();
            _tagSwitch = false;
            _tag = "";
        }

        public override void Echo(string text)
        {
            _tagSwitch = !_tagSwitch;
            if (_tagSwitch)
            {
                _tag = text;
                Console.WriteLine("Enter a name. It will only be printed if the tag starts with the letter 'b'.");
                return;
            }
            _log.WriteStructured("Hello, ${when:${startswith:${tag},b},\\{name\\}}${newline}", _tag, text);
            Console.WriteLine("Enter a tag:");
        }

        public override void Selected()
        {
            Console.WriteLine("Enter a tag:");
        }
    }
}
