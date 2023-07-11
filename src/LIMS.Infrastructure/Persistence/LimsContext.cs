using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LIMS.Infrastructure.Persistence;

public class LimsContext : DbContext, IUnitOfWork
{
    public LimsContext(DbContextOptions<LimsContext> options)
        : base(options) { }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<Meeting> Meetings { get; set; }
    public virtual DbSet<MemberShip> MemberShips { get; set; }
    public virtual DbSet<Server> Servers { get; set; }

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

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {

    }
}
