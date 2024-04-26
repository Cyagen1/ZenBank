using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZenCore.Models
{
    [method: JsonConstructor]
    public class UserForUpdateDto(Guid id, string name, string email)
    {
        [Required]
        public Guid Id { get; set; } = id;
        [Required]
        public string Name { get; set; } = name;
        [Required]
        public string Email { get; set; } = email;
    }
}
