using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePraxis.Tinsl.Specialized
{
    public class ChainedProviderResolver<T> : IBindingResolver<T>
    {
        ChainedResolver<T> chainedResolver = null;
        ProviderResolver<IResolver<T>> providerResolver = null;

        public string Front
        {
            get;
            set;
        }
        public string Back
        {
            get;
            set;
        }
        public string SectionName
        {
            get;
            set;
        }

        private ChainedResolver<T> ChainedResolver
        {
            get
            {
                if (chainedResolver == null)
                {
                    chainedResolver = new ChainedResolver<T>(
                        ProviderResolver.Resolve<IResolver<T>>(Front),
                        ProviderResolver.Resolve<IResolver<T>>(Back));
                }
                return chainedResolver;
            }
        }

        private ProviderResolver<IResolver<T>> ProviderResolver
        {
            get
            {
                if (providerResolver == null)
                {
                    providerResolver = new ProviderResolver<IResolver<T>>(SectionName);
                }
                return providerResolver;
            }
        }


        public void Bind(T instance, object identifier = null)
        {
            ChainedResolver.Bind(instance, identifier);
        }

        public R Resolve<R>(object identifier = null) where R : T
        {
            return ChainedResolver.Resolve<R>(identifier);
        }
    }
}
