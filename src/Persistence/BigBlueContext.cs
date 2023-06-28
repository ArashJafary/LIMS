using BigBlueApi.Domain;
using BigBlueApi.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence;

public class BigBlueContext : DbContext,IUnitOfWork
{
    public BigBlueContext(DbContextOptions<BigBlueContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<MemberShip> MemberShips { get; set; }
    public DbSet<Server> Servers { get; set; }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BigBlueContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
