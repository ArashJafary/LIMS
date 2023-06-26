namespace BigBlueApi.Domain.IRepository;
public interface IUnitOfWork 
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync();
}