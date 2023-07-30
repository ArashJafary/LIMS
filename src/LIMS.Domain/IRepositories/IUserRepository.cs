using LIMS.Domain.Entities;
using LIMS.Domain.Entities;

namespace LIMS.Domain.IRepositories;
public interface IUserRepository
{
    ValueTask<long> CreateAsync(User User);
    ValueTask<User> GetByIdAsync(long id);
    ValueTask<IEnumerable<User>> GetAllAsync();
    Task<long> DeleteAsync(long UserId);
}