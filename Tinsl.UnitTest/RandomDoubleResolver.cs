using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodePraxis.Tinsl.UnitTest
{

    public class RandomDoubleResolver : IResolver<TestType>
    {
        Random _rand = new Random();

        public R Resolve<R>(object identifier = null) where R : TestType
        {
            TestType testType = new TestType();
            testType.Double = _rand.NextDouble();
            return (R)testType;
        }
    }

    public class TestType
    {
        public Double Double { get; set; }
    }
}
