using Task_Management.Models.DTOs.Projects;
using Task_Management.Models.Entities.Projects;

namespace Task_Management.Services.Orchestration.Projects
{
    public interface IProjectOrchestrationService
    {
        Task<(bool, string)> AddProjectAsync(ProjectDto projectDto);

        Task<List<Project>> RetrieveAllProjectsAsync();

        Task<Project> RetrieveProjectByIdAsync(Guid id);

        Task<(bool, string)> UpdateProjectAsync(ProjectDto projectDto);

        Task<(bool, string)> DeleteProjectByIdAsync(Guid id);
        Task AssignTaskToProjectAsync(Guid taskId, Guid projectId);
    }
}
