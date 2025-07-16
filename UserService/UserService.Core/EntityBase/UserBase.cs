using System.ComponentModel.DataAnnotations;

namespace UserService.Core.EntityBase
{
    public abstract class UserBase
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? ZipCode { get; set; }
    }
}
