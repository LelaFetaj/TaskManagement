using SendGrid.Helpers.Errors.Model;
using Task_Management.Models.DTOs.Projects;
using Task_Management.Models.DTOs.Tasks;
using Task_Management.Models.Entities.Projects;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Services.Foundations.Projects;
using Task_Management.Services.Foundations.Tasks;
using Task_Management.Services.Foundations.UserTaks;

namespace Task_Management.Services.Orchestration.Projects
{
    public class ProjectOrchestrationService : IProjectOrchestrationService
    {
        private readonly ITaskService taskService;
        private readonly IProjectService projectService;

        public ProjectOrchestrationService(ITaskService taskService,
            IProjectService projectService)
        {
            this.taskService = taskService;
            this.projectService = projectService;
        }

        public async Task<(bool, string)> AddProjectAsync(ProjectDto projectDto) =>
            await this.projectService.AddProjectAsync(projectDto);

        public async Task AssignTaskToProjectAsync(Guid taskId, Guid projectId)
        {
            // Retrieve the task and project details
            TaskEntity task = await taskService.RetrieveTaskById(taskId);
            Project project = await projectService.RetrieveProjectByIdAsync(projectId);

            if (task == null || project == null)
            {
                throw new NotFoundException("Task or Project not found");
            }

            // Assign the task to the project
            task.ProjectId = projectId;
            await taskService.UpdateTaskAsync(new UpdateTaskRequest
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ProjectId = projectId
            });
        }

        public async Task<(bool, string)> DeleteProjectByIdAsync(Guid id) =>
            await this.projectService.DeleteProjectByIdAsync(id);

        public async Task<List<Project>> RetrieveAllProjectsAsync() =>
            await this.projectService.RetrieveAllProjectsAsync();

        public async Task<Project> RetrieveProjectByIdAsync(Guid id) =>
            await this.projectService.RetrieveProjectByIdAsync(id);

        public async Task<(bool, string)> UpdateProjectAsync(ProjectDto projectDto)=>
            await this.projectService.UpdateProjectAsync(projectDto);
    }
}
