﻿using Task_Management.Models.DTOs.Categories;
using Task_Management.Models.Entities.Categories;

namespace Task_Management.Services.Foundations.Categories
{
    public interface ICategoryService
    {
        Task<(bool, string)> AddCategoryAsync(CategoryDto categoryDto);

        Task<List<Category>> RetrieveAllCategoriesAsync();

        Task<Category> RetrieveCategoryByIdAsync(Guid id);

        Task<(bool, string)> UpdateCategoryAsync(CategoryDto categoryDto);

        Task<(bool, string)> DeleteCategoryByIdAsync(Guid id);
    }
}
