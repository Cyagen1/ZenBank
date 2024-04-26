using ZenCore.Entities;

namespace ZenCore.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly BankContext _bankContext;

        public UserRepository(BankContext bankContext)
        {
            _bankContext = bankContext;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return _bankContext.Users.ToList();
        }
        public async Task<User> GetUserById(Guid id)
        {
            return await _bankContext.Users.FindAsync(id);
        }

        public async Task CreateUserAsync(User user)
        {
            await _bankContext.Users.AddAsync(user);
            await _bankContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _bankContext.Users.FindAsync(id);
            _bankContext.Users.Remove(user);
            await _bankContext.SaveChangesAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var currentUser = await _bankContext.Users.FindAsync(user.Id);
            if (currentUser != null)
            {
                currentUser.Name = user.Name;
                currentUser.Email = user.Email;
                await _bankContext.SaveChangesAsync();
                return currentUser;
            }
            return null;
        }
    }
}
