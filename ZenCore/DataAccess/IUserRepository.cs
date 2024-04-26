using ZenCore.Entities;

namespace ZenCore.DataAccess
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserById(Guid id);
        Task CreateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<User> UpdateUserAsync(User user);
    }
}
