using System.ComponentModel.DataAnnotations;

namespace ZenReporting.Contracts
{
    public class Report(User user, IEnumerable<Transaction> transactions, decimal transactionsSum, DateTime date)
    {
        [Required]
        public User User { get; } = user;
        [Required]
        public IEnumerable<Transaction> Transactions { get; } = transactions;
        [Required]
        public decimal TransactionsSum { get; } = transactionsSum;
        [Required]
        public DateTime Date { get; } = date;
    }
}
