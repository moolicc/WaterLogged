using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterLogged.Parsing.Templater;
using WaterLogged.Parsing.Templater.Tokens;

namespace WaterLogged.Templating
{
    public class Template
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


        private Token[] _templateTokens;

        public Template(IEnumerable<Token> tokens)
        {
            _templateTokens = tokens.ToArray();
        }

        public Token[] GetTokens()
        {
            return _templateTokens;
        }
    }
}
