using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterLogged.Parsing.Templater;
using WaterLogged.Parsing.Templater.Tokens;

namespace WaterLogged.Templating
{
    public struct Template
    {
        private static Dictionary<string, Template> _templateCache;

        static Template()
        {
            _templateCache = new Dictionary<string, Template>();
        }

        public static void ClearTemplateCache()
        {
            _templateCache.Clear();
        }

        public static int GetTemplateCacheSize()
        {
            return _templateCache.Count;
        }

        public static Template FromTemplateCache(string templateSource)
        {
            if (_templateCache.ContainsKey(templateSource))
            {
                return _templateCache[templateSource];
            }
            else
            {
                var template = new Template(Tokenizer.Parse(templateSource));
                _templateCache.Add(templateSource, template);
                return template;
            }
        }

        public bool NamedProperties { get; private set; }

        private Token[] _templateTokens;

        public Template(IEnumerable<Token> tokens)
        {
            NamedProperties = false;

            var enumerable = tokens as Token[] ?? tokens.ToArray();
            _templateTokens = enumerable.ToArray();

            if (enumerable.FirstOrDefault(t => t is PropertyHoleToken) is PropertyHoleToken token)
            {
                if (!int.TryParse(token.PropertyName, out _))
                {
                    NamedProperties = true;
                }
            }
        }

        public Token[] GetTokens()
        {
            return _templateTokens;
        }

        public string BuildSource()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var token in _templateTokens)
            {
                builder.Append(token.BuildString());
            }
            return builder.ToString();
        }
    }
}
