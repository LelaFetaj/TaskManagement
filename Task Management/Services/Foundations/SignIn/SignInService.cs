using Task_Management.Models.Entities.Users;
using Task_Management.Models.Exceptions;
using Task_Management.Respositories.SignIn;

namespace Task_Management.Services.Foundations.SignIn
{
    public class SignInService : ISignInService
    {
        private readonly ISignInRepository signInRepository;

        public SignInService(ISignInRepository signInRepository)
        {
            this.signInRepository = signInRepository;
        }

        public async ValueTask<bool> CheckPasswordSignInAsync(User user, string password)
        {
            var result = await signInRepository.CheckPasswordSignInAsync(
                user,
                password);

            if (!result.Succeeded)
            {
                throw new ValidateException("password was incorrect");
            }

            return result.Succeeded;
        }
    }
}
