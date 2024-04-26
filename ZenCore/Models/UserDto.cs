using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenCore.Models
{
    [method: JsonConstructor]
    public class UserDto(string name, string email)
    {
        [Required]
        public string Name { get; } = name;
        [Required]
        public string Email { get; } = email;
    }
}
