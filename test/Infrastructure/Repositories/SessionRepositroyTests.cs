using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entity;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LIMS.Test.Infrastructure.Repositories
{
    namespace LIMS.Test.Infrastructure.Repositories
    {
        public class SessionRepositoryTests
        {
        //    private Mock<LimsContext> _Context;
        //    public SessionRepositoryTests()
        //    {
        //        _Context = new Mock<LimsContext>();
        //    }
        //    [Fact]
        //    public void Get_All_session()
        //    {
        //        //Arrang
        //        _Context.Setup<DbSet<Meeting>>(ses => ses.Meetings)
        //            .ReturnsDbSet(GetSessions());
        //        //Act
        //        var SesionRePository = new SessionRepository(_Context.Object);
        //        var sessions = SesionRePository.GetAll().Result;
        //        //Assert
        //        Assert.Equal(3, sessions.Count);
        //    }
        //    private List<Session> GetSessions()
        //    {
        //        return new List<Session>()
        //        {
        //            new Meeting(true
        //                ,"test"
        //                ,"test1234"
        //                ,"test4321"),
        //            new Meeting(true
        //                ,"test2"
        //                ,"test1234"
        //                ,"test4321"),
        //            new Session(true
        //                ,"test3"
        //                ,"test1234"
        //                ,"test4321")
        //        };
        //    }
        //}
    }
}
