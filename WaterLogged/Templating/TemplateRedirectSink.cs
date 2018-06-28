using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    /// <summary>
    /// Implements a <see cref="MessageSink"/> that renders a structured message and outputs it back to the owning log.
    /// </summary>
    /// <inheritdoc />
    public class TemplateRedirectSink : MessageSink
    {
        public override void ProcessMessage(StructuredMessage message, string tag)
        {
            Log.WriteTag(TemplateProcessor.BuildString(message), tag);
        }
    }
}
