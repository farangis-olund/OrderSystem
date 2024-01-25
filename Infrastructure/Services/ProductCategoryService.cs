
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Infrastructure.Dtos;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class ProductCategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ILogger<CustomerService> _logger;

        public ProductCategoryService(CategoryRepository categoryRepository, ILogger<CustomerService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<CategoryEntity> AddCategoryAsync(Category category)
        {

            try
            {
                var existingCategory = await _categoryRepository.Exist(c => c.CategoryName == category.CategoryName && 
                                                                            c.ParentCategoryId == category.ParentCategoryId);

                if (!existingCategory)
                {
                    var newCategory = new CategoryEntity
                    {
                        CategoryName = category.CategoryName,
                        ParentCategoryId = category.ParentCategoryId
                    };

                    return await _categoryRepository.AddAsync(newCategory);
                }
                return null!;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding product image: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<CategoryEntity> GetCategoryAsync(string categoryName, int parentCategoryId)
        {

            try
            {
                var existingCategory = await _categoryRepository.GetOneAsync(c => c.CategoryName == categoryName &&
                                                                             c.ParentCategoryId == parentCategoryId);
                if (existingCategory !=null)
                {
                    return existingCategory;
                }
                else
                    return null!;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding category: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<CategoryEntity> GetCategoryAsync(int id)
        {

            try
            {
                var existingCategory = await _categoryRepository.GetOneAsync(c => c.Id == id);
                if (existingCategory != null)
                {
                    return existingCategory;
                }
                else
                    return null!;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding category: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<CategoryEntity> UpdateCategoryAsync(int id, string categoryName)
        {

            try
            {
                var existingCategory = await _categoryRepository.GetOneAsync(c => c.Id == id);
                if (existingCategory != null)
                {
                    existingCategory.CategoryName = categoryName;
                    Func<CategoryEntity, object> keySelector = p => p.Id;
                    return await _categoryRepository.UpdateAsync(existingCategory, keySelector);
                }
                else
                    return existingCategory!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in updating category: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<bool> DeleteCategoryAsync(Category category)
        {

            try
            {
                var existingCategory = await _categoryRepository.Exist(c => c.Id == category.Id);
                if (existingCategory)
                {
                    await _categoryRepository.RemoveAsync(category);
                    return true;
                }
                else
                {
                    _logger.LogInformation($"Category does not exist!");
                    return false;
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in deleting category: {ex.Message}");
                Debug.WriteLine(ex.Message);
                return false!;
            }
        }
    }
}
