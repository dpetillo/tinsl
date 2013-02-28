using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePraxis.Tinsl.UnitTest
{
    abstract class PipelineResolver : IResolver<IPipelineableObject>
    {
        public abstract R Resolve<R>(object identifier = null) where R : IPipelineableObject;
    }

    class PipelineResolver1 : PipelineResolver
    {
        public override R Resolve<R>(object identifier = null)
        {

        }
    }

    class PipelineResolver2 : PipelineResolver
    {
        public override R Resolve<R>(object identifier = null)
        {
        }
    }


    interface IPipelineableObject
    {
        string Data
        {
            get;
            set;
        }
    }
}
