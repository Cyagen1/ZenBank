using ZenCore.Entities;

namespace ZenCore.Services
{
    public interface IReportingService
    {
        Task SendReportsForAllUsersAsync(User user, IEnumerable<Transaction> transactions);
    }
}
