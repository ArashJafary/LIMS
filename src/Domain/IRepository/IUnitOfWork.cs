namespace BigBlueApi.Domain.IRepository;
public interface IUnitOfWork 
{
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}