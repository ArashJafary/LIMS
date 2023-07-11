using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Http.BBB;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using Moq;

namespace LIMS.Test.Application.Services.Database.BBB
{
    public class BBBServerServiceImplTests
    {
        private readonly Mock<IServerRepository> _serverRepositoryMock = new Mock<IServerRepository>();
        private readonly Mock<ServerActiveService> _activeServiceMock = new Mock<ServerActiveService>();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task CanJoinUser_IfServerNotFound()
        {
          
        }

        [Fact]
        public async Task CanJoinUser_IfServerLimitIsFull()
        {

        }

        [Fact]
        public async Task CanJoinUser_IfServerHasEmptyCapacity()
        {

        }

        [Fact]
        public async Task UpdateServer_IfServerNotFound()
        {

        }

        [Fact]
        public async Task UpdateServer_MustUpdateServerCorrectly()
        {

        }
    }
}
