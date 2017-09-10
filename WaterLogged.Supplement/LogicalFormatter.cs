using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using WaterLogged.Parsing;
using WaterLogged.Parsing.Expressions;

namespace WaterLogged.Supplement
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
