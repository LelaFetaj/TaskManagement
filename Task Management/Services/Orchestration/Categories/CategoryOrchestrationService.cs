using SendGrid.Helpers.Errors.Model;
using Task_Management.Models.DTOs.Categories;
using Task_Management.Models.DTOs.Tasks;
using Task_Management.Models.Entities.Categories;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Services.Foundations.Categories;
using Task_Management.Services.Foundations.Projects;
using Task_Management.Services.Foundations.Tasks;

namespace Task_Management.Services.Orchestration.Categories
{
    public class CategoryOrchestrationService : ICategoryOrchestrationService
    {
        private readonly ITaskService taskService;
        private readonly ICategoryService categoryService;

        public CategoryOrchestrationService(ITaskService taskService,
            ICategoryService categoryService)
        {
            this.taskService = taskService;
            this.categoryService = categoryService;
        }

        public async Task<(bool, string)> AddCategoryAsync(CategoryDto categoryDto) =>
            await this.categoryService.AddCategoryAsync(categoryDto);

        public async Task AssignTaskToCategoryAsync(Guid taskId, Guid categoryId)
        {
            // Retrieve the task and category details
            TaskEntity task = await taskService.RetrieveTaskById(taskId);
            Category category = await categoryService.RetrieveCategoryByIdAsync(categoryId);

            if (task == null || category == null)
            {
                throw new NotFoundException("Task or Category not found");
            }

            // Assign the task to the category
            task.CategoryId = categoryId;
            await taskService.UpdateTaskAsync(new UpdateTaskRequest
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CategoryId = categoryId
            });
        }

        public async Task<(bool, string)> DeleteCategoryByIdAsync(Guid id) =>
            await this.categoryService.DeleteCategoryByIdAsync(id);

        public async Task<List<Category>> RetrieveAllCategoriesAsync() =>
            await this.categoryService.RetrieveAllCategoriesAsync();

        public async Task<Category> RetrieveCategoryByIdAsync(Guid id) =>
            await this.categoryService.RetrieveCategoryByIdAsync(id);

        public async Task<(bool, string)> UpdateCategoryAsync(CategoryDto categoryDto) =>
            await this.categoryService.UpdateCategoryAsync(categoryDto);
    }
}
