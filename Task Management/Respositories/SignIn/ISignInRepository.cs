using Microsoft.AspNetCore.Identity;
using Task_Management.Models.Entities.Users;

namespace Task_Management.Respositories.SignIn
{
    public interface ISignInRepository
    {
        ValueTask<SignInResult> CheckPasswordSignInAsync(User user, string password);
    }
}
