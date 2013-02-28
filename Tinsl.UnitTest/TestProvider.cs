using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePraxis.Tinsl.UnitTest
{
    public interface ITestProvider
    {
        string Setting1
        {
            get;
            set;
        }
    }

    public class TestProvider : ITestProvider
    {
        public string Setting1
        {
            get;
            set;
        }
    }

    public class DerivedTestProvider : TestProvider { } 
}
