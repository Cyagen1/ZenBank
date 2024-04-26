using System.ComponentModel.DataAnnotations;

namespace ZenReporting.Contracts
{
    public class User(string name, string email)
    {
        [Required]
        public string Name { get; } = name;
        [Required]
        public string Email { get; } = email;
    }
}
