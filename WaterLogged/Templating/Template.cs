using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterLogged.Parsing.Templater;
using WaterLogged.Parsing.Templater.Tokens;

namespace WaterLogged.Templating
{
    /// <summary>
    /// Represents a parsed Template.
    /// </summary>
    public struct Template
    {
        private static Dictionary<string, Template> _templateCache;

        static Template()
        {
            _templateCache = new Dictionary<string, Template>();
        }

        /// <summary>
        /// Clears the cache of parsed templates.
        /// </summary>
        public static void ClearTemplateCache()
        {
            _templateCache.Clear();
        }

        /// <summary>
        /// Returns the number of items in the template cache.
        /// </summary>
        /// <returns></returns>
        public static int GetTemplateCacheSize()
        {
            return _templateCache.Count;
        }

        /// <summary>
        /// Returns a parsed representation of the specified template source.
        /// If the source is cached, the result is pulled from the cache.
        /// Otherwise, parses the source and caches the result.
        /// </summary>
        /// <param name="templateSource">The source of the template.</param>
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

        /// <summary>
        /// Gets a value indiciated if properties are named or indexed.
        /// </summary>
        public bool NamedProperties { get; private set; }

        private Token[] _templateTokens;

        /// <summary>
        /// Instantiates a new Template instance using the specified tokens.
        /// </summary>
        /// <param name="tokens"></param>
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

        /// <summary>
        /// Returns the tokens this Template represents.
        /// </summary>
        /// <returns></returns>
        public Token[] GetTokens()
        {
            return _templateTokens;
        }

        /// <summary>
        /// Rebuilds the source of this template and returns the results.
        /// </summary>
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
