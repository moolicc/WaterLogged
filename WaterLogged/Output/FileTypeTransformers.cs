using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Output
{
    public delegate string FileTypeTransformer(string input = "", StructuredMessage? message = null);

    public sealed class FileTypeTransformers
    {
        public static FileTypeTransformer TextTransformer = (s, m) =>
        {
            if (m.HasValue)
            {
                return TemplateProcessor.BuildString(m.Value);
            }
            return s;
        };
    }
}
