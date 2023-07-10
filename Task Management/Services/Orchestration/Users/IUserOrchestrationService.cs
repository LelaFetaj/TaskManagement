using Task_Management.Models.DTOs.LogIn;
using Task_Management.Models.DTOs.Register;

namespace Task_Management.Services.Orchestration.Users
{
    public interface IUserOrchestrationService
    {
        ValueTask<string> UserRegisterAsync(RegisterDto registerDto);
        ValueTask<string> UserLoginAsync(LogInDto logInDto);
    }
}
