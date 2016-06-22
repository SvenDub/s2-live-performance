using Inject;
using Live_Performance.Persistence.Memory;
using Live_Performance.Persistence.Tests.Entity;
using NUnit.Framework;

namespace Live_Performance.Persistence.Tests
{
    public class PersistenceInjectorTest
    {
        [OneTimeSetUp]
        public void SetUp() => PersistenceInjector.Inject(new MemoryRepositoryProvider());

        [OneTimeTearDown]
        public void TearDown() => Injector.Reset();

        [Test]
        public void TestInjection()
        {
            Assert.IsInstanceOf<RepositoryArmour<Apple>>(Injector.Resolve<IRepository<Apple>>());
            Assert.IsInstanceOf<RepositoryArmour<Banana>>(Injector.Resolve<IRepository<Banana>>());

            Assert.IsInstanceOf<MemoryRepository<Apple>>(Injector.Resolve<IStrictRepository<Apple>>());
            Assert.IsInstanceOf<MemoryRepository<Banana>>(Injector.Resolve<IStrictRepository<Banana>>());
        }
    }
}