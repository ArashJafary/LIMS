using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LIMS.Infrastructure.Persistence;

public class LimsContextFactory : IDesignTimeDbContextFactory<LimsContext>
{
    public LimsContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<LimsContextFactory>();

        var builder = configuration.Build();

        var optionsBuilder = new DbContextOptionsBuilder<LimsContext>();

        optionsBuilder.UseSqlServer(builder.GetConnectionString("MSSQL"));

        return new LimsContext(optionsBuilder.Options);
    }
}
