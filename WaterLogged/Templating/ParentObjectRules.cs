using System;

namespace WaterLogged.Templating
{
    [Flags]
    public enum ParentObjectRules : byte
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
