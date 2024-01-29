
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;


namespace Infrastructure.Services;

public class SizeService
{
    private readonly SizeRepository _sizeRepository;
    private readonly ILogger<SizeService> _logger;

    public SizeService(SizeRepository sizeRepository, ILogger<SizeService> logger)
    {
        _sizeRepository = sizeRepository;
        _logger = logger;
    }

    public async Task<SizeEntity> AddSizeAsync(ProductSize productSize)
    {
        try
        {
            return await _sizeRepository.GetOneAsync(s => s.SizeType == productSize.SizeType &&
                                                                 s.SizeValue == productSize.SizeValue &&
                                                                 s.AgeGroup == productSize.AgeGroup)
                            ?? await _sizeRepository.AddAsync(new SizeEntity { SizeType = productSize.SizeType, 
                                                                               SizeValue = productSize.SizeValue, 
                                                                               AgeGroup =productSize.AgeGroup });
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
                Debug.WriteLine("Size does not exist!");
                return null!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in geting Size: {ex.Message}");
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
            _logger.LogError($"Error in geting Size: {ex.Message}");
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

    public async Task<SizeEntity> UpdateSizeAsync(SizeEntity size)
    {
        try
        {
            var existingSize = await _sizeRepository.GetOneAsync(s => s.SizeType == size.SizeType &&
                                                                s.SizeValue == size.SizeValue &&
                                                                s.AgeGroup == size.AgeGroup);
            if (existingSize != null)
            {
                 return await _sizeRepository.UpdateAsync(s => s.Id == size.Id, size);
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
            var sizeToDelete = await _sizeRepository.GetOneAsync(s =>
                s.SizeType == productSize.SizeType &&
                s.SizeValue == productSize.SizeValue &&
                s.AgeGroup == productSize.AgeGroup);

            if (sizeToDelete != null)
            {
                await _sizeRepository.RemoveAsync(sizeToDelete);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting size: {ex.Message}");
            Debug.WriteLine($"Error deleting size: {ex.Message}");
            return false;
        }
    }
}
