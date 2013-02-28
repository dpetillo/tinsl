using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodePraxis.Tinsl.UnitTest
{
    abstract class PipelineResolver : IResolver<IPipelineableObject>
    {
		public virtual R Resolve<R> (object identifier)
		{
			throw new System.NotImplementedException ();
		}
        //public abstract R Resolve<R>(object identifier = null) where R : IPipelineableObject;
    }

    class PipelineResolver1 : PipelineResolver
    {
        public override R Resolve<R>(object identifier = null)
        {
			throw new System.NotImplementedException ();
        }
    }

    class PipelineResolver2 : PipelineResolver
    {
        public override R Resolve<R>(object identifier = null)
        {
			throw new System.NotImplementedException ();
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
