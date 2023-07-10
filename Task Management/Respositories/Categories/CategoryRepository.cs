using Task_Management.Data.Context;
using Task_Management.Models.Entities.Categories;
using Task_Management.Models.Entities.Tasks;

namespace Task_Management.Respositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TaskManagementDbContext _dbContext;

        public CategoryRepository(TaskManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertCategoryAsync(Category category)
        {
            await _dbContext.Category.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Category> SelectAllCategories() =>
           _dbContext.Category;

        public async Task<Category> SelectCategoryByIdAsync(Guid id) =>
            await _dbContext.Category.FindAsync(id);

        public async Task UpdateCategoryAsync(Category category)
        {
            _dbContext.Category.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _dbContext.Category.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
