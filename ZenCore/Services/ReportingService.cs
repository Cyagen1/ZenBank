using ZenCore.Services.ZenReporting;
using ZenReporting.Contracts;

namespace ZenCore.Services
{
    public class ReportingService : IReportingService
    {
        private readonly IZenReportingClient _zenReportingClient;

        public ReportingService(IZenReportingClient zenReportingClient)
        {
            ArgumentNullException.ThrowIfNull(zenReportingClient);
            _zenReportingClient = zenReportingClient;
        }
        public async Task SendReportsForAllUsersAsync(Entities.User user, IEnumerable<Entities.Transaction> transactions)
        {
            var sum = transactions.Sum(x => x.Amount);
            var userContract = new User(user.Name, user.Email);
            var transactionsContract = new List<Transaction>();
            foreach (var transaction in transactions)
            {
                transactionsContract.Add(new Transaction(transaction.Amount, transaction.Currency, transaction.CreatedAt));
            }
            await _zenReportingClient.SendReportAsync(new Report(userContract, transactionsContract, sum, DateTime.UtcNow.Date));
        }
    }
}
