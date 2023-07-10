
using Task_Management.Models.Entities.Projects;

namespace Task_Management.Respositories.Projects
{
    public interface IProjectRepository
    {
        Task InsertProjectAsync(Project project);
        IQueryable<Project> SelectAllProjects();
        Task<Project> SelectProjectByIdAsync(Guid id);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Project project);
    }
}
