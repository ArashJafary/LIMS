using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LIMS.Infrastructure.Persistence.Configuration;

public class MeetingEntityConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.HasKey(session => session.Id);
        builder.HasMany(session => session.Users).WithMany(user => user.Meeting);
        builder
            .HasOne(session => session.Server)
            .WithMany(server => server.Meetings)
            .HasForeignKey("ServerId");
        builder.HasMany(session => session.MemberShips).WithOne(member => member.Meeting);
    }
}
