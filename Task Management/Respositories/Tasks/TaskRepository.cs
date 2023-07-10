using Microsoft.EntityFrameworkCore;
using Task_Management.Data.Context;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Models.Entities.UserTasks;
using static Task_Management.Respositories.Tasks.TaskRepository;

namespace Task_Management.Respositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext dbContext;

        public TaskRepository(TaskManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task InsertTaskAsync(TaskEntity taskEntity)
        {
            await dbContext.TaskEntity.AddAsync(taskEntity);
            await dbContext.SaveChangesAsync();
        }

        public IQueryable<TaskEntity> SelectAllTasks() =>
           dbContext.TaskEntity;

        public async Task<TaskEntity> SelectTaskByIdAsync(Guid id) =>
            await dbContext.TaskEntity.FindAsync(id);

        public async Task UpdateTaskAsync(TaskEntity taskEntity)
        {
            dbContext.TaskEntity.Update(taskEntity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(TaskEntity taskEntity)
        {
            dbContext.TaskEntity.Remove(taskEntity);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddTaskAssignment(UserTask userTask)
        {
            await dbContext.UserTasks.AddAsync(userTask);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<TaskEntity>> GetUpcomingTasksAsync()
        {
            var upcomingTasks = await dbContext.TaskEntity
                .Where(taskentity => taskentity.DueDate > DateTimeOffset.UtcNow)
                .ToListAsync();

            return upcomingTasks;
        }

    }
}
