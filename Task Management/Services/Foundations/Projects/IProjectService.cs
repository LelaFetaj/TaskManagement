using Task_Management.Models.DTOs.Projects;
using Task_Management.Models.Entities.Projects;

namespace Task_Management.Services.Foundations.Projects
{
    public interface IProjectService
    {
        Task<(bool, string)> AddProjectAsync(ProjectDto projectDto);

        Task<List<Project>> RetrieveAllProjectsAsync();

        Task<Project> RetrieveProjectByIdAsync(Guid id);

        Task<(bool, string)> UpdateProjectAsync(ProjectDto projectDto);

        Task<(bool, string)> DeleteProjectByIdAsync(Guid id);
    }
}
