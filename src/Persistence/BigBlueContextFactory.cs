using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BigBlueApi.Persistence;

public class BigBlueContextFactory : IDesignTimeDbContextFactory<BigBlueContext>
{
    public BigBlueContext CreateDbContext(string[] args)
    {
        var _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var optionsBuilder = new DbContextOptionsBuilder<BigBlueContext>();

        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MSSQL"));
        return new BigBlueContext(optionsBuilder.Options);
    }
}
