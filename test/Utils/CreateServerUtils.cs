using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Entities;

namespace LIMS.Test.Utils
{
    public class CreateServerUtils
    {
        private static Random randomNumber = new Random();

        public static async ValueTask<List<Server>> CreateServers(int count = 1) 
            => await Task.Run(() =>
            Enumerable.Range(0, count).Select(server =>
                    new Server($"Test Server Url {server}",
                        $"Test Shared Secret {server}",
                        randomNumber.Next(50, 101)))
                .ToList());
    }
}
