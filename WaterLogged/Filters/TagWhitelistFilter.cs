using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Filters
{
    public class TagWhitelistFilter : IFilter, ITemplatedMessageFilter
    {
        public List<string> Whitelist { get; private set; }

        public TagWhitelistFilter(params string[] whitelist)
        {
           Whitelist = new List<string>();
           Whitelist.AddRange(whitelist);
        }

        public bool Validate(string message, string tag)
        {
            if (Whitelist.Contains(tag))
            {
                return true;
            }
            return false;
        }

        public bool Validate(StructuredMessage message, string tag)
        {
            if (Whitelist.Contains(tag))
            {
                return true;
            }
            return false;
        }
    }
}
