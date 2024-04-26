using System.ComponentModel.DataAnnotations;

namespace ZenCore.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(125)]
        public string Name { get; set; }

        [StringLength(55)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
