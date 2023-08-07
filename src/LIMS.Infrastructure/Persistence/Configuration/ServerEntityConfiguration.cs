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

        //builder.HasData(new Server("https://test-install.blindsidenetworks.com/bigbluebutton/api/", "8cd8ef52e8e101574e400365b55e11a6",100,true));
    }
}