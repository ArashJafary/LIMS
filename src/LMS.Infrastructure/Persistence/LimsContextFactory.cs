using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LIMS.Infrastructure.Persistence;

public class LimsContextFactory : IDesignTimeDbContextFactory<LimsContext>
{
    public LimsContext CreateDbContext(string[] args)
    {
        var _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var optionsBuilder = new DbContextOptionsBuilder<LimsContext>();

        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MSSQL"));
        return new LimsContext(optionsBuilder.Options);
    }
}
