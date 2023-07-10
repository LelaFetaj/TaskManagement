using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Net;
using Task_Management.Models.DTOs.Categories;
using Task_Management.Models.DTOs.Tasks;
using Task_Management.Models.Entities.Categories;
using Task_Management.Models.Entities.ErrorResponse;
using Task_Management.Services.Orchestration.Categories;

namespace Task_Management.Controllers.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryOrchestrationService categoryOrchestrationService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ILogger<CategoriesController> _logger,
            ICategoryOrchestrationService categoryOrchestrationService)
        {
            this._logger = _logger;
            this.categoryOrchestrationService = categoryOrchestrationService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddCategory(CategoryDto categoryDto)
        {
            try
            {
                (bool result, string message) =
                    await categoryOrchestrationService.AddCategoryAsync(categoryDto);

                if (result)
                {
                    return Ok(message);
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the category.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost("assign-task-to-category")]
        public async Task<ActionResult<string>> AssignTaskToCategoryAsync(UpdateTaskRequest updateTaskRequest)
        {
            try
            {
                await categoryOrchestrationService.AssignTaskToCategoryAsync(
                    updateTaskRequest.Id,
                    updateTaskRequest.CategoryId ?? Guid.Empty);

                return Ok("Task assigned to the category successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning the task to the category.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            try
            {
                List<Category> categories =
                    await categoryOrchestrationService.RetrieveAllCategoriesAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all categories.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            try
            {
                Category category =
                    await categoryOrchestrationService.RetrieveCategoryByIdAsync(id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the category.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateCategory(CategoryDto categoryDto)
        {
            try
            {
                (bool result, string message) =
                    await categoryOrchestrationService.UpdateCategoryAsync(categoryDto);

                if (result)
                {
                    return Ok(message);
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the category.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = ex.Message
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteCategory(Guid id)
        {
            try
            {
                (bool result, string message) =
                    await categoryOrchestrationService.DeleteCategoryByIdAsync(id);

                if (result)
                {
                    return Ok(message);
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the category.");

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
