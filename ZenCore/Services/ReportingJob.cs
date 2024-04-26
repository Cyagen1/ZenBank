using Quartz;
using ZenCore.DataAccess;

namespace ZenCore.Services
{
    public class ReportingJob : IJob
    {
        private readonly IReportingService _reportingService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;

        public ReportingJob(IReportingService reportingService, ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _reportingService = reportingService;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var users = await _userRepository.GetAllUsersAsync();
            foreach(var user in users)
            {
                var transactions = _transactionRepository.FindUserTransactionsForToday(user.Id);
                await _reportingService.SendReportsForAllUsersAsync(user, transactions);
            }
        }
    }
}
