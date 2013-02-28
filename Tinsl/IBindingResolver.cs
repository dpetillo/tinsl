using System;
namespace CodePraxis.Tinsl
{
    public interface IBindingResolver<T> : IResolver<T>
    {
        void Bind(T instance, object identifier = null);
    }
}
