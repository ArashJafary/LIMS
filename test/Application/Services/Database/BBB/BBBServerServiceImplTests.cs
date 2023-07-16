using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LIMS.Application.Mappers;
using LIMS.Application.Services.Database.BBB;
using LIMS.Application.Services.Http.BBB;
using LIMS.Domain;
using LIMS.Domain.Entities;
using LIMS.Domain.Enumerables;
using LIMS.Domain.IRepositories;
using Moq;

namespace LIMS.Test.Application.Services.Database.BBB
{
    public class BBBServerServiceImplTests
    {
        private readonly Mock<IServerRepository> _serverRepositoryMock
            = new Mock<IServerRepository>();
        private readonly Mock<BBBServerActiveService> _activeServiceMock 
            = new Mock<BBBServerActiveService>();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock 
            = new Mock<IUnitOfWork>();


        private async Task<List<Server>> GetServers()
            => new List<Server>
            {
                new Server("https://TestServer1.com",
                    "secret1234",
                    100),
                new Server("https://TestServer2.com",
                    "secret5678",
                    90),
                new Server("https://TestServer3.com",
                    "secret9876",
                    80),
                new Server("https://TestServer4.com",
                    "secret5432",
                    70)
            };

        private async Task<Server> GetServer() => await Task.Run(() =>
        {
             var server = new Server("https://TestServer1.com",
                "secret1234",
                100);
             //var meeting = new Domain.Entities.Meeting("123b",
             //    true,
             //    "Asp.Net Core",
             //    "mp",
             //    "ap",
             //    DateTime.Now,
             //    DateTime.Now,
             //    5,
             //    "123bParent",
             //    true,
             //    false,
             //    server,
             //    true,
             //    PlatformTypes.BigBlueButton);
             //meeting.Users.Add(new User("Mohammad","MMD",new UserRole(UserRoleTypes.Moderator)));

            return server;
        });  
         

        [Fact]
        public async Task CanJoinUser_IfServerNotFound()
        {
            var server = await GetServer();
            _serverRepositoryMock.Setup(repository => repository.GetServerAsync(It.IsAny<long>())).Returns(null);

            var service = new BBBServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object);

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
            _serverRepositoryMock.Setup(repository => repository.GetServerAsync(It.IsAny<long>())).ReturnsAsync(server);

            var service = new BBBServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object);

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
          
            _serverRepositoryMock.Setup(repository => repository.GetServerAsync(It.IsAny<long>())).ReturnsAsync(server);

            var service = new BBBServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object);

            var result = await service.CanJoinServer(1);

            Assert.Null(result.Exception);
            Assert.Null(result.OnFailedMessage);
            Assert.True(result.Success);
            Assert.True(result.Result);
        }

        [Fact]
        public async Task UpdateServer_IfInputServerIsNull()
        {
            var service = new BBBServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object);

            var result = await service.UpdateServer(1,null);

            Assert.False(result.Success);
            Assert.NotNull(result.OnFailedMessage);
            Assert.Null(result.Exception);
        }

        [Fact]
        public async Task UpdateServer_IfServerNotFound()
        {
            _serverRepositoryMock.Setup(repository => repository.GetServerAsync(It.IsAny<long>())).Returns(null);

            var service = new BBBServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object);
            var result = await service.UpdateServer(1, ServerDtoMapper.Map(await GetServer()));

            Assert.False(result.Success);
            Assert.NotNull(result.OnFailedMessage);
            Assert.Null(result.Exception);
        }

        [Fact]
        public async Task UpdateServer_MustUpdateServerCorrectly()
        {
            var server  = await GetServer();
            _serverRepositoryMock.Setup(repository => repository.GetServerAsync(It.IsAny<long>())).ReturnsAsync(server);

            var service = new BBBServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object);
            var result = await service.UpdateServer(1, ServerDtoMapper.Map(await GetServer()));

            Assert.True(result.Success);
            Assert.Null(result.Exception);
            Assert.Null(result.OnFailedMessage);
        }

        [Fact]
        public async Task CreatServer_MustReturnIdOfEntitiyCorrectly()
        {
            var server = await GetServer();
            _serverRepositoryMock.Setup(repository => repository.CreateServerAsync(server)).ReturnsAsync(server.Id);

            var service = new BBBServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object);

            var result = await service.CreateServer(ServerDtoMapper.Map(server));

            Assert.True(result.Success);
            Assert.Null(result.Exception);
            Assert.Null(result.OnFailedMessage);
            Assert.Equal(result.Result, server.Id);
        }

        [Fact]
        public async Task CreateServer_IfInputIsNull()
        {
            var service = new BBBServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object);

            var result = await service.CreateServer(null);

            Assert.False(result.Success);
            Assert.Equal(result.Result,0);
            Assert.Null(result.Exception);
            Assert.NotNull(result.OnFailedMessage);
        }


        [Fact]
        public async Task MostCapableServer_ReturnCapableServer()
        {
            var servers = await GetServers();
            var expectedServer = servers.FirstOrDefault();


            _serverRepositoryMock.Setup(repository => repository.GetAllServersAsync()).ReturnsAsync(servers);

            var service = new BBBServerServiceImpl(_activeServiceMock.Object, _serverRepositoryMock.Object, _unitOfWorkMock.Object);

            var result = await service.MostCapableServer();

            Assert.Equal(ServerDtoMapper.Map(result.Result),expectedServer);
            Assert.Null(result.OnFailedMessage);
        }

    }
}
