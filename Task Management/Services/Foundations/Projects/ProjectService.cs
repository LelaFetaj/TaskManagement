using Microsoft.EntityFrameworkCore;
using Task_Management.Models.DTOs.Projects;
using Task_Management.Models.Entities.Projects;
using Task_Management.Respositories.Projects;

namespace Task_Management.Services.Foundations.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<(bool, string)> AddProjectAsync(ProjectDto projectDto)
        {
            bool exists = await projectRepository
                .SelectAllProjects()
                .AnyAsync(x => x.Title == projectDto.Title);

            if (exists)
            {
                return (false, "A project with the same title exists. Please choose another title!");
            }

            Project project = new Project
            {
                Title = projectDto.Title,
                Description = projectDto.Description,
                StartDate= projectDto.StartDate,
                EndDate= projectDto.EndDate
            };

            await projectRepository.InsertProjectAsync(project);

            return (true, "Success");
        }

        public async Task<List<Project>> RetrieveAllProjectsAsync()
        {
            List<Project> projects = await projectRepository
                .SelectAllProjects()
                .ToListAsync();

            return projects;
        }

        public async Task<Project> RetrieveProjectByIdAsync(Guid id) =>
            await projectRepository.SelectProjectByIdAsync(id);

        public async Task<(bool, string)> UpdateProjectAsync(ProjectDto projectDto)
        {
            Project project = await projectRepository.SelectProjectByIdAsync(projectDto.Id);

            if (project == null)
            {
                return (false, $"Couldn't find project with id: {projectDto.Id}");
            }

            bool exists = await projectRepository
                .SelectAllProjects()
                .AnyAsync(x => x.Title == projectDto.Title && x.Id != projectDto.Id);

            if (exists)
            {
                return (false, "An project with the same title exists. Please choose another title!");
            }

            project.Title = projectDto.Title;
            project.Description = projectDto.Description;
            project.StartDate = projectDto.StartDate;
            project.EndDate = projectDto.EndDate;

            await projectRepository.UpdateProjectAsync(project);

            return (true, "Success");
        }

        public async Task<(bool, string)> DeleteProjectByIdAsync(Guid id)
        {
            Project project = await projectRepository.SelectProjectByIdAsync(id);

            if (project == null)
            {
                return (false, $"Couldn't find project with id: {id}");
            }

            await projectRepository.DeleteProjectAsync(project);

            return (true, "Success");
        }
    }
}
