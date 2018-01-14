using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Filters;

namespace WaterLogged
{
    public class FilterManager
    {
        public List<IFilter> Filters { get; private set; }
        public List<ITemplatedMessageFilter> TemplatedFilters { get; private set; }

        public FilterManager()
        {
            Filters = new List<IFilter>();
            TemplatedFilters = new List<ITemplatedMessageFilter>();
        }

        public bool Validate(string message, string tag)
        {
            foreach (var filter in Filters)
            {
                if (!filter.Validate(message, tag))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ValidateTemplated(Templating.StructuredMessage message, string tag)
        {
            foreach (var filter in TemplatedFilters)
            {
                if (!filter.Validate(message, tag))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
