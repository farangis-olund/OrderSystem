
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Drawing;

namespace Infrastructure.Services;

public class ProductSizeService
{
    private readonly SizeRepository _sizeRepository;
    private readonly ILogger<ProductSizeService> _logger;

    public ProductSizeService(SizeRepository sizeRepository, ILogger<ProductSizeService> logger)
    {
        _sizeRepository = sizeRepository;
        _logger = logger;
    }

    public async Task<SizeEntity> AddSizeAsync(ProductSize productSize)
    {

        try
        {
            var existingSize = await _sizeRepository.Exist(s => s.SizeType == productSize.SizeType &&
                                                           s.SizeValue == productSize.SizeValue &&
                                                           s.AgeGroup == productSize.AgeGroup);

            if (!existingSize)
            {
                var newSize = new SizeEntity
                {
                    SizeType = productSize.SizeType,
                    SizeValue = productSize.SizeValue,
                    AgeGroup = productSize.AgeGroup
                };

                return await _sizeRepository.AddAsync(newSize);
            }
            return null!;

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in adding size: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<SizeEntity> GetSizeAsync(ProductSize productSize)
    {

        try
        {
            var existingSize = await _sizeRepository.GetOneAsync(s => s.SizeType == productSize.SizeType &&
                                                                 s.SizeValue == productSize.SizeValue &&
                                                                 s.AgeGroup == productSize.AgeGroup);

            if (existingSize != null)
            {
                return existingSize;
            }
            else
            {
                Debug.WriteLine("Customer does not exist!");
                return null!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in geting customer: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<IEnumerable<SizeEntity>> GetAllSizesAsync()
    {

        try
        {
            var sizes = await _sizeRepository.GetAllAsync();

            return sizes;
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in geting customer: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return [];
        }
    }

    public async Task<SizeEntity> GetSizeAsync(int id)
    {

        try
        {
            var existingSize = await _sizeRepository.GetOneAsync(c => c.Id == id);

            if (existingSize != null)
            {
                return existingSize;
            }
            else
            {
                Debug.WriteLine("Size does not exist!");
                return null!;
            }


        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in adding size: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<SizeEntity> UpdateSizeAsync(ProductSize productSize)
    {
        try
        {
            var existingSize = await _sizeRepository.Exist(s => s.SizeType == productSize.SizeType &&
                                                                s.SizeValue == productSize.SizeValue &&
                                                                s.AgeGroup == productSize.AgeGroup);
            if (existingSize)
            {
                Func<SizeEntity, object> keySelector = p => p.Id;
                return await _sizeRepository.UpdateAsync(productSize, keySelector);
            }
            else
                return null!;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in adding size: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<bool> DeleteSizeAsync(ProductSize productSize)
    {
        try
        {

            var existingSize = await _sizeRepository.Exist(x => x.Equals(productSize));

            if (existingSize)
            {
                await _sizeRepository.RemoveAsync(productSize);
                return true;
            }
            else
                return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in adding size: {ex.Message}");
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
}
