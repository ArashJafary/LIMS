using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _User;
        public UserRepository(LimsContext context) => _User = context.Set<User>(); 
        
        public async ValueTask<long> CreateUser(User User)
        {
          var NewUser =  await _User.AddAsync(User);
            return NewUser.Entity.Id;
        }

        public async ValueTask<User> GetUser(long UserId) => await _User.FirstOrDefaultAsync(user => user.Id == UserId);

        public async ValueTask<IEnumerable<User>> GetUsers() => await _User.ToListAsync();

        public async Task<long> DeleteUser(long userId)
        {
            var user = await _User.FirstOrDefaultAsync(user => user.Id == userId);
            _User.Remove(user);
            return user.Id;
        }
    }
}
