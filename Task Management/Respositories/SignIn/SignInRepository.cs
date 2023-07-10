using Microsoft.AspNetCore.Identity;
using Task_Management.Models.Entities.Users;

namespace Task_Management.Respositories.SignIn
{
    public class SignInRepository : ISignInRepository
    {
        private readonly SignInManager<User> signInManagement;

        public SignInRepository(SignInManager<User> signInManagement)
        {
            this.signInManagement = signInManagement;
        }

        public async ValueTask<SignInResult> CheckPasswordSignInAsync(User user, string password)
        {
            return await this.signInManagement.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }
    }
}
