using Budget_Tracker_WebAPI.DTOs;
using Budget_Tracker_WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget_Tracker_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService _authService)
        {
            this.authService =  _authService;
        }

        [HttpGet]
        public IActionResult GetUserDetail(Guid userId)
        {
            try
            {
                var user = authService.GetUserDetail(userId);

                if (user == null)
                {
                    var err_response = new ResponseModel<object>("User not found", StatusCodes.Status404NotFound);
                    return NotFound(err_response);
                }

                var response = new ResponseModel<UserDetailModel>(user, "User retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Register( RegisterUserDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var responses = new ResponseModel<object>("Validation failed", StatusCodes.Status400BadRequest);
                    return Ok(responses); // Return a BadRequest with the response object
                }
                await authService.ResgisterUserAsync(dto);
                // Return success response with a message
                var response = new ResponseModel<object>(null, "User registered successfully", 201);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return error response with 400 Bad Request status code
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login( LoginUserDTO dto)
        {
            try
            {

                var token = await authService.LoginUserAsync(dto);
                var response = new ResponseModel<object>(token, "User registered successfully", 201);
                return Ok(response);

            }
            catch (Exception ex)
            {
                // Return error response with 400 Bad Request status code
                var response = new ResponseModel<object>(ex.Message, 400);
                return StatusCode(response.StatusCode, response);
            }
        }

    }

}
