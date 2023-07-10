using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_Management.Models.Entities.Users;
using Task_Management.Respositories.Users;

namespace Task_Management.Services.Foundations.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async ValueTask<User> RegisterUserAsync(User user, string password)
        {
            if (user is null || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Invalid user parameters");
            }

            var result = await userRepository.InsertUserAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception(result.ToString());
            }

            return user;
        }

        public async ValueTask<User> RetreiveUserByUserNameAsync(string username) =>
            await userRepository.SelectUserByUserNameAsync(username.ToLower());

        public async ValueTask<User> RetreiveUserByIdAsync(Guid userId) =>
           await userRepository.SelectUserByIdAsync(userId);
    }
}
