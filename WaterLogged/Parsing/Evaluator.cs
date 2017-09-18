using System.Text;

namespace WaterLogged.Parsing
{
    /// <summary>
    /// Evaluates a series of expressions parsed from a format string.
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// The context to resolve functions from.
        /// </summary>
        public Context Context { get; set; }

        /// <summary>
        /// An array of <see cref="WaterLogged.Parsing.Expressions.IExpression"/> to evaluate.
        /// </summary>
        public Expressions.IExpression[] SourceTree { get; set; }

        /// <summary>
        /// Instantiates an instance of the Evaluator;
        /// parsing the specified source into the necessary
        /// <see cref="WaterLogged.Parsing.Expressions.IExpression"/> array.
        /// </summary>
        /// <param name="source">The source to evaluate.</param>
        public Evaluator(string source)
            : this(new Parser(source).Parse())
        {
            
        }

        /// <summary>
        /// Instantiates an instance of the Evaluator; using the specified
        /// <see cref="WaterLogged.Parsing.Expressions.IExpression"/> array.
        /// </summary>
        /// <param name="sourceTree">The source to evaluate.</param>
        public Evaluator(Expressions.IExpression[] sourceTree)
        {
            SourceTree = sourceTree;
        }

        /// <summary>
        /// Evaluates the the SourceTree into text.
        /// </summary>
        public string Eval()
        {
            var resultBuilder = new StringBuilder();

            foreach (var expressionNode in SourceTree)
            {
                resultBuilder.Append(expressionNode.Eval(this));
            }

            return resultBuilder.ToString();
        }
    }
}
