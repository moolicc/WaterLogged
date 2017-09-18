using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WaterLogged.Templating;

namespace WaterLogged.Serialization.Json
{
    public class TemplateMessageProcessor
    {
        public static StructuredMessage Read(string json)
        {
            var jObject = JObject.Parse(json);
            string template = jObject["template"].Value<string>();
            DateTime time = jObject["time"].Value<DateTime>();

            List<(string, object)> holeValues = new List<(string, object)>();
            foreach (var value in jObject)
            {
                if (value.Key == "template" || value.Key == "time")
                {
                    continue;
                }
                holeValues.Add((value.Key, value.Value.Value<object>()));
            }
            return TemplateProcessor.ProcessNamedTemplate(template, holeValues.ToArray());
        }

        public static string Write(StructuredMessage message)
        {
            var jObject = new JObject();
            jObject.Add("time", message.CreationDate);
            jObject.Add("template", message.Template);

            foreach (var messageValue in message.Values)
            {
                object value = null;
                if (messageValue.Value.Hole.Prefix == HolePrefix.Destructuring)
                {
                    value = messageValue.Value.Value;
                }
                else if (messageValue.Value.Hole.Prefix == HolePrefix.Stringification)
                {
                    value = messageValue.Value.Value.ToString();
                }
                else
                {
                    value = messageValue.Value.Value.ToString();
                }
                jObject.Add(messageValue.Key.Id.NamedId, JToken.FromObject(value));
            }
            foreach (var messageValue in message.ContextValues)
            {
                jObject.Add(messageValue.Key, JToken.FromObject(messageValue.Value));
            }

            return jObject.ToString();
        }
    }
}
