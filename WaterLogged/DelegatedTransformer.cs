using System;
using System.Collections.Generic;
using System.Text;

namespace WaterLogged
{
    public class DelegatedTransformer : ObjectTransformer
    {
        public Func<object, string> Transformer { get; private set; }
        public override Type TargetType
        {
            get { return _targetType; }
        }

        private Type _targetType;
        

        public DelegatedTransformer()
            : this(null, null)
        {
            
        }

        public DelegatedTransformer(Type targetType, Func<object, string> transformer)
        {
            _targetType = targetType;
            Transformer = transformer;
        }

        public DelegatedTransformer SetType(Type targetType)
        {
            _targetType = targetType;
            return this;
        }

        public DelegatedTransformer SetTransformer(Func<object, string> transformer)
        {
            Transformer = transformer;
            return this;
        }

        public override string Transform(object value, object argument)
        {
            if (Transformer == null)
            {
                throw new InvalidOperationException("Transformer property not set.");
            }
            return Transformer(argument);
        }
    }
}
