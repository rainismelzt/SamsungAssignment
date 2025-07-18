using UserService.Core.Dto;

namespace UserService.Core.Interface
{
    public interface IUserDataService
    {
        Task CreateUser(CreateUserRequestDto request);
        Task UpdateUser(UpdateUserRequestDto request);
        Task DeleteUser(long id);
        Task<GetUserListResponseDto> GetUserList(GetUserListRequestDto request);
        Task<UserDetailsResponseDto> GetUserDetails(long id);
    }
}
