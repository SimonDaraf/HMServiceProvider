using HMServiceProvider.Core;
using Xunit.Sdk;

namespace HMServiceProvider
{
    public class ServiceProviderTest : BeforeAfterTestAttribute
    {
        /// <summary>
        /// Simple dummy class used to tbe used as a dependent.
        /// </summary>
        private class TestDependent
        {
            public TestDependent()
            {
                
            }
        }

        /// <summary>
        /// Simple dummy class used to test the service provider.
        /// </summary>
        private class TestDummy
        {
            public TestDependent? testD = null;

            public int id = 0;

            public TestDummy()
            {
                
            }

            public TestDummy(TestDependent d)
            {
                testD = d;
            }
        }

        [Fact]
        public void Test_SimpleSingletonRegistration_WithZeroDependencies()
        {
            // Assemble
            ServiceProvider sp = new();
            sp.RegisterSingleton<TestDummy>();

            // Act
            TestDummy testDummy = sp.GetInstance<TestDummy>();

            // Assert
            Assert.NotNull(testDummy);
        }

        [Fact]
        public void Test_SimpleSingletonRegistration_WithDependency()
        {
            // Assemble
            ServiceProvider sp = new();
            sp.RegisterSingleton<TestDependent>();
            sp.RegisterSingleton<TestDummy>();

            // Act
            TestDummy testDummy = sp.GetInstance<TestDummy>();

            // Assert
            Assert.NotNull(testDummy);
            Assert.NotNull(testDummy.testD);
        }

        [Fact]
        public void Test_SimpleSingletonRegistration_AssertSingleton()
        {
            // Assemble
            ServiceProvider sp = new();
            sp.RegisterSingleton<TestDummy>();

            // Act
            TestDummy testDummy = sp.GetInstance<TestDummy>();
            Assert.NotNull(testDummy);
            testDummy.id = 1;
            TestDummy sameDummy = sp.GetInstance<TestDummy>();
            Assert.NotNull(sameDummy);

            // Assert
            Assert.Equal(testDummy.id, sameDummy.id);    
        }

        [Fact]
        public void Test_SimpleTransientRegistration_WithZeroDependencies()
        {
            // Assemble
            ServiceProvider sp = new();
            sp.RegisterTransient<TestDummy>();

            // Act
            TestDummy testDummy = sp.GetInstance<TestDummy>();

            // Assert
            Assert.NotNull(testDummy);
        }

        [Fact]
        public void Test_SimpleTransientRegistration_WithDependency()
        {
            // Assemble
            ServiceProvider sp = new();
            sp.RegisterTransient<TestDependent>();
            sp.RegisterTransient<TestDummy>();

            // Act
            TestDummy testDummy = sp.GetInstance<TestDummy>();

            // Assert
            Assert.NotNull(testDummy);
            Assert.NotNull(testDummy.testD);
        }

        [Fact]
        public void Test_SimpleTransientRegistration_AssertTransient()
        {
            // Assemble
            ServiceProvider sp = new();
            sp.RegisterTransient<TestDependent>();
            sp.RegisterTransient<TestDummy>();

            // Act
            TestDummy testDummy = sp.GetInstance<TestDummy>();
            Assert.NotNull(testDummy);
            testDummy.id = 1;
            TestDummy notSameDummy = sp.GetInstance<TestDummy>();
            Assert.NotNull(notSameDummy);

            // Assert
            Assert.NotEqual(testDummy.id, notSameDummy.id);
        }

        [Fact]
        public void Test_FactoryDelegateSingletonRegistration_AssertCreation()
        {
            // Assemble
            ServiceProvider sp = new();
            sp.RegisterSingleton(serviceProvider => new TestDummy()
            {
                id = 5
            });

            // Act
            TestDummy testDummy = sp.GetInstance<TestDummy>();

            // Assert
            Assert.NotNull(testDummy);
            Assert.Equal(5, testDummy.id);
        }

        [Fact]
        public void Test_FactoryDelegateTransientRegistration_AssertCreation()
        {
            // Assemble
            ServiceProvider sp = new();
            sp.RegisterTransient(serviceProvider => new TestDummy()
            {
                id = 5
            });

            // Act
            TestDummy testDummy = sp.GetInstance<TestDummy>();

            // Assert
            Assert.NotNull(testDummy);
            Assert.Equal(5, testDummy.id);
        }
    }
}