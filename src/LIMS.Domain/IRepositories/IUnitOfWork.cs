namespace BigBlueApi.Domain.IRepository;
public interface IUnitOfWork
{
    ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken);
    ValueTask<int> SaveChangesAsync();
}