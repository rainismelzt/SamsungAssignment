using Microsoft.AspNetCore.Mvc;
using UserService.Core.Dto;
using UserService.Core.Interface;

namespace UserService.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserDataService _userService;

        public UserController(IUserDataService userService) { 
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestDto request)
        {
            return await CommonControllerFlow(async() => { 
                await _userService.CreateUser(request);
                return "User has been created successfully.";
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserRequestDto request)
        {
            return await CommonControllerFlow(async () => {
                await _userService.UpdateUser(request);
                return "User has been updated successfully.";
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(long id)
        {
            return await CommonControllerFlow(async () => {
                await _userService.DeleteUser(id);
                return "User has been deleted successfully.";
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList([FromQuery] GetUserListRequestDto request)
        {
            return await CommonControllerFlow(async () => {
                var response = await _userService.GetUserList(request);
                return response;
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDetails([FromQuery] long id)
        {
            return await CommonControllerFlow(async () => {
                var response = await _userService.GetUserDetails(id);
                return response;
            });
        }

        private async Task<IActionResult> CommonControllerFlow<T>(Func<Task<T>> serviceCall)
        {
            try
            {
                T response = await serviceCall();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
