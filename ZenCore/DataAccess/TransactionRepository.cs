using ZenCore.Entities;

namespace ZenCore.DataAccess
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankContext _bankContext;

        public TransactionRepository(BankContext bankContext)
        {
            _bankContext = bankContext;
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            return await _bankContext.Transactions.FindAsync(id);
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            await _bankContext.Transactions.AddAsync(transaction);
            await _bankContext.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(Guid id)
        {
            var transaction = await _bankContext.Transactions.FindAsync(id);
            _bankContext.Transactions.Remove(transaction);
            await _bankContext.SaveChangesAsync();
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            var currentTransaction = await _bankContext.Transactions.FindAsync(transaction.Id);
            if (currentTransaction != null)
            {
                currentTransaction.Amount = transaction.Amount;
                currentTransaction.Currency = transaction.Currency;
                currentTransaction.CreatedAt = transaction.CreatedAt;
                await _bankContext.SaveChangesAsync();
                return currentTransaction;
            }
            return null;
        }

        public IEnumerable<Transaction> FindUserTransactionsForToday(Guid userId)
        {
            return _bankContext.Transactions.Where(x => x.UserId == userId && x.CreatedAt.Date == DateTime.UtcNow.Date).ToList();
        }
    }
}
