using Castle.Components.DictionaryAdapter;
using LIMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Test.Utils
{
    public static class CreateMeetingUtils
    {
        private static Random randomNumber = new Random();

        public static async ValueTask<List<Meeting>> CreateMeetings(int count = 1) => await Task.Run(() => Enumerable.Range(0, count)
            .Select(meeting =>
                new Meeting(
                    $"TestID {meeting}",
                    meeting % 2 == 0,
                    $"Test Name {meeting}",
                    $"TestModeratorPassword {meeting}",
                    $"TestAttendedPassword {meeting}",
                    DateTime.Now,
                    DateTime.Now,
                    meeting,
                    $"TestParentID {meeting}",
                    meeting % 3 == 0,
                    meeting % 2 == 0,
                    new Server("Test Server" + meeting, meeting),
                    meeting % 3 == 0,
                    (Domain.Enumerables.PlatformTypes)randomNumber.Next(1, 3)))
            .ToList());
    }
}
