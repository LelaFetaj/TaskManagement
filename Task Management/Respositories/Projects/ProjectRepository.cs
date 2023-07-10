using Task_Management.Data.Context;
using Task_Management.Models.Entities.Projects;

namespace Task_Management.Respositories.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskManagementDbContext _dbContext;

        public ProjectRepository(TaskManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertProjectAsync(Project project)
        {
            await _dbContext.Project.AddAsync(project);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Project> SelectAllProjects() =>
           _dbContext.Project;

        public async Task<Project> SelectProjectByIdAsync(Guid id) =>
            await _dbContext.Project.FindAsync(id);

        public async Task UpdateProjectAsync(Project project)
        {
            _dbContext.Project.Update(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(Project project)
        {
            _dbContext.Project.Remove(project);
            await _dbContext.SaveChangesAsync();
        }
    }
}
