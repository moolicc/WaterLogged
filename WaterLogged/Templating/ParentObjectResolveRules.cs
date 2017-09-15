using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged.Templating
{
    [Flags]
    public enum ParentObjectResolveRules : byte
    {
        None = 0,
        PublicProperties = 1,
        PrivateProperties = 2,
        PublicFields = 4,
        PrivateFields = 8,
        ReadonlyProperties = 16,
        All = PublicProperties | PrivateProperties | PublicFields | PrivateFields | ReadonlyProperties,
    }
}
