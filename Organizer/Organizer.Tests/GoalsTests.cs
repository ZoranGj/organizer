using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using Organizer.Model;
using Model.DataProviders;

namespace Organizer.Tests
{
    [TestFixture]
    public class GoalsTests
    {
        GoalsProvider mockDbProvider;
        Mock<DbSet<Goal>> mockDbSet;

        [SetUp]
        public void Initialize()
        {
            var mockDbContext = new Mock<DataContext>();
            mockDbSet = new Mock<DbSet<Goal>>();
            mockDbContext.Setup(m => m.Goals).Returns(mockDbSet.Object);
            mockDbContext.Setup(c => c.Set<Goal>()).Returns(mockDbSet.Object);

            mockDbProvider = new GoalsProvider(mockDbContext.Object);
        }

        [Test]
        public void SaveValidGoal_AdedInDb()
        {
            var goal = new Goal
            {
                Id = 15,
                Name = "Test goal",
                Priority = 10,
            };
            mockDbProvider.Insert(goal);
            mockDbProvider.Save();

            mockDbSet.Verify(s => s.Add(It.IsAny<Goal>()), Times.Once());
        }

        [Test]
        public void GetGoals_OrderedByPriority()
        {
            var data = InitializeData();
            var dataOrdered = data.OrderBy(c => c.Priority).ToList();

            var dataFromDb = mockDbProvider.GetAll();
            Assert.That(dataFromDb, Has.Count.EqualTo(dataOrdered.Count));
            Assert.That(dataFromDb, Is.EqualTo(dataOrdered));
        }

        [Test]
        public void UpdatePriority_SwapsGoals()
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
        public void GetGoal_ReturnsObject()
        {
            var data = InitializeData();

            var dataFromDb = mockDbProvider.GetById(5);
            Assert.That(dataFromDb, Is.Not.Null);
        }

        private IEnumerable<Goal> InitializeData()
        {
            var data = new List<Goal>
            {
                new Goal { Id = 1, Name = "Goal1", Priority = 5 },
                new Goal { Id = 2, Name = "Goal2", Priority = 2 },
                new Goal { Id = 3, Name = "Goal3", Priority = 4 },
                new Goal { Id = 4, Name = "Goal4", Priority = 1 },
                new Goal { Id = 5, Name = "Goal5", Priority = 3 },
            };
            var dataQ = data.AsQueryable();

            mockDbSet.As<IQueryable<Goal>>().Setup(m => m.Provider).Returns(dataQ.Provider);
            mockDbSet.As<IQueryable<Goal>>().Setup(m => m.Expression).Returns(dataQ.Expression);
            mockDbSet.As<IQueryable<Goal>>().Setup(m => m.ElementType).Returns(dataQ.ElementType);
            mockDbSet.As<IQueryable<Goal>>().Setup(m => m.GetEnumerator()).Returns(dataQ.GetEnumerator());
            mockDbSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.Id == (int)ids[0]));
            mockDbSet.Setup(m => m.Add(It.IsAny<Goal>())).Callback<Goal>(data.Add);

            return data;
        }
    }
}
