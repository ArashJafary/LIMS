using BigBlueApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigBlueApi.Persistence.Configuration;

public class SessionEntityConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(session => session.Id);
        builder.HasMany(session => session.Users).WithMany(user => user.Sessions);
        builder
            .HasOne(session => session.Server)
            .WithMany(server => server.Sessions)
            .HasForeignKey("ServerId");
        builder.HasMany(session => session.MemberShips).WithMany(member => member.Sessions);
    }
}
