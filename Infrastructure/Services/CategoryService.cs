
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(CategoryRepository categoryRepository, ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<CategoryEntity> AddCategoryAsync(string categoryName)
    {
        try
        {
            var existingCategory = await _categoryRepository.GetOneAsync(b => b.CategoryName == categoryName)
                                  ?? await _categoryRepository.AddAsync(new CategoryEntity { CategoryName = categoryName });

            return existingCategory;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting/adding category: {ex.Message}");
            Debug.WriteLine($"Error getting/adding category: {ex.Message}");
            return null!;
        }
    }


    public async Task<CategoryEntity> GetCategoryAsync(string categoryName)
    {
        try
        {
            return await _categoryRepository.GetOneAsync(b => b.CategoryName == categoryName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting/adding category: {ex.Message}");
            Debug.WriteLine($"Error getting/adding category: {ex.Message}");
            return null!;
        }
    }


    public async Task<IEnumerable<CategoryEntity>> GetAllCategoriesAsync()
    {
        try
        {
            return await _categoryRepository.GetAllAsync() ?? Enumerable.Empty<CategoryEntity>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting categorys: {ex.Message}");
            Debug.WriteLine($"Error getting categorys: {ex.Message}");
            return Enumerable.Empty<CategoryEntity>();
        }
    }


    public async Task<CategoryEntity> UpdateCategoryAsync(CategoryEntity category)
    {
        try
        {
           return await _categoryRepository.UpdateAsync(c => c.Id == category.Id, category);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in updating product: {ex.Message}");
            Debug.WriteLine($"Error in updating product: {ex.Message}");
            return null!;
        }
    }

    public async Task<bool> DeleteCategoryAsync(string categoryName)
    {
        try
        {
            var result = await _categoryRepository.GetOneAsync(b => b.CategoryName == categoryName);
            if (result != null)
            {
                await _categoryRepository.RemoveAsync(b => b.CategoryName == categoryName);
                return true;
            } else
                return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting category: {ex.Message}");
            Debug.WriteLine($"Error deleting category: {ex.Message}");
            return false;
        }
    }
}
