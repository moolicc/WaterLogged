using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Templating;

namespace WaterLogged.Filters
{
    public delegate bool FilterPredicate(string message, string tag);
    public delegate bool TemplatedFilterPredicate(StructuredMessage message, string tag);

    public class DelegatedFilter : IFilter, ITemplatedMessageFilter
    {
        public FilterPredicate MessageValidator { get; set; }
        public TemplatedFilterPredicate TemplatedMessageValidator { get; set; }

        public DelegatedFilter(FilterPredicate messageValidator)
            : this(messageValidator, null)
        {

        }

        public DelegatedFilter(TemplatedFilterPredicate templatedMessageValidator)
            : this(null, templatedMessageValidator)
        {

        }

        public DelegatedFilter(FilterPredicate messageValidator, TemplatedFilterPredicate templatedMessageValidator)
        {
            MessageValidator = messageValidator;
            TemplatedMessageValidator = templatedMessageValidator;
        }

        public bool Validate(string message, string tag)
        {
            if (MessageValidator != null)
            {
                return MessageValidator(message, tag);
            }
            return true;
        }

        public bool Validate(StructuredMessage message, string tag)
        {
            if (TemplatedMessageValidator != null)
            {
                return TemplatedMessageValidator(message, tag);
            }
            return true;
        }
    }
}
