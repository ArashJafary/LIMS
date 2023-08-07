using LIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Infrastructure.Persistence.Configuration
{
    public class MemberShipEntityConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            //builder.HasData(new MemberShip(
            //    1, new Meeting("asp-1243", true, "Asp.Net Core Course", "mp", "ap", DateTime.Now, DateTime.Now.AddHours(1), 10, string.Empty, true, false,
            //    new Server("https://test-install.blindsidenetworks.com/bigbluebutton/api/", "8cd8ef52e8e101574e400365b55e11a6", 100, true), true, Domain.Enumerables.PlatformTypes.BigBlueButton),
            //    new User("Mohammad Mahdi Moghaddam", "MMD", Domain.UserRoleTypes.Moderator)));
        }
    }
}
