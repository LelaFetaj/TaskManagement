using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task_Management.Data.Context;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Models.Entities.Users;

namespace Task_Management.Respositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> userManagement;

        public UserRepository(UserManager<User> userManagement)
        {
            this.userManagement = userManagement;
        }

        public async ValueTask<IdentityResult> InsertUserAsync(User user, string password)
        {
            var broker = new UserRepository(this.userManagement);

            return await broker.userManagement.CreateAsync(user, password);
        }

        public async ValueTask<User> SelectUserByUserNameAsync(string username)
        {
            var broker = new UserRepository(this.userManagement);

            return await broker.userManagement.FindByNameAsync(username);
        }

        public async ValueTask<User> SelectUserByIdAsync(Guid? id)
        {
            var broker = new UserRepository(this.userManagement);
            return await broker.userManagement.FindByIdAsync(id.ToString());
        }
            
    }
}
