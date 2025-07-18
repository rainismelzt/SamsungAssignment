using System.ComponentModel.DataAnnotations;

namespace UserService.Core.EntityBase
{
    public abstract class UserBase
    {
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [RegularExpression(@"^[689]\d{7}$", ErrorMessage = "Invalid Singapore phone number.")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid Singapore postal code.")]
        public string? ZipCode { get; set; }
    }
}
