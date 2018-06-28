using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    /// <summary>
    /// Base type for ObjectTransformers.
    /// </summary>
    public abstract class ObjectTransformer
    {
        private static Dictionary<Type, ObjectTransformer> _transformers;

        static ObjectTransformer()
        {
            _transformers = new Dictionary<Type, ObjectTransformer>();
        }

        /// <summary>
        /// Returns an <see cref="ObjectTransformer"/> for the specified <see cref="Type"/>
        /// </summary>
        /// <param name="targetType">The type to search for.</param>
        /// <exception cref="KeyNotFoundException"></exception>
        public static ObjectTransformer GetTransformer(Type targetType)
        {
            return _transformers[targetType];
        }

        /// <summary>
        /// Returns a value indicating if there is a <see cref="ObjectTransformer"/> for the specified <see cref="Type"/>
        /// </summary>
        /// <param name="targetType">The type to search for.</param>
        public static bool ContainsTransformer(Type targetType)
        {
            return _transformers.ContainsKey(targetType);
        }

        /// <summary>
        /// Adds a <see cref="ObjectTransformer"/> to a list of known transformers.
        /// </summary>
        /// <param name="transformer">The transformer to add.</param>
        public static void SetTransformer(ObjectTransformer transformer)
        {
            if (_transformers.ContainsKey(transformer.TargetType))
            {
                _transformers.Remove(transformer.TargetType);
            }
            _transformers.Add(transformer.TargetType, transformer);
        }

        /// <summary>
        /// Removes a <see cref="ObjectTransformer"/> from the list of known transformers.
        /// </summary>
        /// <param name="targetType">The type to remove a transformer for.</param>
        public static void RemoveTransformer(Type targetType)
        {
            if (_transformers.ContainsKey(targetType))
            {
                _transformers.Remove(targetType);
            }
        }

        /// <summary>
        /// When overridden in a derived class; returns the <see cref="Type"/> the transformer works on.
        /// </summary>
        public abstract Type TargetType { get; }

        /// <summary>
        /// When overridden in a derived class; converts the specified value into a friendly string representation.
        /// </summary>
        /// <param name="value">The input object to transform.</param>
        /// <param name="argument">An optional argument to pass to the transformer.</param>
        public abstract string Transform(object value, object argument);
    }
}