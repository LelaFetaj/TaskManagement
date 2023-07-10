using Task_Management.Models.Entities.Users;

namespace Task_Management.Services.Foundations.Users
{
    public interface IUserService
    {
        ValueTask<User> RegisterUserAsync(User user, string password);
        ValueTask<User> RetreiveUserByUserNameAsync(string username);
        ValueTask<User> RetreiveUserByIdAsync(Guid userId);
    }
}
