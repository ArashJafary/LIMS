using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigBlueApi.Domain;
using BigBlueApi.Persistence;
using BigBlueApi.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
namespace BigBlueTest.Persistence.Repository
{
    public class ServerRepositoryTests
    {
        private readonly Mock<DbSet<Server>> _dbSet;
        private readonly Mock<BigBlueContext> _context;
        public ServerRepositoryTests() => (_dbSet, _context) = (CreateMockDbSet(), CreateMockContext(_dbSet));
        private Mock<DbSet<Server>> CreateMockDbSet()
        {
            var servers = new List<Server>()
            {
                new Server("Url Test 1","Shared Test 1",1){ Id = 1 },
                new Server("Url Test 2","Shared Test 2",2) { Id = 2},
                new Server("Url Test 2","Shared Test 2",3)
            };

            var mockDbSet = new Mock<DbSet<Server>>();
            mockDbSet.As<IQueryable<Server>>().Setup(m => m.Provider).Returns(servers.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Server>>().Setup(m => m.Expression).Returns(servers.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Server>>().Setup(m => m.ElementType).Returns(servers.AsQueryable().ElementType);

            mockDbSet.As<IQueryable<Server>>().Setup(m => m.GetEnumerator()).Returns(servers.AsQueryable().GetEnumerator());
            return mockDbSet;
        }
        private Mock<BigBlueContext> CreateMockContext(Mock<DbSet<Server>> mockDbSet)
        {
            var mockContext = new Mock<BigBlueContext>();
            mockContext.Setup(c => c.Set<Server>()).Returns(mockDbSet.Object);

            return mockContext;
        }


        [Fact]
        public async Task CanJoinServer_ReturnsTrue_WhenServerLimit_NotReached()
        {
            var repository = new ServerRepository(_context.Object);

            var result = await repository.CanJoinServer(1);

            Assert.True(result);
        }
        [Fact]
        public async Task CreateServer_Returns_NewIdFromServer()
        {
            var repository = new ServerRepository(_context.Object);

            var newServer = new Server("Url Test", "Shared Test", 5);
            var result = await repository.CreateServer(newServer);

            Assert.Equal(1,result);
        }
    }
}
