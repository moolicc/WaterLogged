using System;

namespace WaterLogged.Templating
{
    /// <summary>
    /// Represents a set of rules the template parser follows when resolving values from a parent object.
    /// </summary>
    [Flags]
    public enum ParentObjectRules : byte
    {
        /// <summary>
        /// No rules specified.
        /// </summary>
        None = 0,
        /// <summary>
        /// Public properties should be included.
        /// </summary>
        PublicProperties = 1,
        /// <summary>
        /// Private properties should be included.
        /// </summary>
        PrivateProperties = 2,
        /// <summary>
        /// Public fields should be included.
        /// </summary>
        PublicFields = 4,
        /// <summary>
        /// Private fields should be included.
        /// </summary>
        PrivateFields = 8,
        /// <summary>
        /// Readonly properties should be included.
        /// </summary>
        ReadonlyProperties = 16,
        /// <summary>
        /// All properties and fields should be included.
        /// </summary>
        All = PublicProperties | PrivateProperties | PublicFields | PrivateFields | ReadonlyProperties,
    }
}
