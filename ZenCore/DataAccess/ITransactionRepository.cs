using ZenCore.Entities;

namespace ZenCore.DataAccess
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetTransactionByIdAsync(Guid id);
        Task CreateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(Guid id);
        Task<Transaction> UpdateTransactionAsync(Transaction transaction);
        IEnumerable<Transaction> FindUserTransactionsForToday(Guid userId);
    }
}
