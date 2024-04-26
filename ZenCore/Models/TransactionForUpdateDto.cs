using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenCore.Models
{
    [method: JsonConstructor]
    public class TransactionForUpdateDto(Guid id, decimal amount, string currency, DateTime createdAt)
    {
        [Required]
        public Guid Id { get; } = id;
        [Required]
        public decimal Amount { get; } = amount;
        [Required]
        public string Currency { get; } = currency;
        [Required]
        public DateTime CreatedAt { get; } = createdAt;
    }
}
