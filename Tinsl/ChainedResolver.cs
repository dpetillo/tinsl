using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePraxis.Tinsl
{
    public class ChainedResolver<T> : IBindingResolver<T>
    {
        readonly IResolver<T> _frontResolver;
        readonly IResolver<T> _backResolver;

        public ChainedResolver(IResolver<T> frontResolver, IResolver<T> backResolver)
        {
            _frontResolver = frontResolver;
            _backResolver = backResolver;
        }

        public R Resolve<R>(object identifier = null) where R : T
        {
            R instance = _frontResolver.Resolve<R>(identifier);

            if (instance == null)
            {
                instance = _backResolver.Resolve<R>(identifier);

                if (instance != null && _frontResolver is IBindingResolver<T>)
                {
                    ((IBindingResolver<T>)_frontResolver).Bind(instance, identifier);
                }
            }
            
            return instance;
        }

        public void Bind(T instance, object identifier = null)
        {
            if (_frontResolver is IBindingResolver<T>)
            {
                ((IBindingResolver<T>)_frontResolver).Bind(instance, identifier);
            }
            else if (_backResolver is IBindingResolver<T>)
            {
                ((IBindingResolver<T>)_backResolver).Bind(instance, identifier);
            }
        }
    }

}
