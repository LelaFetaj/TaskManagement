
using Task_Management.Models.Entities.Categories;

namespace Task_Management.Respositories.Categories
{
    public interface ICategoryRepository
    {
        Task InsertCategoryAsync(Category category);
        IQueryable<Category> SelectAllCategories();
        Task<Category> SelectCategoryByIdAsync(Guid id);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
