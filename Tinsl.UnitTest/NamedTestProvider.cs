using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodePraxis.Tinsl.UnitTest
{
    public interface INamedTestProvider
    {
        string Setting1
        {
            get;
            set;
        }
    }

    public class NamedTestProvider : INamedTestProvider
    {
        public string Setting1
        {
            get;
            set;
        }
    }
}
