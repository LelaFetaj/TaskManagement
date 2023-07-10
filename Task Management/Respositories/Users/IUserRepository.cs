
using Microsoft.AspNetCore.Identity;
using Task_Management.Models.Entities.Users;

namespace Task_Management.Respositories.Users
{
    public interface IUserRepository
    {
        ValueTask<IdentityResult> InsertUserAsync(User user, string password);
        ValueTask<User> SelectUserByUserNameAsync(string username);
        ValueTask<User> SelectUserByIdAsync(Guid? id);
    }
}
