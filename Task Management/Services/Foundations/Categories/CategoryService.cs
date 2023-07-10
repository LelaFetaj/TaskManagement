using Microsoft.EntityFrameworkCore;
using Task_Management.Models.DTOs.Categories;
using Task_Management.Models.Entities.Categories;
using Task_Management.Respositories.Categories;

namespace Task_Management.Services.Foundations.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<(bool, string)> AddCategoryAsync(CategoryDto categoryDto)
        {
            bool exists = await categoryRepository
                .SelectAllCategories()
                .AnyAsync(x => x.Name == categoryDto.Name);

            if (exists)
            {
                return (false, "A category with the same name exists. Please choose another name!");
            }

            Category category = new Category
            {
                Name = categoryDto.Name
            };

            await categoryRepository.InsertCategoryAsync(category);

            return (true, "Success");
        }

        public async Task<List<Category>> RetrieveAllCategoriesAsync()
        {
            List<Category> categories = await categoryRepository
                .SelectAllCategories()
                .ToListAsync();

            return categories;
        }

        public async Task<Category> RetrieveCategoryByIdAsync(Guid id) =>
            await categoryRepository.SelectCategoryByIdAsync(id);

        public async Task<(bool, string)> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            Category category = await categoryRepository.SelectCategoryByIdAsync(categoryDto.Id);

            if (category == null)
            {
                return (false, $"Couldn't find category with id: {categoryDto.Id}");
            }

            bool exists = await categoryRepository
                .SelectAllCategories()
                .AnyAsync(x => x.Name == categoryDto.Name && x.Id != categoryDto.Id);

            if (exists)
            {
                return (false, "An category with the same name exists. Please choose another name!");
            }

            category.Name = categoryDto.Name;

            await categoryRepository.UpdateCategoryAsync(category);

            return (true, "Success");
        }

        public async Task<(bool, string)> DeleteCategoryByIdAsync(Guid id)
        {
            Category category = await categoryRepository.SelectCategoryByIdAsync(id);

            if (category == null)
            {
                return (false, $"Couldn't find category with id: {id}");
            }

            await categoryRepository.DeleteCategoryAsync(category);

            return (true, "Success");
        }
    }
}
