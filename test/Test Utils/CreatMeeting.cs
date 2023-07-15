using Castle.Components.DictionaryAdapter;
using LIMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Test.Test_Utils
{
    public static class CreatMeeting
    {
        private static Random _rnd =new Random();
        public static List<Meeting> CreatMeetings(int count=1)=>
            Enumerable.Range(0, count)
            .Select( index=>
                new Meeting(
                    $"TestID {index}",
                    index%2==0,
                    $"TestName {index}",
                    $"TestModeratorPassword {index}",
                    $"TestAttendedPawwword {index}",
                    DateTime.Now,
                    DateTime.Now,
                    index,
                    $"TestParentID {index}",
                    index % 3 == 0,
                    index % 2 == 0,
                    new Server("Test"+index,index),
                    index % 3 == 0,
                    (Domain.Enumerables.PlatformTypes)_rnd.Next(1,3))).ToList();
    }
}
