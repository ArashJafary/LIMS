using LIMS.Domain.Entities;
using LIMS.Domain.Enumerables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LIMS.Infrastructure.Persistence.Configuration;

public class MeetingEntityConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.HasKey(meeting => meeting.Id);

        builder
            .HasOne(meeting => meeting.Server)
            .WithMany(server => server.Meetings)
            .HasForeignKey("ServerId");

        builder.HasMany(meeting => meeting.MemberShips).WithOne(member => member.Meeting);

        builder.HasOne(meeting => meeting.Record).WithOne(record => record.Meeting);

        //builder.HasData(new Meeting("asp-1243", true, "Asp.Net Core Course", "mp", "ap", DateTime.Now, DateTime.Now.AddHours(1), 10, string.Empty, true, false,
            //new Server("https://test-install.blindsidenetworks.com/bigbluebutton/api/", "8cd8ef52e8e101574e400365b55e11a6", 100, true), true, Domain.Enumerables.PlatformTypes.BigBlueButton));
    }
}
