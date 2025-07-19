using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using UserService.Core.Dto;
using UserService.Core.Interface;
using UserService.Presentation.Controllers;

namespace UserService.Presentation.Tests.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserDataService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _userServiceMock = new Mock<IUserDataService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task CreateUser_ReturnsOk_WhenSuccess()
        {
            var request = new CreateUserRequestDto();
            _userServiceMock.Setup(s => s.CreateUser(request)).Returns(Task.CompletedTask);

            var result = await _controller.CreateUser(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(JsonConvert.SerializeObject("User has been created successfully."), okResult.Value);
        }

        [Fact]
        public async Task CreateUser_ReturnsProblem_WhenException()
        {
            var request = new CreateUserRequestDto();
            _userServiceMock.Setup(s => s.CreateUser(request)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.CreateUser(request);

            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, problemResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Contains("fail", problemDetails.Detail);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOk_WhenSuccess()
        {
            var request = new UpdateUserRequestDto { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com" };
            _userServiceMock.Setup(s => s.UpdateUser(request)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateUser(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(JsonConvert.SerializeObject("User has been updated successfully."), okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsProblem_WhenException()
        {
            var request = new UpdateUserRequestDto { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com" };
            _userServiceMock.Setup(s => s.UpdateUser(request)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.UpdateUser(request);

            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, problemResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Contains("fail", problemDetails.Detail);
        }

        [Fact]
        public async Task DeleteUser_ReturnsOk_WhenSuccess()
        {
            long id = 1;
            _userServiceMock.Setup(s => s.DeleteUser(id)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteUser(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(JsonConvert.SerializeObject("User has been deleted successfully."), okResult.Value);
        }

        [Fact]
        public async Task DeleteUser_ReturnsProblem_WhenException()
        {
            long id = 1;
            _userServiceMock.Setup(s => s.DeleteUser(id)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.DeleteUser(id);

            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, problemResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Contains("fail", problemDetails.Detail);
        }

        [Fact]
        public async Task GetUserList_ReturnsOk_WhenSuccess()
        {
            var request = new GetUserListRequestDto();
            var response = new GetUserListResponseDto();
            _userServiceMock.Setup(s => s.GetUserList(request)).ReturnsAsync(response);

            var result = await _controller.GetUserList(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task GetUserList_ReturnsProblem_WhenException()
        {
            var request = new GetUserListRequestDto();
            _userServiceMock.Setup(s => s.GetUserList(request)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.GetUserList(request);

            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, problemResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Contains("fail", problemDetails.Detail);
        }

        [Fact]
        public async Task GetUserDetails_ReturnsOk_WhenSuccess()
        {
            long id = 1;
            var response = new UserDetailsResponseDto { Id = id, FirstName = "A", LastName = "B", Email = "a@b.com" };
            _userServiceMock.Setup(s => s.GetUserDetails(id)).ReturnsAsync(response);

            var result = await _controller.GetUserDetails(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task GetUserDetails_ReturnsProblem_WhenException()
        {
            long id = 1;
            _userServiceMock.Setup(s => s.GetUserDetails(id)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.GetUserDetails(id);

            var problemResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, problemResult.StatusCode);

            var problemDetails = Assert.IsType<ProblemDetails>(problemResult.Value);
            Assert.Contains("fail", problemDetails.Detail);
        }
    }
}