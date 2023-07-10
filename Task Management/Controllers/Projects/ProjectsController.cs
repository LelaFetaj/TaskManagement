using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Net;
using Task_Management.Models.DTOs.Projects;
using Task_Management.Models.DTOs.Tasks;
using Task_Management.Models.Entities.ErrorResponse;
using Task_Management.Models.Entities.Projects;
using Task_Management.Services.Foundations.Projects;
using Task_Management.Services.Orchestration.Projects;

namespace Task_Management.Controllers.Projects
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectOrchestrationService projectOrchestrationService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            IProjectOrchestrationService projectOrchestrationService, 
            ILogger<ProjectsController> logger)
        {
            this.projectOrchestrationService=projectOrchestrationService;
            _logger=logger;
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddProject(ProjectDto projectDto)
        {
            try
            {
                (bool result, string message) = 
                    await projectOrchestrationService.AddProjectAsync(projectDto);

                if (result)
                {
                    return Ok(message);
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a project.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };      

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost("assign-task-to-project")]
        public async Task<ActionResult<string>> AssignTaskToProjectAsync(UpdateTaskRequest updateTaskRequest)
        {
            try
            {
                await projectOrchestrationService.AssignTaskToProjectAsync(
                    updateTaskRequest.Id, 
                    updateTaskRequest.ProjectId ?? Guid.Empty);

                return Ok("Task assigned to the proejct successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning the task to the project.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Project>>> GetAllProjects()
        {
            try
            {
                List<Project> projects = 
                    await projectOrchestrationService.RetrieveAllProjectsAsync();

                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all projects.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(Guid id)
        {
            try
            {
                Project project = 
                    await projectOrchestrationService.RetrieveProjectByIdAsync(id);

                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving project with id: {id}");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateProject(ProjectDto projectDto)
        {
            try
            {
                (bool result, string message) = 
                    await projectOrchestrationService.UpdateProjectAsync(projectDto);

                if (result)
                {
                    return Ok(message);
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a project.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteProject(Guid id)
        {
            try
            {
                (bool result, string message) = 
                    await projectOrchestrationService.DeleteProjectByIdAsync(id);

                if (result)
                {
                    return Ok(message);
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting project with id: {id}");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }
}
