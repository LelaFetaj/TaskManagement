using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SendGrid.Helpers.Errors.Model;
using Task_Management.Models.DTOs.Tasks;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Respositories.Categories;
using Task_Management.Respositories.Projects;
using Task_Management.Respositories.Tasks;
using Task_Management.Respositories.Users;

namespace Task_Management.Services.Foundations.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<(bool, string)> AddTaskAsync(CreateTaskRequest createtaskRequest)
        {
            bool exists = await taskRepository
                .SelectAllTasks()
                .AnyAsync(x => x.Title == createtaskRequest.Title);

            if (exists)
            {
                return (false, "A task with the same title exists. Please choose another title!");
            }

            TaskEntity taskEntity = new TaskEntity
            {
                Title = createtaskRequest.Title,
                Description = createtaskRequest.Description,
                DueDate = createtaskRequest.DueDate,
                Status = createtaskRequest.Status,
                Priority = createtaskRequest.Priority,
                Urgency = createtaskRequest.Urgency,
                CategoryId = createtaskRequest.CategoryId,
                ProjectId = createtaskRequest.ProjectId
            };

            await taskRepository.InsertTaskAsync(taskEntity);

            return (true, "Success");
        }

        public async Task<List<TaskEntity>> RetrieveAllTasks()
        {
            List<TaskEntity> tasksEntity = await taskRepository
                .SelectAllTasks()
                .ToListAsync();

            return tasksEntity;
        }

        public async Task<TaskEntity> RetrieveTaskById(Guid id) =>
            await taskRepository.SelectTaskByIdAsync(id);

        public async Task<(bool, string)> UpdateTaskAsync(UpdateTaskRequest updateTaskRequest)
        {
            TaskEntity taskEntity = await taskRepository.SelectTaskByIdAsync(updateTaskRequest.Id);

            if (taskEntity == null)
            {
                return (false, $"Couldn't find task with id: {updateTaskRequest.Id}");
            }

            taskEntity.Title = updateTaskRequest.Title ?? taskEntity.Title;
            taskEntity.Description = updateTaskRequest.Description ?? taskEntity.Description;
            taskEntity.DueDate = updateTaskRequest.DueDate ?? taskEntity.DueDate;
            taskEntity.Priority = updateTaskRequest.Priority ?? taskEntity.Priority;
            taskEntity.Urgency = updateTaskRequest.Urgency ?? taskEntity.Urgency;
            taskEntity.Status = updateTaskRequest.Status ?? taskEntity.Status;
            taskEntity.CategoryId = updateTaskRequest.CategoryId ?? taskEntity.CategoryId;
            taskEntity.ProjectId = updateTaskRequest.ProjectId ?? taskEntity.ProjectId;
            taskEntity.Progress = updateTaskRequest.Progress ?? taskEntity.Progress;

            await taskRepository.UpdateTaskAsync(taskEntity);

            return (true, "Success");
        }

        public async Task<(bool, string)> DeleteTaskByIdAsync(Guid id)
        {
            TaskEntity taskEntity = await taskRepository.SelectTaskByIdAsync(id);

            if (taskEntity == null)
            {
                return (false, $"Couldn't find task with id: {id}");
            }

            await taskRepository.DeleteTaskAsync(taskEntity);

            return (true, "Success");
        }

        public async Task<List<UpdateTaskRequest>> GetFilteredTasksAsync(TaskFilterCriteria criteria)
        {
            // Apply filters based on the criteria provided
            IQueryable<TaskEntity> query = taskRepository.SelectAllTasks();

            if (!string.IsNullOrEmpty(criteria.Keyword) || criteria.CategoryId.HasValue || criteria.ProjectId.HasValue || criteria.Status.HasValue || criteria.Priority.HasValue || criteria.Urgency.HasValue)
            {
                query = query.Where(t =>
                    (string.IsNullOrEmpty(criteria.Keyword) || t.Title.Contains(criteria.Keyword) || t.Description.Contains(criteria.Keyword)) ||
                    (criteria.AssignedUserId.HasValue && t.AssignedUserId == criteria.AssignedUserId.Value) ||
                    (criteria.CategoryId.HasValue && t.CategoryId == criteria.CategoryId.Value) ||
                    (criteria.ProjectId.HasValue && t.ProjectId == criteria.ProjectId.Value) ||
                    (criteria.Status.HasValue && t.Status == criteria.Status.Value) ||
                    (criteria.Priority.HasValue && t.Priority == criteria.Priority.Value) ||
                    (criteria.Urgency.HasValue && t.Urgency == criteria.Urgency.Value)
                );
            }

            // Execute the query and map the results to DTOs
            List<UpdateTaskRequest> tasks = await query.Select(t => new UpdateTaskRequest
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
            }).ToListAsync();

            return tasks;
        }

        public async Task<List<UpdateTaskRequest>> GetUpcomingTasksAsync()
        {
            var upcomingTasks = await taskRepository.GetUpcomingTasksAsync();

            var taskDtos = upcomingTasks.Select(task => new UpdateTaskRequest
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
            }).ToList();

            return taskDtos;
        }
    }
}
