using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Net;
using Task_Management.Models.DTOs.Tasks;
using Task_Management.Models.Entities.ErrorResponse;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Models.Entities.UserTasks;
using Task_Management.Services.Orchestration.UserTasks;

namespace Task_Management.Controllers.Tasks
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IUserTaskOrchestrationService userTaskOrchestrationService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(
            IUserTaskOrchestrationService userTaskOrchestrationService,
            ILogger<TasksController> _logger)
        {
            this.userTaskOrchestrationService = userTaskOrchestrationService;
            this._logger = _logger;
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddTask(CreateTaskRequest createTaskRequest)
        {
            try
            {
                (bool result, string message) =
                    await userTaskOrchestrationService.AddTaskAsync(createTaskRequest);

                if (result)
                {
                    return Ok(message);
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a task.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskEntity>>> GetAllTasks()
        {
            try
            {
                List<TaskEntity> taskEntities =
                    await userTaskOrchestrationService.RetrieveAllTasks();

                return Ok(taskEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving tasks.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred while retrieving tasks. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskEntity>> GetTask(Guid id)
        {
            try
            {
                TaskEntity taskEntity =
                    await userTaskOrchestrationService.RetrieveTaskById(id);

                return Ok(taskEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the task.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred while retrieving the task. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateTask(UpdateTaskRequest updateTaskRequest)
        {
            try
            {
                (bool result, string message) =
                    await userTaskOrchestrationService.UpdateTaskAsync(updateTaskRequest);

                if (result)
                {
                    return Ok(message);
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the task.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteTask(Guid id)
        {
            try
            {
                (bool result, string message) =
                    await userTaskOrchestrationService.DeleteTaskByIdAsync(id);

                if (result)
                {
                    return Ok(message);
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the task.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpGet("tasks")]
        public async Task<ActionResult<List<UpdateTaskRequest>>> GetFilteredTasks([FromQuery] TaskFilterCriteria criteria)
        {
            try
            {
                List<UpdateTaskRequest> tasks =
                    await userTaskOrchestrationService.GetFilteredTasksAsync(criteria);

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving filtered tasks.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPut("assign-task-to-user")]
        public async Task<ActionResult<string>> AssignTaskToUser(Guid userId, Guid taskId)
        {
            try
            {
               var (code, message) =  await userTaskOrchestrationService.AssignTaskToUserAsync(
                    taskId,
                    userId);

                return Ok(message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning the task to the user.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost("add-collaborators")]
        public async Task<ActionResult<string>> AddTaskCollaborators(UserTask userTask)
        {
            try
            {
                var (code, message) = await userTaskOrchestrationService.AddTaskCollaborators(
                    userTask.TaskId,
                    userTask.UserId);

                return Ok(message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning the task to the user.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }
}
