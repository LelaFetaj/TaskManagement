using Task_Management.Models.Entities.Users;

namespace Task_Management.Services.Foundations.SignIn
{
    public interface ISignInService
    {
        ValueTask<bool> CheckPasswordSignInAsync(User user, string password);
    }
}
