using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Parsing.Templater.Tokens;

namespace WaterLogged.Parsing.Templater
{
    public class Tokenizer
    {
        public static List<Token> Parse(string template)
        {
            var tokenizer = new Tokenizer(template);
            return tokenizer.Tokenize();
        }

        private string _source;
        private int _index;

        private Tokenizer(string source)
        {
            _source = source;
            _index = 0;
        }

        private List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            var token = ParseNext();
            while (token != null)
            {
                tokens.Add(token);
                token = ParseNext();
            }
            return tokens;
        }

        private Token ParseNext()
        {
            int i = _index;
            char curChar = CurrentChar();
            if (curChar == '\0')
            {
                return null;
            }
            char nextChar = Peek();
            if (curChar == '{')
            {
                if (nextChar != '{')
                {
                    return ParseProperty().SetIndex(i);
                }
            }
            return ParseLiteral().SetIndex(i);
        }

        private LiteralToken ParseLiteral()
        {
            StringBuilder literalBuilder = new StringBuilder();

            int i;
            for (i = _index; i < _source.Length; i++)
            {
                char curChar = Pull();
                if (curChar == '\0')
                {
                    break;
                }
                if (curChar == '{')
                {
                    if (Peek() == '{')
                    {
                        literalBuilder.Append('{');
                        Pull();
                        continue;
                    }
                    break;
                }
                literalBuilder.Append(curChar);
            }

            _index = i;
            return new LiteralToken().SetText(literalBuilder.ToString());
        }

        private PropertyHoleToken ParseProperty()
        {
            StringBuilder propertyBuilder = new StringBuilder();
            StringBuilder argBuilder = new StringBuilder();
            PropertyHoleToken token = new PropertyHoleToken();

            bool parsingArg = false;

            bool breakLoop = false;
            int i = _index;

            while (i < _source.Length && !breakLoop)
            {
                char curChar = Pull();
                switch (curChar)
                {
                    case '\0':
                    case '}':
                        breakLoop = true;
                        break;
                    case '@':
                        token = new StringifiedPropertyHoleToken();
                        break;
                    case '$':
                        token = new SerializePropertyHoleToken();
                        break;
                    case ':':
                        parsingArg = true;
                        break;
                    default:
                        if (!parsingArg)
                        {
                            propertyBuilder.Append(curChar);
                        }
                        else
                        {
                            argBuilder.Append(curChar);
                        }
                        break;
                }
                i++;
            }

            _index = i;
            return token.SetNameAndArg(propertyBuilder.ToString(), argBuilder.ToString());
        }

        private char CurrentChar()
        {
            if (_index < _source.Length)
            {
                return _source[_index];
            }
            return '\0';
        }

        private char Pull()
        {
            _index++;
            if (_index < _source.Length)
            {
                return _source[_index - 1];
            }
            return '\0';
        }

        private char Peek()
        {
            if (_index + 1 < _source.Length)
            {
                return _source[_index + 1];
            }
            return '\0';
        }
    }
}
