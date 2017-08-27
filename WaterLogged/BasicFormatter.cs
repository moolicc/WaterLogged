using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace WaterLogged
{
    public delegate string Format(FormatData data);

    public class BasicFormatter : Formatter
    {
        public static Dictionary<string, Format> Formatters { get; private set; }
        private static Formatters _formatters;

        public string Format { get; set; }
        private int _index;

        static BasicFormatter()
        {
            _formatters = new Formatters();

            Formatters = new Dictionary<string, Format>();
            Formatters.Add("message", d => d.Message);
            Formatters.Add("tag", d => d.Tag);
            Formatters.Add("log", d => d.Log.Name);
            Formatters.Add("datetime", d => string.Format("{0:" + d.Argument + "}", DateTime.Now));
            Formatters.Add("memory", _formatters.GetMemoryUsage);
        }

        public BasicFormatter()
            : this("${message}")
        {
            
        }

        public BasicFormatter(string format)
        {
            Format = format;
            _index = 0;
        }

        public override string Transform(Log log, string input, string tag, Dictionary<string, string> args)
        {
            //input = input.Trim();
            _index = 0;
            StringBuilder outputBuilder = new StringBuilder();

            string format = Format;
            if (args.ContainsKey("format"))
            {
                format = args["format"];
            }

            for (_index = 0; _index < format.Length; _index++)
            {
                if (format[_index] == '$')
                {
                    if (NextChar(format) == '{')
                    {
                        _index++;
                        _index++;
                        string formatSpecifier = ReadPast(format, '}', out var foundChar);
                        string arg = "";
                        if (!foundChar)
                        {
                            throw new FormatException("Closing '}' not found.");
                        }
                        if (formatSpecifier.Contains(":"))
                        {
                            var split = formatSpecifier.Split(new char[] {':'}, 2);
                            formatSpecifier = split[0];
                            arg = split[1];
                        }
                        if (!Formatters.ContainsKey(formatSpecifier))
                        {
                            throw new FormatException("Formatter '" + formatSpecifier + "' not found.");
                        }
                        outputBuilder.Append(Formatters[formatSpecifier](new FormatData(log, this, input, tag, arg)));
                        continue;
                    }
                }
                outputBuilder.Append(format[_index]);
            }

            return outputBuilder.ToString();
        }

        private char NextChar(string input)
        {
            if (_index + 1 >= input.Length)
            {
                return '\0';
            }
            return input[_index + 1];
        }

        private string ReadPast(string input, char token, out bool foundChar)
        {
            StringBuilder builder = new StringBuilder();
            foundChar = false;

            for (int i = _index; i < input.Length; i++)
            {
                if (input[i] == token)
                {
                    foundChar = true;
                    break;
                }
                _index++;
                builder.Append(input[i]);
            }

            return builder.ToString();
        }
    }
}
