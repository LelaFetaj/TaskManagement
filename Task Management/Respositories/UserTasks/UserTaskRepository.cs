using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Task_Management.Data.Context;
using Task_Management.Models.Entities.UserTasks;

namespace Task_Management.Respositories.UserTasks
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly TaskManagementDbContext dbContext;

        public UserTaskRepository(TaskManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async ValueTask<UserTask> InsertUserTaskAsync(UserTask userTask)
        {
            EntityEntry<UserTask> entityEntry =
                await this.dbContext.UserTasks.AddAsync(userTask);

            await this.dbContext.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask<List<UserTask>> SelectAllUserTasksAsync(Guid userId)
        {
            return await this.dbContext.UserTasks
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async ValueTask<UserTask> SelectUserTaskByIdAsync(Guid userId, Guid taskId)
        {
            return await this.dbContext.UserTasks.FindAsync(userId, taskId);
        }

        public async ValueTask<UserTask> DeleteUserTaskAsync(UserTask userTask)
        {
            EntityEntry<UserTask> entityEntry =
                this.dbContext.UserTasks.Remove(userTask);

            await this.dbContext.SaveChangesAsync();

            return entityEntry.Entity;
        }
    }
}
