using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Logic.Parsing.Expressions;
using WaterLogged.Logic.Parsing.Tokens;

namespace WaterLogged.Logic.Parsing
{
    public class Parser
    {
        public string Source { get; private set; }
        public Tokenizer Tokenizer { get; private set; }

        private Token[] _tokens;
        private int _tokenIndex;

        public Parser(string source)
        {
            Source = source;
            Tokenizer = new Tokenizer();
            _tokens = Tokenizer.EvaluateTokens(source);
            _tokenIndex = -1;
        }

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
                    return new TextExpression("");
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
            return new TextExpression(ParseLiteralUntil(typeof(CloseBraceToken)));
        }

        private LogicalExpression ParseLogical()
        {
            return null;
        }

        private InterpolateExpression ParseInterpolation()
        {
            return null;
        }
        

        private string ParseLiteralUntil(Type tokenType)
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
