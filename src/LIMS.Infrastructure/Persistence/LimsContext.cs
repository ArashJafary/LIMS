using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace LIMS.Infrastructure.Persistence;

public class LimsContext : DbContext, IUnitOfWork
{
    public LimsContext(DbContextOptions<LimsContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<MemberShip> MemberShips { get; set; }
    public DbSet<Server> Servers { get; set; }

    public async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LimsContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
