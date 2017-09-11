using System;
using System.Reflection;
using System.Text;
using WaterLogged;
using WaterLogged.Supplement;

namespace Example.Examples
{
    class Example4 : ExampleBase
    {
        public override string IntroText => "This example demonstrates logical log usage. It echoes back input but takes advantage of logic-based formatting.";
        public override string Name => "Logical formatting usage";

        private bool _flag;

        public Example4()
        {
            var listener = new WaterLogged.Listeners.StandardOut();
            _log.AddListener(listener);
        }

        public override void Selected()
        {
            var formatter = new LogicalFormatter("${newline}${fmtdate:${datetime},d}${newline}${message}");
            _log.Formatter = formatter;

            Console.WriteLine();
            Console.WriteLine("The syntax is simple");
            Console.WriteLine(
                "%{text} - Just a string literal. Nothing special is pulled out of that except for text.");
            Console.WriteLine(
                "#{expression} - Evaluates an expression using ncalc. Boolean and mathematical expressions are resolved from these.");
            Console.WriteLine(
                "${func:paramlist} - Invokes a function with the specified parameters. Parameters are separated with commas, naturally.");
            Console.WriteLine(
                "You can use any combination of these to create your format string. You can even nest them within each-other like so: \"#{1 + ${getnumber}}\".");
            Console.WriteLine("Here's a list of available functions:");
            foreach (var funcKeyValue in formatter.BaseContext.Functions)
            {
                StringBuilder paramListBuilder = new StringBuilder();
                foreach (var paramInfo in funcKeyValue.Value.GetMethodInfo().GetParameters())
                {
                    paramListBuilder.Append(paramInfo.ParameterType);
                    paramListBuilder.Append(",");
                }
                Console.WriteLine("${" + funcKeyValue.Key + ":" + paramListBuilder.ToString().TrimEnd(',') + "}");
            }
            var tempContext = new MessageContext(formatter.BaseContext, _log, "", "");
            foreach (var funcKeyValue in tempContext.Functions)
            {
                StringBuilder paramListBuilder = new StringBuilder();
                foreach (var paramInfo in funcKeyValue.Value.GetMethodInfo().GetParameters())
                {
                    paramListBuilder.Append(paramInfo.ParameterType);
                    paramListBuilder.Append(",");
                }
                Console.WriteLine("${" + funcKeyValue.Key + ":" + paramListBuilder.ToString().TrimEnd(',') + "}");
            }

            Console.WriteLine();
            Console.WriteLine("Enter format (or press enter to use the default; \"${newline}${fmtdate:${datetime},d}${newline}${message}\"):");
            string format = Console.ReadLine().Trim();
            if (!string.IsNullOrWhiteSpace(format))
            {
                formatter.Format = format;
            }
        }

        public override void Echo(string text)
        {
            try
            {
                _log.WriteLine(text);
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input or format:\n{0}", e);
                throw;
            }
        }
    }
}
