using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodePraxis.Tinsl.UnitTest
{
    using System.Collections.Generic;
    using CodePraxis.Tinsl;
    using CodePraxis.Tinsl.Specialized;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BasicFactoryTest()
        {
            IBindingResolver<object> resolver = new BasicResolver<object>();
            resolver.Bind(new RandomDoubleResolver());
            double e = resolver.Resolve<IResolver<TestType>>().Resolve<TestType>().Double;
        }

        [TestMethod]
        public void TwoSingletonBindTest()
        {
            IBindingResolver<object> resolver = new BasicResolver<object>();
            var newInstance = new RandomDoubleResolver();

            resolver.Bind(new RandomDoubleResolver());
            resolver.Bind(newInstance);

            var resolvedInstance = resolver.Resolve<IResolver<TestType>>();

            Assert.AreSame(newInstance, resolvedInstance);
        }

        [TestMethod]
        public void TwoNamedInstanceBindTest()
        {
            IBindingResolver<object> resolver = new BasicResolver<object>();
            var newInstance = new RandomDoubleResolver();

            Guid guid = Guid.NewGuid();

            resolver.Bind(new RandomDoubleResolver(), guid);
            resolver.Bind(newInstance, guid);

            var resolvedInstance = resolver.Resolve<IResolver<TestType>>(guid);

            Assert.AreSame(newInstance, resolvedInstance);
        }

        [TestMethod]
        public void ProviderSettingTest()
        {
            IResolver<object> resolver = new ProviderResolver<object>("providerConfiguration");

            TestProvider instance = resolver.Resolve<TestProvider>();
            Assert.IsTrue(instance.Setting1 == "setting1value");
        }

        [TestMethod]
        public void ProviderPolymorphismTest()
        {
            IResolver<object> resolver = new ProviderResolver<object>("providerConfiguration");
            ITestProvider instance = resolver.Resolve<ITestProvider>();
            Assert.IsNotNull(instance);
        }


        [TestMethod]
        public void ChainedTest()
        {
            IResolver<object> chainedResolver = new ChainedResolver<object>(
                    new BasicResolver<object>(),
                    new ProviderResolver<object>("providerConfiguration"));

            ITestProvider instance = chainedResolver.Resolve<ITestProvider>();
            Assert.IsNotNull(instance);
        }

        //Test that in a two resolver resolution, we front resolver has 
        //binded the back resolver's instance
        [TestMethod]
        public void ChainedSingletonBindTest()
        {
            var br = new BasicResolver<object>();

            IBindingResolver<object> chainedResolver = new ChainedResolver<object>(
                    br,
                    new ProviderResolver<object>("providerConfiguration"));

            ITestProvider instance = chainedResolver.Resolve<ITestProvider>();
            Assert.IsNotNull(instance);

            ITestProvider instance2 = br.Resolve<ITestProvider>();
            Assert.AreSame(instance, instance2);
        }


        //Test that in a two resolver resolution, we front resolver has 
        //binded the back resolver's instance
        [TestMethod]
        public void ChainedNamedInstanceBindTest()
        {
            var br = new BasicResolver<object>();

            IBindingResolver<object> chainedResolver = new ChainedResolver<object>(
                    br,
                    new ProviderResolver<object>("providerConfiguration"));

            ITestProvider instance = chainedResolver.Resolve<ITestProvider>("test1");
            Assert.IsNotNull(instance);

            ITestProvider instance2 = br.Resolve<ITestProvider>("test1");
            Assert.AreSame(instance, instance2);
        }


        [TestMethod]
        [ExpectedException(typeof(ResolverException))]
        public void TooManyProviders()
        {
            INamedTestProvider instance = new ProviderResolver<object>("providerConfiguration").Resolve<INamedTestProvider>();

        }

        [TestMethod]
        public void DoubleBind()
        {
            IBindingResolver<object> br = new BasicResolver<object>();

            br.Bind(new DerivedTestProvider());

            var secondBind = new TestProvider();
            br.Bind(secondBind);

            ITestProvider resolved = br.Resolve<ITestProvider>();

            Assert.AreSame(secondBind, resolved);
        }

        [TestMethod]
        public void ChainedProviderResolverSetup()
        {
            IResolver<object> resolver = new ProviderResolver<object>("resolverConfiguration");

            //chained provider will own an instance of a basic on the front end
            ChainedProviderResolver<object> chainProvider1 = resolver.Resolve<ChainedProviderResolver<object>>("chainedprovider1");

            Assert.AreEqual(chainProvider1.Front, "basic1");
            Assert.AreEqual(chainProvider1.Back, "provider1");
            Assert.AreEqual(chainProvider1.SectionName, "resolverConfiguration");
        }


        [TestMethod]
        public void ChainedProviderResolverDoubleResolve()
        {
            IResolver<object> resolver = new ProviderResolver<object>("resolverConfiguration");

            //chained provider will own an instance of a basic on the front end
            ChainedProviderResolver<object> chainProvider1 = resolver.Resolve<ChainedProviderResolver<object>>("chainedprovider1");

            var resolve1 = chainProvider1.Resolve<ITestProvider>();
            var resolve2 = chainProvider1.Resolve<ITestProvider>();

            Assert.AreSame(resolve1, resolve2);
        }


        [TestMethod]
        public void PipeliningObjectCreation()
        {
            

        }
    }
}
