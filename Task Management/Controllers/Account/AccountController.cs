using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_Management.Models.DTOs.LogIn;
using Task_Management.Models.DTOs.Register;
using Task_Management.Models.Entities.ErrorResponse;
using Task_Management.Models.Exceptions;
using Task_Management.Services.Orchestration.Users;

namespace Task_Management.Controllers.Account
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserOrchestrationService userOrchestrationService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IUserOrchestrationService userOrchestrationService,
            ILogger<AccountController> _logger)
        {
            this.userOrchestrationService = userOrchestrationService;
            this._logger = _logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto registerDto)
        {
            try
            {
                string result = await userOrchestrationService.UserRegisterAsync(registerDto);
                return Ok(result);
            }
            catch(ValidateException validateException)
            {
                _logger.LogError(validateException.Message);

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = ((int)HttpStatusCode.BadRequest).ToString(),
                    ErrorMessage = validateException.Message
                };

                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }


        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate(LogInDto logInDto)
        {
            try
            {
                string result = await userOrchestrationService.UserLoginAsync(logInDto);
                return Ok(result);
            }
            catch (ValidateException validateException)
            {
                _logger.LogError(validateException.Message);

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = ((int)HttpStatusCode.BadRequest).ToString(),
                    ErrorMessage = validateException.Message
                };

                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating the user.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

    }
}
