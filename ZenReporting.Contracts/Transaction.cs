using System.ComponentModel.DataAnnotations;

namespace ZenReporting.Contracts
{
    public class Transaction(decimal amount, string currency, DateTime createdAt)
    {
        [Required]
        public decimal Amount { get; } = amount;
        [Required]
        public string Currency { get; } = currency;
        [Required]
        public DateTime CreatedAt { get; } = createdAt;
    }
}
