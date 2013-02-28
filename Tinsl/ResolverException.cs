using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePraxis.Tinsl
{
    public class ResolverException : Exception
    {
        public ResolverException(string message, Type type, object args = null) : base(message)
        {
        }
    }
}
