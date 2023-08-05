using LIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LIMS.Infrastructure.Persistence.Configuration;

public class MeetingEntityConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.HasKey(meeting => meeting.Id);

        builder.HasMany(meeting => meeting.Users).WithMany(user => user.Meetings);

        builder
            .HasOne(meeting => meeting.Server)
            .WithMany(server => server.Meetings)
            .HasForeignKey("ServerId");

        builder.HasMany(meeting => meeting.MemberShips).WithOne(member => member.Meeting);

        builder.HasOne(meeting => meeting.Record).WithOne(record => record.Meeting);
    }
}
