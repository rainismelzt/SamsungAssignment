using AutoMapper;
using Moq;
using UserService.Core.Dto;
using UserService.Core.Interface;
using UserService.Database.BusinessEntity;
using UserService.Infrastructure.Service;

namespace UserService.Test.Infrastructure.Service
{
    public class UserDataServiceTest
    {
        private readonly Mock<IUserDataRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserDataService _service;

        public UserDataServiceTest()
        {
            _userRepositoryMock = new Mock<IUserDataRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new UserDataService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateUser_CallsRepositoryWithMappedUser()
        {
            var request = new CreateUserRequestDto();
            var userData = new UserData();
            _mapperMock.Setup(m => m.Map<UserData>(request)).Returns(userData);

            await _service.CreateUser(request);

            _userRepositoryMock.Verify(r => r.CreateUser(userData), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_CallsRepositoryWithMappedUser()
        {
            var request = new UpdateUserRequestDto();
            var userData = new UserData();
            _mapperMock.Setup(m => m.Map<UserData>(request)).Returns(userData);

            await _service.UpdateUser(request);

            _userRepositoryMock.Verify(r => r.UpdateUser(userData), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_CallsRepositoryWithId()
        {
            long id = 42;

            await _service.DeleteUser(id);

            _userRepositoryMock.Verify(r => r.DeleteUser(id), Times.Once);
        }

        [Fact]
        public async Task GetUserList_ReturnsMappedResponse()
        {
            var request = new GetUserListRequestDto();
            var userList = new List<UserData> { new UserData { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com" } };
            int totalCount = 1;
            var mappedList = new List<UserDetailsResponseDto> { new UserDetailsResponseDto { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com" } };

            _userRepositoryMock.Setup(r => r.GetUserList(request)).ReturnsAsync((userList, totalCount));
            _mapperMock.Setup(m => m.Map<List<UserDetailsResponseDto>>(userList)).Returns(mappedList);

            var result = await _service.GetUserList(request);

            Assert.NotNull(result);
            Assert.Equal(mappedList, result.UserList);
            Assert.Equal(totalCount, result.TotalCount);
        }

        [Fact]
        public async Task GetUserDetails_ReturnsMappedUser()
        {
            long id = 1;
            var userData = new UserData { Id = id, FirstName = "A", LastName = "B", Email = "a@b.com" };
            var mapped = new UserDetailsResponseDto { Id = id, FirstName = "A", LastName = "B", Email = "a@b.com" };

            _userRepositoryMock.Setup(r => r.GetUserDetails(id)).ReturnsAsync(userData);
            _mapperMock.Setup(m => m.Map<UserDetailsResponseDto>(userData)).Returns(mapped);

            var result = await _service.GetUserDetails(id);

            Assert.NotNull(result);
            Assert.Equal(mapped, result);
        }

        [Fact]
        public async Task CreateUser_Throws_WhenRepositoryThrows()
        {
            var request = new CreateUserRequestDto();
            var userData = new UserData();
            _mapperMock.Setup(m => m.Map<UserData>(request)).Returns(userData);
            _userRepositoryMock.Setup(r => r.CreateUser(userData)).ThrowsAsync(new Exception("fail"));

            await Assert.ThrowsAsync<Exception>(() => _service.CreateUser(request));
        }

        [Fact]
        public async Task UpdateUser_Throws_WhenRepositoryThrows()
        {
            var request = new UpdateUserRequestDto();
            var userData = new UserData();
            _mapperMock.Setup(m => m.Map<UserData>(request)).Returns(userData);
            _userRepositoryMock.Setup(r => r.UpdateUser(userData)).ThrowsAsync(new Exception("fail"));

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateUser(request));
        }

        [Fact]
        public async Task DeleteUser_Throws_WhenRepositoryThrows()
        {
            long id = 1;
            _userRepositoryMock.Setup(r => r.DeleteUser(id)).ThrowsAsync(new Exception("fail"));

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteUser(id));
        }

        [Fact]
        public async Task GetUserList_Throws_WhenRepositoryThrows()
        {
            var request = new GetUserListRequestDto();
            _userRepositoryMock.Setup(r => r.GetUserList(request)).ThrowsAsync(new Exception("fail"));

            await Assert.ThrowsAsync<Exception>(() => _service.GetUserList(request));
        }

        [Fact]
        public async Task GetUserDetails_Throws_WhenRepositoryThrows()
        {
            long id = 1;
            _userRepositoryMock.Setup(r => r.GetUserDetails(id)).ThrowsAsync(new Exception("fail"));

            await Assert.ThrowsAsync<Exception>(() => _service.GetUserDetails(id));
        }
    }
}