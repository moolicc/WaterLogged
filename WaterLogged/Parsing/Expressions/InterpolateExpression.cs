using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WaterLogged.Parsing.Expressions
{
    /// <summary>
    /// Implements an expression which evaluates function calls in the form '${funcname:param1,param2}'.
    /// </summary>
    public class InterpolateExpression : IExpression
    {
        /// <summary>
        /// The function name.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// A list of expressions to evaluate as the function's parameters.
        /// </summary>
        public List<IExpression> Parameters { get; private set; }

        public InterpolateExpression(string id)
        {
            Id = id;
            Parameters = new List<IExpression>();
        }

        /// <inheritdoc />
        public string Eval(Evaluator evaluator)
        {
            var function = evaluator.Context.GetDelegate(Id);
            var funcParmaters = function.GetMethodInfo().GetParameters();

            if (funcParmaters.Length != Parameters.Count)
            {
                throw new MethodAccessException("Parameter count mismatch.");
            }

            var paramValueStrings = Parameters.Select(p => p.Eval(evaluator)).ToArray();
            var paramValues = new List<object>();

            for (int i = 0; i < funcParmaters.Length; i++)
            {
                var funcParam = funcParmaters[i];
                var stringParam = paramValueStrings[i];

                if (funcParam.ParameterType == typeof(string))
                {
                    paramValues.Add(stringParam);
                }
                else
                {
                    paramValues.Add(Serialization.StringConversion.Converter.Convert(stringParam, funcParam.ParameterType));
                }
            }

            return function.DynamicInvoke(paramValues.ToArray()).ToString();
        }
    }
}
