using BigBlueApi.Domain.IRepository;
using LIMS.Domain.Entity;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BigBlueApi.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _user;
        public UserRepository(BigBlueContext context) => _user = context.Set<User>(); 
        
        
        public async ValueTask<int> CreateUser(User user)
        {
          var User =  await _user.AddAsync(user);
            return  User.Entity.Id;
        }

        public async Task EditUser(int Id, User user)
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
