using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterLogged.Parsing.Tokens;

namespace WaterLogged.Parsing
{
    /// <summary>
    /// Transforms an input string into an array of <see cref="WaterLogged.Parsing.Tokens.Token"/>.
    /// </summary>
    public class Tokenizer
    {
        private int _index;

        //$ means interpret as method
        //# means interpret as number (or bool)
        //% means interpret as string
        //Nothing means interpret as string

        /// <summary>
        /// Evalutes the specified input string into tokens.
        /// </summary>
        /// <param name="expression">The string to evaluate.</param>
        public Tokens.Token[] EvaluateTokens(string expression)
        {
            List<Token> tokens = new List<Token>();

            for (_index = 0; _index < expression.Length; _index++)
            {
                char curChar = expression[_index];
                char nextChar = '\0';
                if (_index + 1 < expression.Length)
                {
                    nextChar = expression[_index + 1];
                }
                if (curChar == '\\')
                {
                    if (nextChar == '{')
                    {
                        tokens.Add(new TextDataToken().Init(_index, "{"));
                    }
                    else if (nextChar == '}')
                    {
                        tokens.Add(new TextDataToken().Init(_index, "}"));
                    }
                    else if (nextChar == '#')
                    {
                        tokens.Add(new TextDataToken().Init(_index, "#"));
                    }
                    else if (nextChar == '$')
                    {
                        tokens.Add(new TextDataToken().Init(_index, "$"));
                    }
                    else if (nextChar == '%')
                    {
                        tokens.Add(new TextDataToken().Init(_index, "%"));
                    }
                    else if (nextChar == '\\')
                    {
                        tokens.Add(new TextDataToken().Init(_index, "\\"));
                    }
                    else
                    {
                        tokens.Add(new TextDataToken().Init(_index, "\\"));
                        continue;
                    }
                    _index++;
                    continue;
                }
                if (curChar == '{')
                {
                    tokens.Add(new OpenBraceToken().Init(_index, "{"));
                }
                else if (curChar == '}')
                {
                    tokens.Add(new CloseBraceToken().Init(_index, "}"));
                }
                else if (curChar == '#')
                {
                    tokens.Add(new LogicalToken().Init(_index, "#"));
                }
                else if (curChar == '$')
                {
                    tokens.Add(new InterpolateToken().Init(_index, "$"));
                }
                else if (curChar == '%')
                {
                    tokens.Add(new LiteralToken().Init(_index, "%"));
                }
                else if (curChar == ',')
                {
                    tokens.Add(new CommaToken().Init(_index, ","));
                }
                else if (curChar == ':')
                {
                    tokens.Add(new ColonToken().Init(_index, ":"));
                }
                else
                {
                    tokens.Add(new TextDataToken().Init(_index, ReadUntil(expression, '\n', '{', '}', '#', '$', '%', ',', ':')));
                }
            }
            return tokens.ToArray();
        }

        private string ReadUntil(string expression, params char[] targets)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = _index; i < expression.Length; i++)
            {
                char curChar = expression[i];
                char nextChar = '\0';
                if (_index + 1 < expression.Length)
                {
                    nextChar = expression[_index + 1];
                }

                if (curChar == '\\')
                {
                    if (nextChar == '{' || nextChar == '}' || nextChar == '#' || nextChar == '$' || nextChar == '%' || nextChar == '\\')
                    {
                        builder.Append(nextChar);
                        i++;
                        continue;
                    }
                }

                if (targets.Contains(curChar))
                {
                    _index = i - 1;
                    break;
                }
                builder.Append(curChar);
                _index++;
            }

            return builder.ToString();
        }
    }
}
