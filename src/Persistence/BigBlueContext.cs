using BigBlueApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence;

public class BigBlueContext : DbContext
{
    public BigBlueContext(DbContextOptions<BigBlueContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<MemberShip> MemberShips { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BigBlueContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
