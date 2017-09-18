using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterLogged.Parsing.Expressions;
using WaterLogged.Parsing.Tokens;

namespace WaterLogged.Parsing
{
    /// <summary>
    /// Parses a format string into an <see cref="WaterLogged.Parsing.Expressions.IExpression"/> array.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// The format string to parse.
        /// </summary>
        public string Source { get; private set; }
        /// <summary>
        /// The tokenizer from which token will be... Tokenized...
        /// </summary>
        public Tokenizer Tokenizer { get; private set; }

        private Token[] _tokens;
        private int _tokenIndex;

        /// <summary>
        /// Instantiates a Parser and parses tokens from the specified source string.
        /// </summary>
        /// <param name="source"></param>
        public Parser(string source)
        {
            Source = source;
            Tokenizer = new Tokenizer();
            _tokens = Tokenizer.EvaluateTokens(source);
            _tokenIndex = -1;
        }

        /// <summary>
        /// Parsed the tokens into an array of <see cref="WaterLogged.Parsing.Expressions.IExpression"/>.
        /// </summary>
        public IExpression[] Parse()
        {
            List<IExpression> expressions = new List<IExpression>();

            IExpression value = ParseNext();
            while (value != null)
            {
                expressions.Add(value);
                value = ParseNext();
            }

            return expressions.ToArray();
        }

        private IExpression ParseNext()
        {
            var nextToken = ConsumeToken();

            switch (nextToken)
            {
                case EndOfTextToken _:
                    return null;
                case TextDataToken tok:
                    return new TextExpression(tok.Text);
                case LiteralToken _:
                    return ParseLiteral();
                case LogicalToken _:
                    return ParseLogical();
                case InterpolateToken _:
                    return ParseInterpolation();
                case CommaToken _:
                    return new TextExpression(",");
                case ColonToken _:
                    return new TextExpression(":");
                case OpenBraceToken _:
                    return new TextExpression("{");
                case CloseBraceToken _:
                    return new TextExpression("}");
                default:
                    return new TextExpression(ParseLiteralUntil(typeof(LiteralToken), typeof(LogicalToken), typeof(InterpolateToken), typeof(CommaToken)));
            }
        }

        private TextExpression ParseLiteral()
        {
            var nextToken = PeekToken();
            if (nextToken.GetType() != typeof(OpenBraceToken))
            {
                return new TextExpression("%");
            }
            ConsumeToken();
            return new TextExpression(ParseLiteralPast(typeof(CloseBraceToken)));
        }

        private IExpression ParseLogical()
        {
            var nextToken = PeekToken();
            if (nextToken.GetType() != typeof(OpenBraceToken))
            {
                return new TextExpression("#");
            }
            ConsumeToken();
            nextToken = PeekToken();
            if (nextToken.GetType() == typeof(CloseBraceToken))
            {
                throw new InvalidOperationException("End of logical expression was unexpected.");
            }
            //ConsumeToken();

            return new LogicalExpression(ParseInside(true, typeof(CloseBraceToken)));
        }

        private IExpression ParseInterpolation()
        {
            var nextToken = PeekToken();
            if (nextToken.GetType() != typeof(OpenBraceToken))
            {
                return new TextExpression("$");
            }
            ConsumeToken();
            nextToken = PeekToken();
            if (nextToken.GetType() != typeof(TextDataToken))
            {
                throw new InvalidOperationException("Function ID expected.");
            }
            var idToken = ConsumeToken();
            nextToken = PeekToken();
            if (nextToken.GetType() == typeof(CloseBraceToken))
            {
                ConsumeToken();
                return new InterpolateExpression(idToken.Text);
            }
            if (nextToken.GetType() != typeof(ColonToken))
            {
                throw new InvalidOperationException("Function parameter list or closing brace expected.");
            }
            ConsumeToken();
            var expression = new InterpolateExpression(idToken.Text);
            
            while (true)
            {
                var paramExpression = ParseInside(false, typeof(CloseBraceToken), typeof(CommaToken));
                nextToken = PeekToken();

                if (paramExpression == null)
                {
                    paramExpression = new TextExpression("");
                }

                expression.Parameters.Add(paramExpression);
                if (nextToken.GetType() == typeof(CloseBraceToken))
                {
                    break;
                }
                ConsumeToken();
            }
            ConsumeToken();
            return expression;
        }

        private IExpression ParseInside(bool consume, params Type[] closingTypes)
        {
            IExpression expression = null;
            var nextToken = PeekToken();
            var lastToken = nextToken;

            while (!closingTypes.Contains(nextToken.GetType()))
            {
                if (expression != null)
                {
                    expression = new ComboExpression(expression, ParseNext());
                }
                else
                {
                    expression = ParseNext();
                }
                lastToken = nextToken;
                nextToken = PeekToken();
                if (nextToken == lastToken || nextToken.Index == lastToken.Index)
                {
                    throw new InvalidOperationException("Token mismatch detected.");
                }
            }
            if (consume)
            {
                ConsumeToken();
            }
            return expression;
        }

        private string ParseLiteralPast(Type tokenType)
        {
            var result = new StringBuilder();
            var nextToken = ConsumeToken();
            while (nextToken.GetType() != tokenType)
            {
                result.Append(nextToken.Text);
                nextToken = ConsumeToken();
            }
            return result.ToString();
        }

        private string ParseLiteralUntil(params Type[] tokenTypes)
        {
            var result = new StringBuilder();
            var nextToken = PeekToken();
            while (!tokenTypes.Contains(nextToken.GetType()))
            {
                nextToken = ConsumeToken();
                result.Append(nextToken.Text);
                nextToken = PeekToken();
            }
            return result.ToString();
        }

        private Token PeekToken()
        {
            var tok = ConsumeToken();
            _tokenIndex--;
            return tok;
        }

        private Token ConsumeToken()
        {
            if (_tokenIndex + 1 >= _tokens.Length)
            {
                return new EndOfTextToken();
            }
            _tokenIndex++;
            return _tokens[_tokenIndex];
        }
    }
}
