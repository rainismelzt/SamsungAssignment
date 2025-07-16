using System.ComponentModel.DataAnnotations;
using UserService.Core.EntityBase;

namespace UserService.Core.Dto
{
    public class UpdateUserRequestDto : UserBase
    {
        [Required]
        public long Id { get; set; }
    }
}
