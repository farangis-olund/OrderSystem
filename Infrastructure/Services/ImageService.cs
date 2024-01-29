
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ImageService
{
    private readonly ImageRepository _imageRepository;
    private readonly ILogger<ImageService> _logger;

    public ImageService(ImageRepository imageRepository, ILogger<ImageService> logger)
    {
        _imageRepository = imageRepository;
        _logger = logger;
    }

    public async Task<ImageEntity> AddImageAsync(string imageUrl)
    {
        try
        {
            var existingImage = await _imageRepository.GetOneAsync(b => b.ImageUrl == imageUrl)
                                  ?? await _imageRepository.AddAsync(new ImageEntity { ImageUrl =imageUrl });

            return existingImage!;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting/adding image: {ex.Message}");
            Debug.WriteLine($"Error getting/adding image: {ex.Message}");
            return null!;
        }
    }


    public async Task<ImageEntity> GetImageAsync(string imageUrl)
    {
        try
        {
            return await _imageRepository.GetOneAsync(b => b.ImageUrl == imageUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting/adding image: {ex.Message}");
            Debug.WriteLine($"Error getting/adding image: {ex.Message}");
            return null!;
        }
    }


    public async Task<IEnumerable<ImageEntity>> GetAllimagesAsync()
    {
        try
        {
            return await _imageRepository.GetAllAsync() ?? Enumerable.Empty<ImageEntity>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting images: {ex.Message}");
            Debug.WriteLine($"Error getting images: {ex.Message}");
            return Enumerable.Empty<ImageEntity>();
        }
    }

    public async Task<ImageEntity> UpdateimageAsync(ImageEntity image)
    {
        try
        {
            return await _imageRepository.UpdateAsync(c => c.Id == image.Id, image);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in updating product: {ex.Message}");
            Debug.WriteLine($"Error in updating product: {ex.Message}");
            return null!;
        }
    }

    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        try
        {
            var result = await _imageRepository.GetOneAsync(b => b.ImageUrl == imageUrl);
            if (result != null)
            {
                await _imageRepository.RemoveAsync(b => b.ImageUrl == imageUrl);
                return true;
            } else
                return false;
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting image: {ex.Message}");
            Debug.WriteLine($"Error deleting image: {ex.Message}");
            return false;
        }
    }
}
