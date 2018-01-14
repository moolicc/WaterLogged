using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Filters
{
    public interface IFilter
    {
        bool Validate(string message, string tag);
    }
}
