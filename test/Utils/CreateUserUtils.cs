using LIMS.Domain;
using LIMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Test.Utils
{
    public static class CreateUserUtils
    {
        private static Random randomNumber = new Random();

        public static async ValueTask<List<User>> CreateUsers(int count = 1) 
            => await Task.Run(() =>
            {
                return Enumerable.Range(0, count).
                    Select(user =>
                        new User(
                            $"Test {user}",
                            $"Test 2 {user}",
                            UserRoleTypes.Attendee))
                    .ToList();
            });
    }
}
