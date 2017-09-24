using System;
using Newtonsoft.Json.Linq;
using WaterLogged.Templating;

namespace WaterLogged.Serialization.Json
{
    public class JsonFileTemplateSink : TemplatedMessageSink
    {
        public string Filepath { get; private set; }

        public JsonFileTemplateSink()
            : this("log.json")
        {
            
        }

        public JsonFileTemplateSink(string filepath)
        {
            Filepath = filepath;
        }
        
        public override void ProcessMessage(StructuredMessage message, string tag)
        {
            string json = TemplateMessageProcessor.Write(message);
            System.IO.File.AppendAllText(Filepath, json + Environment.NewLine);
        }
    }
}
