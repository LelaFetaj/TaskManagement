using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http.ModelBinding;
using Task_Management.Models.DTOs.LogIn;
using Task_Management.Models.DTOs.Register;
using Task_Management.Models.Entities.Users;
using Task_Management.Models.Exceptions;
using Task_Management.Services.Foundations.SignIn;
using Task_Management.Services.Foundations.Users;

namespace Task_Management.Services.Orchestration.Users
{
    public class UserOrchestrationService : IUserOrchestrationService
    {
        private readonly IUserService userService;
        private readonly ISignInService signInService;
        private readonly string privateKey;
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$";

        public UserOrchestrationService(
            IUserService userService,
            ISignInService signInService,
            IConfiguration configuration)
        {
            this.userService = userService;
            this.signInService = signInService;
            this.privateKey=configuration["JwtConfiguration:Key"] ?? string.Empty;
        }

        public async ValueTask<string> UserRegisterAsync(RegisterDto registerDto)
        {
            if (registerDto is null ||
                string.IsNullOrWhiteSpace(registerDto.Username) ||
                string.IsNullOrWhiteSpace(registerDto.Password))
            {
                throw new ValidateException("Invalid username or password");
            }

            if (!MailAddress.TryCreate(registerDto.Username, out _))
            {
                throw new ValidateException("Username should be a valid email address");
            }

            if (!Regex.IsMatch(registerDto.Password, PasswordPattern))
            {
                throw new ValidateException("Password must meet the following requirements:\n" +
                    "- At least 8 characters\n" +
                    "- Contains at least one lowercase letter\n" +
                    "- Contains at least one uppercase letter\n" +
                    "- Contains at least one digit\n" +
                    "- Contains at least one special character");
            }

            var storageUser =
                await userService.RetreiveUserByUserNameAsync(registerDto.Username);

            if (storageUser is not null)
            {
                throw new ValidateException($"Username {registerDto.Username} is taken");
            }

            var newUser = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Username
            };

            var user =
                await userService.RegisterUserAsync(newUser, registerDto.Password);


            return CreateToken(user);
        }

        public async ValueTask<string> UserLoginAsync(LogInDto logInDto)
        {
            if (logInDto is null || 
                string.IsNullOrWhiteSpace(logInDto.Username) || 
                string.IsNullOrWhiteSpace(logInDto.Password))
            {
                throw new ValidateException("Invalid username or password");
            }

            var user = await userService.RetreiveUserByUserNameAsync(
                logInDto.Username);

            if (user is null)
            {
                throw new ValidateException($"User with username: {logInDto.Username} does not exist");
            }

            await signInService.CheckPasswordSignInAsync(user, logInDto.Password);


            return CreateToken(user);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.UserName),
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(privateKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
