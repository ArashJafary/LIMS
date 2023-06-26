using BigBlueApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigBlueApi.Persistence.Configuration;

public class ServerEntityConfiguration : IEntityTypeConfiguration<Server>
{
    public void Configure(EntityTypeBuilder<Server> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder
            .HasMany(entity => entity.Sessions)
            .WithOne(entity => entity.Server)
            .HasForeignKey("ServerId");
    }
}
