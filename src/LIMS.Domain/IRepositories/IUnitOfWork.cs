namespace LIMS.Domain.IRepositories;
public interface IUnitOfWork
{
    ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken);
    ValueTask<int> SaveChangesAsync();
}