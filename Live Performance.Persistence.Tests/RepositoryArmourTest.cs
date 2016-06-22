using System;
using System.Collections.Generic;
using Inject;
using Live_Performance.Persistence.Memory;
using Live_Performance.Persistence.Tests.Entity;
using NUnit.Framework;

namespace Live_Performance.Persistence.Tests
{
    public class RepositoryArmourTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp() => PersistenceInjector.Inject(new MemoryRepositoryProvider());

        [OneTimeTearDown]
        public void OneTimeTearDown() => Injector.Reset();

        [SetUp]
        public void SetUp()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();
            repository.Save(new Apple {Type = "Goudreinette"});
        }

        [TearDown]
        public void TearDown()
        {
            IRepository<Apple> appleRepository = Injector.Resolve<IRepository<Apple>>();
            IRepository<Banana> bananaRepository = Injector.Resolve<IRepository<Banana>>();

            appleRepository.DeleteAll();
            bananaRepository.DeleteAll();
        }

        [Test]
        public void TestCount()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.AreEqual(1, repository.Count());

            repository.Save(new Apple {Type = "Elstar"});

            Assert.AreEqual(2, repository.Count());
        }

        [Test]
        public void TestDelete()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();
            repository.Save(new Apple {Type = "Elstar"});

            Assert.AreEqual(2, repository.Count());

            repository.Delete(1);

            Assert.AreEqual(1, repository.Count());
            Assert.AreEqual(2, repository.FindAll()[0].Id);
            Assert.AreEqual("Elstar", repository.FindAll()[0].Type);
        }

        [Test]
        public void TestDeleteInvalidId()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();
            repository.Save(new Apple {Type = "Elstar"});

            Assert.AreEqual(2, repository.Count());

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => repository.Delete(-1));

            Assert.AreEqual(2, repository.Count());
        }

        [Test]
        public void TestDeleteInvalidList()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();
            repository.Save(new Apple {Type = "Elstar"});

            Assert.AreEqual(2, repository.Count());

            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                List<Apple> list = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                repository.Delete(list);
            });

            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                List<Apple> list = new List<Apple> {null};
                repository.Delete(list);
            });

            Assert.AreEqual(2, repository.Count());
        }

        [Test]
        public void TestDeleteInvalidEntity()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();
            repository.Save(new Apple {Type = "Elstar"});

            Assert.AreEqual(2, repository.Count());

            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                Apple entity = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                repository.Delete(entity);
            });

            Assert.Throws(typeof(ArgumentOutOfRangeException), () =>
            {
                Apple entity = new Apple
                {
                    Id = -1
                };
                repository.Delete(entity);
            });

            Assert.AreEqual(2, repository.Count());
        }

        [Test]
        public void TestExists()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.IsTrue(repository.Exists(1));
            Assert.IsFalse(repository.Exists(2));
        }

        [Test]
        public void TestExistsInvalidId()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => repository.Exists(-1));
        }

        [Test]
        public void TestFindAll()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.AreEqual(1, repository.FindAll().Count);
            Assert.AreEqual(1, repository.FindAll(new List<int> {1}).Count);
            Assert.AreEqual(0, repository.FindAll(new List<int> {2}).Count);
        }

        [Test]
        public void TestFindAllInvalidList()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                List<int> list = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                repository.FindAll(list);
            });
        }

        [Test]
        public void TestFindAllWhere()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.AreEqual(1, repository.FindAllWhere(apple => apple.Type == "Goudreinette").Count);
            Assert.AreEqual(0, repository.FindAllWhere(apple => apple.Type == "Elstar").Count);
        }

        [Test]
        public void TestFindAllWhereInvalid()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                Func<Apple, bool> func = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                repository.FindAllWhere(func);
            });

            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                Func<Apple, int, bool> func = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                repository.FindAllWhere(func);
            });
        }

        [Test]
        public void TestFindOne()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.AreEqual("Goudreinette", repository.FindOne(1).Type);
            Assert.IsNull(repository.FindOne(2));
        }

        [Test]
        public void TestFindOneInvalidId()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => repository.FindOne(-1));
        }

        [Test]
        public void TestSave()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            repository.Save(new Apple {Type = "Elstar"});

            Assert.AreEqual(2, repository.Count());
            Assert.AreEqual(2, repository.FindAll()[1].Id);
            Assert.AreEqual("Elstar", repository.FindAll()[1].Type);
        }

        [Test]
        public void TestSaveId()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            repository.Save(new Apple {Id = 12, Type = "Elstar"});

            Assert.AreEqual(2, repository.Count());
            Assert.AreEqual(12, repository.FindAll()[1].Id);
            Assert.AreEqual("Elstar", repository.FindAll()[1].Type);
        }

        [Test]
        public void TestSaveInvalidId()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.AreEqual(1, repository.Count());

            Assert.Throws(typeof(ArgumentOutOfRangeException),
            () => repository.Save(new Apple {Id = -1, Type = "Elstar"}));

            Assert.AreEqual(1, repository.Count());
        }

        [Test]
        public void TestSaveInvalidList()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.AreEqual(1, repository.Count());

            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                List<Apple> list = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                repository.Save(list);
            });

            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                List<Apple> list = new List<Apple> {null};
                repository.Save(list);
            });

            Assert.AreEqual(1, repository.Count());
        }

        [Test]
        public void TestSaveInvalidEntity()
        {
            IRepository<Apple> repository = Injector.Resolve<IRepository<Apple>>();

            Assert.AreEqual(1, repository.Count());

            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                Apple entity = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                repository.Save(entity);
            });

            Assert.Throws(typeof(ArgumentOutOfRangeException), () =>
            {
                Apple entity = new Apple
                {
                    Id = -1
                };
                repository.Save(entity);
            });

            Assert.AreEqual(1, repository.Count());
        }
    }
}