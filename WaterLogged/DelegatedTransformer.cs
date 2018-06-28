using System;
using System.Collections.Generic;
using System.Text;
using WaterLogged.Output;

namespace WaterLogged
{
    /// <summary>
    /// Implements an <see cref="ObjectTransformer"/> which delegates transformation to a <see cref="Func{T1, TResult}"/>.
    /// </summary>
    public class DelegatedTransformer : ObjectTransformer
    {
        /// <summary>
        /// The <see cref="Func{T1, TResult}"/> which will actually handle transformation.
        /// </summary>
        public Func<object, string> Transformer { get; private set; }
        
        /// <inheritdoc />
        public override Type TargetType
        {
            get { return _targetType; }
        }

        private Type _targetType;
        

        public DelegatedTransformer()
            : this(null, null)
        {
            
        }

        /// <summary>
        /// Instantiates an instance of <see cref="DelegatedTransformer"/> with a target type and transformation Func.
        /// </summary>
        /// <param name="targetType">The type which the transformation function supports.</param>
        /// <param name="transformer">The <see cref="Func{T1, TResult}"/> to delegate transformations to.</param>
        public DelegatedTransformer(Type targetType, Func<object, string> transformer)
        {
            _targetType = targetType;
            Transformer = transformer;
        }

        /// <summary>
        /// Sets the target type this <see cref="DelegatedTransformer"/> works on.
        /// </summary>
        /// <param name="targetType">The type.</param>
        public DelegatedTransformer SetType(Type targetType)
        {
            _targetType = targetType;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="Func{T1, TResult}"/> this <see cref="DelegatedTransformer"/> will delegate transformation to.
        /// </summary>
        /// <param name="transformer">The transformation function.</param>
        public DelegatedTransformer SetTransformer(Func<object, string> transformer)
        {
            Transformer = transformer;
            return this;
        }
        
        /// <inheritdoc />
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
