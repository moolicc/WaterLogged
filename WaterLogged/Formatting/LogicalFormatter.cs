using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using WaterLogged.Parsing;
using WaterLogged.Parsing.Expressions;

namespace WaterLogged.Formatting
{
    public class LogicalFormatter : Formatter
    {
        public string Format
        {
            get { return _format; }
            set
            {
                if (_format == value)
                {
                    return;
                }
                _format = value;
                _sourceTree = new Parser(value).Parse();
            }
        }

        public Context BaseContext { get; set; }
        public Dictionary<string, string> Variables { get; private set; }

        private string _format;
        private IExpression[] _sourceTree;
        private Process _curProc;

        public LogicalFormatter()
            : this("${message}")
        {
        }

        public LogicalFormatter(string format)
        {
            Format = format;
            BaseContext = new Context();
            _curProc = Process.GetCurrentProcess();
            InitContext();
        }

        public void InitContext()
        {
            BaseContext.Functions.Add("fmtdate", new Func<DateTime, string, string>((o, s) => string.Format("{0:" + s + "}", o)));
            BaseContext.Functions.Add("fmtint", new Func<long, string, string>((o, s) => string.Format("{0:" + s + "}", o)));
            BaseContext.Functions.Add("fmtnum", new Func<double, string, string>((o, s) => string.Format("{0:" + s + "}", o)));
            BaseContext.Functions.Add("fmtsize", new Func<long, string>((bytes) =>
            {
                string sizeSuffix = "";
                sizeSuffix = "bytes";

                if (bytes > 1024)
                {
                    sizeSuffix = "KB";
                    bytes /= 1024;
                }
                if (bytes > 1024)
                {
                    sizeSuffix = "MB";
                    bytes /= 1024;
                }
                if (bytes > 1024)
                {
                    sizeSuffix = "GB";
                    bytes /= 1024;
                }
                if (bytes > 1024)
                {
                    sizeSuffix = "TB";
                    bytes /= 1024;
                }

                return string.Format("{0} {1}", bytes, sizeSuffix);
            }));
            BaseContext.Functions.Add("mem", new Func<string>(() => _curProc.WorkingSet64.ToString()));
            BaseContext.Functions.Add("cpu", new Func<string>(() => _curProc.TotalProcessorTime.ToString()));
            BaseContext.Functions.Add("proc", new Func<string>(() => _curProc.ProcessName));
            BaseContext.Functions.Add("procid", new Func<string>(() => _curProc.Id.ToString()));
            BaseContext.Functions.Add("machine", new Func<string>(() => Environment.MachineName));
            BaseContext.Functions.Add("getenv", new Func<string, string>(Environment.GetEnvironmentVariable));
            BaseContext.Functions.Add("setenv", new Func<string, string, string>((s1, s2) =>
            {
                Environment.SetEnvironmentVariable(s1, s2);
                return s2;
            }));
            BaseContext.Functions.Add("getvar", new Func<string, string>((s) => Variables[s]));
            BaseContext.Functions.Add("setvar", new Func<string, string, string>((s1, s2) =>
            {
                if (Variables.ContainsKey(s1))
                {
                    Variables[s1] = s2;
                }
                else
                {
                    Variables.Add(s1, s2);
                }
                return s2;
            }));
            BaseContext.Functions.Add("os", new Func<string>(() => RuntimeInformation.OSDescription));
            BaseContext.Functions.Add("arch", new Func<string>(() => RuntimeInformation.OSArchitecture.ToString()));
            BaseContext.Functions.Add("host", new Func<string>(System.Net.Dns.GetHostName));
            BaseContext.Functions.Add("when", new Func<bool, string, string>((b, s) =>
            {
                if (b)
                {
                    return s;
                }
                return "";
            }));
            BaseContext.Functions.Add("newline", new Func<string>(() => Environment.NewLine));
            BaseContext.Functions.Add("startswith", new Func<string, string, bool>((s1, s2) => s1.StartsWith(s2)));
            BaseContext.Functions.Add("endswith", new Func<string, string, bool>((s1, s2) => s1.EndsWith(s2)));
            BaseContext.Functions.Add("contains", new Func<string, string, bool>((s1, s2) => s1.Contains(s2)));
            BaseContext.Functions.Add("index", new Func<string, string, int>((s1, s2) => s1.IndexOf(s2)));
            BaseContext.Functions.Add("length", new Func<string, int>(s1 => s1.Length));
            BaseContext.Functions.Add("sub", new Func<string, int, int, string>((s1, index, length) => s1.Substring(index, length)));
            BaseContext.Functions.Add("right", new Func<string, int, string>((s1, index) => s1.Substring(index)));
            BaseContext.Functions.Add("left", new Func<string, int, string>((s1, index) => s1.Substring(0, index)));
            BaseContext.Functions.Add("mid", new Func<string, int, int, string>((s1, index, index2) => s1.Substring(index, index2 - index + 1)));
            BaseContext.Functions.Add("upper", new Func<string, string>(s1 => s1.ToUpper()));
            BaseContext.Functions.Add("lower", new Func<string, string>(s1 => s1.ToLower()));
            BaseContext.Functions.Add("hasvalue", new Func<string, bool>(s => !string.IsNullOrEmpty(s)));
            BaseContext.Functions.Add("trim", new Func<string, string>(s1 => s1.Trim()));
            BaseContext.Functions.Add("trimend", new Func<string, string>(s1 => s1.TrimEnd()));
            BaseContext.Functions.Add("trimstart", new Func<string, string>(s1 => s1.TrimStart()));
        }

        public override string Transform(Log log, string input, string tag, Dictionary<string, string> overrides)
        {
            var context = new MessageContext(BaseContext, log, tag, input);
            var evaluator = new Evaluator(_sourceTree);
            evaluator.Context = context;

            var result = evaluator.Eval();
            context.Dispose();
            return result;
        }
    }
}