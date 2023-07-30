using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using LIMS.Application.Mappers;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Http.BBB;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using LIMS.Test.Utils;
using Microsoft.Extensions.Logging;
using Moq;

namespace LIMS.Test.Application.Services.Database.BBB
{
    public class BbbServerServiceImplTests
    {
        private readonly Mock<IServerRepository> _serverRepositoryMock
            = new Mock<IServerRepository>();
        private readonly Mock<BbbServerActiveService> _activeServiceMock
            = new Mock<BbbServerActiveService>();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock
            = new Mock<IUnitOfWork>();
        private readonly Mock<ILogger<BbbServerServiceImpl>> _loggerMock = new Mock<ILogger<BbbServerServiceImpl>>();

        private async Task<List<Server>> GetServers()
        {
            var servers = await CreateServerUtils.CreateServers(5);
            var meetings = await CreateMeetingUtils.CreateMeetings(5);
            var users = await CreateUserUtils.CreateUsers(5);

            meetings.ForEach(meeting => meeting.Users.AddRange(users));
            //servers.ForEach(server => server.Meetings.AddRange(meetings));

            return servers;
        }

        private async Task<Server> GetServer() => await Task.Run(() =>
        {
            var server = new Server("https://TestServer1.com",
               "secret1234",
               100);

            var meetings = CreateMeetingUtils.CreateMeetings(5).Result;
            var users = CreateUserUtils.CreateUsers(5).Result;

            meetings.ForEach(meeting => meeting.Users.AddRange(users));
            //server.Meetings.AddRange(meetings);

            return server;
        });

        [Fact]
        public async Task CanJoinUser_IfServerNotFound()
        {
            var server = await GetServer();
            _serverRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<long>())).Returns(null);

            var service = new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);

            var result = await service.CanJoinServer(1);

            Assert.False(result.Success);
            Assert.False(result.Result);
            Assert.NotNull(result.OnFailedMessage);
            Assert.Null(result.Exception);
        }

        [Fact]
        public async Task CanJoinUser_IfServerLimitIsFull()
        {
            var server = await GetServer();
            _serverRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(server);

            var service = new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);

            var result = await service.CanJoinServer(1);

            Assert.Null(result.Exception);
            Assert.NotNull(result.OnFailedMessage);
            Assert.False(result.Success);
            Assert.False(result.Result);
        }

        [Fact]
        public async Task CanJoinUser_IfServerHasEmptyCapacity()
        {
            var server = await GetServer();

            _serverRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(server);

            var service =new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);

            var result = await service.CanJoinServer(1);

            Assert.Null(result.Exception);
            Assert.Null(result.OnFailedMessage);
            Assert.True(result.Success);
            Assert.True(result.Result);
        }

        [Fact]
        public async Task UpdateServer_IfInputServerIsNull()
        {
            var service =new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);

            var result = await service.UpdateServer(1, null);

            Assert.False(result.Success);
            Assert.NotNull(result.OnFailedMessage);
            Assert.Null(result.Exception);
        }

        [Fact]
        public async Task UpdateServer_IfServerNotFound()
        {
            _serverRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<long>())).Returns(null);

            var service =new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);
            var result = await service.UpdateServer(1, ServerDtoMapper.Map(await GetServer()));

            Assert.False(result.Success);
            Assert.NotNull(result.OnFailedMessage);
            Assert.Null(result.Exception);
        }

        [Fact]
        public async Task UpdateServer_MustUpdateServerCorrectly()
        {
            var server = await GetServer();
            _serverRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(server);

            var service =new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);
            var result = await service.UpdateServer(1, ServerDtoMapper.Map(await GetServer()));

            Assert.True(result.Success);
            Assert.Null(result.Exception);
            Assert.Null(result.OnFailedMessage);
        }

        [Fact]
        public async Task CreatServer_MustReturnIdOfEntitiyCorrectly()
        {
            var server = await GetServer();
            _serverRepositoryMock.Setup(repository => repository.CreateAsync(server)).ReturnsAsync(server.Id);

            var service =new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);

            var result = await service.CreateServer(ServerDtoMapper.Map(server));

            Assert.True(result.Success);
            Assert.Null(result.Exception);
            Assert.Null(result.OnFailedMessage);
            Assert.Equal(result.Result, server.Id);
        }

        [Fact]
        public async Task CreateServer_IfInputIsNull()
        {
            var service =new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);

            var result = await service.CreateServer(null);

            Assert.False(result.Success);
            Assert.Equal(result.Result, 0);
            Assert.Null(result.Exception);
            Assert.NotNull(result.OnFailedMessage);
        }

        [Fact]
        public async Task MostCapableServer_ReturnCapableServer()
        {
            var servers = await GetServers();
            var expectedServer = servers.FirstOrDefault();


            _serverRepositoryMock.Setup(repository => repository.GetAllAsync()).ReturnsAsync(servers);

            var service =new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);

            var result = await service.MostCapableServer();

            Assert.Equal(ServerDtoMapper.Map(result.Result), expectedServer);
            Assert.Null(result.OnFailedMessage);
        }

        [Fact]
        public async Task DeleteServer_MustRemoveCorrectly()
        {
            var serverId = 1;
            _serverRepositoryMock.Setup(repository => repository.DeleteAsync(It.IsAny<long>())).Verifiable();

            var service =new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);
            

            var result = await service.DeleteServer(serverId);

            _serverRepositoryMock.Verify(repository => repository.DeleteAsync(serverId),Times.Once);
            Assert.True(result.Success);
            Assert.Null(result.Exception);
            Assert.Null(result.OnFailedMessage);
        }

        [Fact]
        public async Task DeleteServer_MostThrowsException()
        {
            var serverId = 1;
            var exception = "Server Not Found.";

            _serverRepositoryMock.Setup(repository => repository.DeleteAsync(It.IsAny<long>())).ThrowsAsync(new Exception(exception)).Verifiable();

            var service =new BbbServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object,_loggerMock.Object);

            var result = await service.DeleteServer(serverId);


            Assert.NotNull(result.Exception);
            Assert.NotNull(result.OnFailedMessage);
            Assert.Equal(exception, result.Exception.Message);
            _serverRepositoryMock.Verify(repository => repository.DeleteAsync(serverId),Times.Once);
        }
    }
}
