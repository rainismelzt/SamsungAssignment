using System.ComponentModel.DataAnnotations;
using UserService.Core.EntityBase;

namespace UserService.Core.Dto
{
    public class UserDetailsResponseDto : UserBase
    {
        [Required]
        public long Id { get; set; }
    }
}
