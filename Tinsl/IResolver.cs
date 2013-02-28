using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodePraxis.Tinsl
{
    public interface IResolver<T> 
    {
        R Resolve<R>(object identifier = null) where R : T;
    }
}
