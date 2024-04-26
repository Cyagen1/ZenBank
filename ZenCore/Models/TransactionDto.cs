using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenCore.Models
{
    [method: JsonConstructor]
    public class TransactionDto(decimal amount, string currency, Guid userId, DateTime createdAt)
    {
        [Required]
        public decimal Amount { get; } = amount;
        [Required]
        public string Currency { get; } = currency;
        [Required]
        public Guid UserId { get; } = userId;
        [Required]
        public DateTime CreatedAt { get; } = createdAt;
    }
}
