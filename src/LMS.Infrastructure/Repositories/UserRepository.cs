using BigBlueApi.Domain.IRepository;
using LIMS.Domain.Entity;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _user;
        public UserRepository(LimsContext context) => _user = context.Set<User>(); 
        
        public async ValueTask<long> CreateUser(User user)
        {
          var User =  await _user.AddAsync(user);
            return  User.Entity.Id;
        }

        public async ValueTask<User> GetUser(long userId) => await  _user.FirstOrDefaultAsync(user => user.Id == userId);

        public async ValueTask<IEnumerable<User>> GetAllUsers() => await _user.ToListAsync();

        public async Task<long> DeleteUser(long userId)
        {
            var user = await _user.FirstOrDefaultAsync(user => user.Id == userId);
            _user.Remove(user);
            return user.Id;
        }

        public async Task EditUser(long Id, User user)
        {
         var User = await _user.FirstOrDefaultAsync(us => us.Id == user.Id);
          User = user;
         _user.Update(User);
        }

        public async ValueTask<User> Find(int userId)
        {
            var user = await _user.FirstOrDefaultAsync(us => us.Id == userId);
            return user!;
        }
    }
}
