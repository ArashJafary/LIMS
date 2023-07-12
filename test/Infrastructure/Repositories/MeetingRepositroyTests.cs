using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using LIMS.Domain.IRepositories;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LIMS.Test.Infrastructure.Repositories
{
       
    public class MeetingRepositroyTests
    {
        private Mock<IMeetingRepository> _meetingRepositoryMock = new Mock<IMeetingRepository>();

        public async ValueTask<IEnumerable<Meeting>> GetMeetings() => await Task.Run(() =>
        {
            return new List<Meeting>
            {
                new Meeting ("abcde", true, "Asp.Net Core Course", "1234", "012345"),
                new Meeting ("fghij", true, "NodeJs Course", "5678", "6789"),
                new Meeting ("klmno", true, "Unit Testing Course", "9012", "09876")
            };
        });

        [Fact]
        public async Task CreateMeeting_MustReturnCorrectMeetingId()
        {
            var meeting = new Meeting
                ("abcde", true, "Asp.Net Core Course", "1234", "012345");
            _meetingRepositoryMock.Setup(repo 
                => repo.CreateMeetingAsync(meeting)).ReturnsAsync(meeting.MeetingId);

            var result = await _meetingRepositoryMock.Object.CreateMeetingAsync(meeting);

            Assert.Equal(meeting.MeetingId,result);
        }

        [Fact]
        public async Task GetMeetings_MustReturnListOfMeetings()
        {
            var meetings = await GetMeetings();
            _meetingRepositoryMock.Setup(repo => repo.GetMeetingsAsync()).ReturnsAsync(meetings);

            var result = await _meetingRepositoryMock.Object.GetMeetingsAsync();

            Assert.Equal(meetings,result);
        }

        [Fact]
        public async Task DeleteMeeting_MustRemoveCorrectly()
        {
            var meeting = new Meeting
                ("abcde", true, "Asp.Net Core Course", "1234", "012345");
            long meetingId = 1;

        }

        [Fact]
        public async Task FindMeeting_MustReturnCorrectMeeting()
        {
            var meeting = new Meeting
                ("abcde", true, "Asp.Net Core Course", "1234", "012345");
            

        }

        [Fact]
        public async Task FindByMeetingId_MustReturnCorrectMeeting()
        {
            
        }
    }
}
