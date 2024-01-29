
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ColorService
{
    private readonly ColorRepository _colorRepository;
    private readonly ILogger<ColorService> _logger;

    public ColorService(ColorRepository colorRepository, ILogger<ColorService> logger)
    {
        _colorRepository = colorRepository;
        _logger = logger;
    }

    public async Task<ColorEntity> AddColorAsync(string colorName)
    {
        try
        {
            var existingcolor = await _colorRepository.GetOneAsync(b => b.ColorName == colorName)
                                  ?? await _colorRepository.AddAsync(new ColorEntity { ColorName = colorName });

            return existingcolor;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting/adding color: {ex.Message}");
            Debug.WriteLine($"Error getting/adding color: {ex.Message}");
            return null!;
        }
    }


    public async Task<ColorEntity> UpdateColorAsync(ColorEntity color)
    {
        try
        {
            return await _colorRepository.UpdateAsync(c => c.Id == color.Id, color);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in updating color: {ex.Message}");
            Debug.WriteLine($"Error in updating color: {ex.Message}");
            return null!;
        }
    }

    public async Task<ColorEntity> GetColorAsync(string colorName)
    {
        try
        {
            return await _colorRepository.GetOneAsync(b => b.ColorName == colorName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting/adding color: {ex.Message}");
            Debug.WriteLine($"Error getting/adding color: {ex.Message}");
            return null!;
        }
    }


    public async Task<IEnumerable<ColorEntity>> GetAllColorsAsync()
    {
        try
        {
            return await _colorRepository.GetAllAsync() ?? Enumerable.Empty<ColorEntity>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting colors: {ex.Message}");
            Debug.WriteLine($"Error getting colors: {ex.Message}");
            return Enumerable.Empty<ColorEntity>();
        }
    }



    public async Task<bool> DeleteColorAsync(string colorName)
    {
        try
        {
            var result = await _colorRepository.GetOneAsync(b => b.ColorName == colorName);
            if (result != null)
            {
                await _colorRepository.RemoveAsync(b => b.ColorName == colorName);
                return true;
            }
            else
                return false;
  
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting color: {ex.Message}");
            Debug.WriteLine($"Error deleting color: {ex.Message}");
            return false;
        }
    }
}
