using LIMS.Domain.IRepositories;
using LIMS.Domain.Entities;
using LIMS.Domain.Entities;
using LIMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using LIMS.Domain.Exceptions.Database.BBB;

namespace LIMS.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _User;
        public UserRepository(IUnitOfWork context) => _User = context.Set<User>();

        public async ValueTask<long> CreateAsync(User User)
        {
            var newUser = await _User.AddAsync(User);

            ThrowExpectedExceptions(newUser.Entity,false,true);

            return newUser.Entity.Id;
        }

        public async ValueTask<User> GetByIdAsync(long id)
        {
            var userById = await _User.FirstOrDefaultAsync(user => user.Id == id);

            ThrowExpectedExceptions(userById);

            return userById!;
        }

        public async ValueTask<IEnumerable<User>> GetAllAsync() => await _User.ToListAsync();

        public async Task<long> DeleteAsync(long userId)
        {
            var user = await GetByIdAsync(userId);

            ThrowExpectedExceptions(user);

            _User.Remove(user);

            return user.Id;
        }

        private void ThrowExpectedExceptions(User? user, bool argumentNullThrow = false, bool createdInDatabase = false)
        {
            if (user is null)
                if (argumentNullThrow)
                    throw new ArgumentNullException("Server Cannot Be Null.");
                else if (createdInDatabase)
                    throw new EntityConnotAddInDatabaseException("Cannot Create Server On Database");
                else
                    throw new NotAnyEntityFoundInDatabaseException("Not Any Server Found With Expected Datas");
        }
    }
}
