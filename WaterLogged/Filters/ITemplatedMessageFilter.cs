using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Filters
{
    public interface ITemplatedMessageFilter
    {
        bool Validate(Templating.StructuredMessage message, string tag);
    }
}
