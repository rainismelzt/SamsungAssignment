namespace UserService.Core.Dto
{
    public class GetUserListResponseDto
    {
        public List<UserDetailsResponseDto> UserList { get; set; } = [];

        public int TotalCount { get; set; }
    }
}
