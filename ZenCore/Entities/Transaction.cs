using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenCore.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        [StringLength(5)]
        public string Currency { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
