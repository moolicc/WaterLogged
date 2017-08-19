using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace WaterLogged
{
    public delegate string Format(Log log, string message, string tag, string arg);

    public class BasicFormatter : Formatter
    {
        public static Dictionary<string, Format> Formatters { get; private set; }
        
        public string Format { get; set; }
        private int _index;

        static BasicFormatter()
        {
            Formatters = new Dictionary<string, Format>();
            Formatters.Add("message", (l, m, t, a) => m);
            Formatters.Add("tag", (l, m, t, a) => t);
            Formatters.Add("log", (l, m, t, a) => l.Name);
            Formatters.Add("datetime", (l, m, t, a) => string.Format("{0:" + a + "}", DateTime.Now));
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

        public override string Transform(Log log, string input, string tag)
        {
            input = input.Trim();
            _index = 0;
            StringBuilder outputBuilder = new StringBuilder();

            for (_index = 0; _index < Format.Length; _index++)
            {
                if (Format[_index] == '$')
                {
                    if (NextChar(Format) == '{')
                    {
                        _index++;
                        _index++;
                        string formatSpecifier = ReadPast(Format, '}', out var foundChar);
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
                        outputBuilder.Append(Formatters[formatSpecifier](log, input, tag, arg));
                        continue;
                    }
                }
                outputBuilder.Append(Format[_index]);
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
