using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    public class TemplateRedirectSink : TemplatedMessageSink
    {
        public override void ProcessMessage(Log log, StructuredMessage message, string tag)
        {
            log.WriteTag(TemplateProcessor.ProcessMessage(message), tag);
        }
    }
}
