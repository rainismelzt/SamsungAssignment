using UserService.Core.Dto;
using UserService.Database.BusinessEntity;

namespace UserService.Core.Interface
{
    public interface IUserDataRepository
    {
        Task CreateUser(UserData user);
        Task UpdateUser(UserData user);
        Task DeleteUser(long id);
        Task<(List<UserData>, int)> GetUserList(GetUserListRequestDto request);
        Task<UserData> GetUserDetails(long id);
    }
}
