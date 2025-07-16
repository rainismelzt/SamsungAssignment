using AutoMapper;
using UserService.Core.Dto;
using UserService.Core.Interface;
using UserService.Database.BusinessEntity;

namespace UserService.Infrastructure.Service
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserDataRepository _userRepository;
        private readonly IMapper _mapper;

        public UserDataService(IUserDataRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateUser(CreateUserRequestDto request)
        {
            var user = _mapper.Map<UserData>(request);
            await _userRepository.CreateUser(user);
        }

        public async Task UpdateUser(UpdateUserRequestDto request)
        {
            var user = _mapper.Map<UserData>(request);
            await _userRepository.UpdateUser(user);
        }

        public async Task DeleteUser(long id)
        {
            await _userRepository.DeleteUser(id);
        }

        public async Task<GetUserListResponseDto> GetUserList(GetUserListRequestDto request)
        {
            var (users, totalCount) = await _userRepository.GetUserList(request);
            return new GetUserListResponseDto
            {
                UserList = _mapper.Map<List<UserDetailsResponseDto>>(users),
                TotalCount = totalCount
            };
        }

        public async Task<UserDetailsResponseDto> GetUserDetails(long id)
        {
            var user = await _userRepository.GetUserDetails(id);
            return _mapper.Map<UserDetailsResponseDto>(user);
        }
    }
}
