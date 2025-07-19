using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                return JsonConvert.SerializeObject("User has been created successfully.");
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserRequestDto request)
        {
            return await CommonControllerFlow(async () => {
                await _userService.UpdateUser(request);
                return JsonConvert.SerializeObject("User has been updated successfully.");
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] long id)
        {
            return await CommonControllerFlow(async () => {
                await _userService.DeleteUser(id);
                return JsonConvert.SerializeObject("User has been deleted successfully.");
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
                // Return ProblemDetails object for proper error handling
                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "An error occurred while processing your request.",
                    Detail = ex.Message
                };
                return StatusCode(500, problemDetails);
            }
        }
    }
}
