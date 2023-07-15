using LIMS.Domain;
using LIMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Test.Test_Utils
{
    public static class CreatUser
    {
        private static Random _rnd = new Random();
        public static List<User> CreatUsers(int count=1)=>
            Enumerable.Range(0, count).
            Select(index=>
            new User($"Test {index}",
                $"Test2 {index}",
                new UserRole((UserRoleTypes) _rnd.Next(-1,2)))).ToList();
    }
}
