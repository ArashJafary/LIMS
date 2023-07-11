using LIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LIMS.Infrastructure.Persistence.Configuration;
public class ServerEntityConfiguration : IEntityTypeConfiguration<Server>
{
    public void Configure(EntityTypeBuilder<Server> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder
            .HasMany(entity => entity.Meetings)
            .WithOne(entity => entity.Server)
            .HasForeignKey("ServerId");
    }
}