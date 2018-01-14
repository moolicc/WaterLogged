using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Filters
{
    public class TagBlacklistFilter : IFilter, ITemplatedMessageFilter
    {
        public List<string> Blacklist { get; private set; }

        public TagBlacklistFilter(params string[] blacklist)
        {
            Blacklist = new List<string>();
            Blacklist.AddRange(blacklist);
        }

        public bool Validate(string message, string tag)
        {
            if (Blacklist.Contains(tag))
            {
                return false;
            }
            return true;
        }

        public bool Validate(StructuredMessage message, string tag)
        {
            if (Blacklist.Contains(tag))
            {
                return false;
            }
            return true;
        }
    }
}
