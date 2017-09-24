using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Formatting;
using WaterLogged.Listeners;
using WaterLogged.Serialization.Json;
using WaterLogged.Templating;
using WaterLogged.Supplement;

namespace Example.Examples
{
    class Example9 : ExampleBase
    {
        public override string IntroText => "This example demonstrates loading a log from a json configuration file.";
        public override string Name => "Loading json configuration file.";
        
        public Example9()
        {
            Console.WriteLine(typeof(WaterLogged.Log).AssemblyQualifiedName);
            JsonConfigReader reader = new JsonConfigReader();
            var config = reader.Read(System.IO.File.ReadAllText("exampleconfig.json"));
            _log = config.ResolveLog("mylog");
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
