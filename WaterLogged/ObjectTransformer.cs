using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public abstract class ObjectTransformer
    {
        private static Dictionary<Type, ObjectTransformer> _transformers;

        static ObjectTransformer()
        {
            _transformers = new Dictionary<Type, ObjectTransformer>();
        }

        public static ObjectTransformer GetTransformer(Type targetType)
        {
            return _transformers[targetType];
        }

        public static bool ContainsTransformer(Type targetType)
        {
            return _transformers.ContainsKey(targetType);
        }

        public static void SetTransformer(ObjectTransformer transformer)
        {
            if (_transformers.ContainsKey(transformer.TargetType))
            {
                _transformers.Remove(transformer.TargetType);
            }
            _transformers.Add(transformer.TargetType, transformer);
        }

        public static void RemoveTransformer(Type targetType)
        {
            if (_transformers.ContainsKey(targetType))
            {
                _transformers.Remove(targetType);
            }
        }

        public abstract Type TargetType { get; }
        public abstract string Transform(object value, object argument);
    }
}