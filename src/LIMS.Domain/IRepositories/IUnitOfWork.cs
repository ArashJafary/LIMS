using Microsoft.EntityFrameworkCore;

namespace LIMS.Domain.IRepositories;
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}