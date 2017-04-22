using Model.DataProviders;
using Moq;
using NUnit.Framework;
using Organizer.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Organizer.Tests
{
    [TestFixture]
    public class CategoriesTests
    {
        CategoriesProvider mockDbProvider;
        Mock<DbSet<Category>> mockDbSet;

        [SetUp]
        public void Initialize()
        {
            var mockDbContext = new Mock<DataContext>();
            mockDbSet = new Mock<DbSet<Category>>();
            mockDbContext.Setup(m => m.Categories).Returns(mockDbSet.Object);
            mockDbContext.Setup(c => c.Set<Category>()).Returns(mockDbSet.Object);

            mockDbProvider = new CategoriesProvider(mockDbContext.Object);
        }

        [Test]
        public void SaveValidCategory_AdedInDb()
        {
            var category = new Category
            {
                Id = 15,
                Name = "Test category",
                Priority = 10,
            };
            mockDbProvider.Insert(category);
            mockDbProvider.Save();

            mockDbSet.Verify(s => s.Add(It.IsAny<Category>()), Times.Once());
        }

        [Test]
        public void GetCategories_OrderedByPriority()
        {
            var data = InitializeData();
            var dataOrdered = data.OrderBy(c => c.Priority).ToList();

            var dataFromDb = mockDbProvider.GetAll();
            Assert.That(dataFromDb, Has.Count.EqualTo(dataOrdered.Count));
            Assert.That(dataFromDb, Is.EqualTo(dataOrdered));
        }

        [Test]
        public void UpdatePriority_SwapsCategories()
        {
            var data = InitializeData();
            var swapItem = data.First(c => c.Id == 5);
            int oldPriority = swapItem.Priority;
            int newPriority = 1;
            var swapId = data.First(c => c.Priority == 1).Id;
            mockDbProvider.UpdatePriority(5, 1);

            var dataFromDb = mockDbProvider.GetAll();
            var firstItem = dataFromDb.First(c => c.Id == 5);
            Assert.That(firstItem, Is.Not.Null);
            Assert.That(firstItem.Priority, Is.EqualTo(newPriority));

            var swappedItem = dataFromDb.First(c => c.Id == swapId);
            Assert.That(swappedItem, Is.Not.Null);
            Assert.That(swappedItem.Priority, Is.EqualTo(oldPriority));
        }

        [Test]
        public void GetCategory_ReturnsObject()
        {
            var data = InitializeData();

            var dataFromDb = mockDbProvider.GetById(5);
            Assert.That(dataFromDb, Is.Not.Null);
        }

        private IEnumerable<Category> InitializeData()
        {
            var data = new List<Category>
            {
                new Category { Id = 1, Name = "Category1", Priority = 5 },
                new Category { Id = 2, Name = "Category2", Priority = 2 },
                new Category { Id = 3, Name = "Category3", Priority = 4 },
                new Category { Id = 4, Name = "Category4", Priority = 1 },
                new Category { Id = 5, Name = "Category5", Priority = 3 },
            };
            var dataQ = data.AsQueryable();

            mockDbSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(dataQ.Provider);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(dataQ.Expression);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(dataQ.ElementType);
            mockDbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(dataQ.GetEnumerator());
            mockDbSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.Id == (int)ids[0]));
            mockDbSet.Setup(m => m.Add(It.IsAny<Category>())).Callback<Category>(data.Add);

            return data;
        }
    }
}
