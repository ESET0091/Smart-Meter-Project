using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Models.DTOs;

namespace SmartMeter.Services
{
    public interface IConsumerPhotoService
    {
        Task<PhotoResponseDto?> UploadConsumerPhotoAsync(long consumerId, IFormFile file);
        Task<bool> DeleteConsumerPhotoAsync(long consumerId);
        Task<PhotoResponseDto?> GetConsumerPhotoAsync(long consumerId);
        Task<bool> ConsumerExistsAsync(long consumerId);
    }

    public class ConsumerPhotoService : IConsumerPhotoService
    {
        private readonly SmartMeterDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ConsumerPhotoService> _logger;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private static readonly string[] AllowedContentTypes = { "image/jpeg", "image/png", "image/gif", "image/bmp" };
        private readonly IConfiguration _configuration;

        public ConsumerPhotoService(SmartMeterDbContext context, IWebHostEnvironment environment, ILogger<ConsumerPhotoService> logger, IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> ConsumerExistsAsync(long consumerId)
        {
            return await _context.Consumers.AnyAsync(c => c.Consumerid == consumerId);
        }

        public async Task<PhotoResponseDto?> UploadConsumerPhotoAsync(long consumerId, IFormFile file)
        {
            try
            {
                // Validate file
                var validationResult = ValidateFile(file);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("File validation failed: {Error}", validationResult.ErrorMessage);
                    return null;
                }

                // Find consumer
                var consumer = await _context.Consumers.FindAsync(consumerId);
                if (consumer == null)
                {
                    _logger.LogWarning("Consumer {ConsumerId} not found", consumerId);
                    return null;
                }

                // Delete old photo if exists
                await DeleteExistingPhoto(consumer);

                // Generate unique filename
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var fileName = $"consumer_{consumerId}_{Guid.NewGuid()}{fileExtension}";
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "consumers");

                // Ensure directory exists
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Update consumer record
                var baseUrl = $"{GetBaseUrl()}/uploads/consumers/";
                consumer.ProfilePhotoPath = filePath;
                consumer.ProfilePhotoUrl = $"{baseUrl}{fileName}";
                consumer.ProfilePhotoSize = file.Length;
                consumer.ProfilePhotoContentType = file.ContentType;
                consumer.ProfilePhotoUpdatedAt = DateTime.UtcNow;
                consumer.Updatedat = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Profile photo uploaded for consumer {ConsumerId}", consumerId);

                return new PhotoResponseDto
                {
                    PhotoUrl = consumer.ProfilePhotoUrl,
                    FilePath = filePath,
                    FileSize = file.Length,
                    ContentType = file.ContentType,
                    UploadedAt = consumer.ProfilePhotoUpdatedAt.Value
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading profile photo for consumer {ConsumerId}", consumerId);
                return null;
            }
        }

        public async Task<bool> DeleteConsumerPhotoAsync(long consumerId)
        {
            try
            {
                var consumer = await _context.Consumers.FindAsync(consumerId);
                if (consumer == null || string.IsNullOrEmpty(consumer.ProfilePhotoPath))
                    return false;

                // Delete physical file
                if (File.Exists(consumer.ProfilePhotoPath))
                {
                    File.Delete(consumer.ProfilePhotoPath);
                }

                // Clear consumer photo data
                consumer.ProfilePhotoPath = null;
                consumer.ProfilePhotoUrl = null;
                consumer.ProfilePhotoSize = null;
                consumer.ProfilePhotoContentType = null;
                consumer.ProfilePhotoUpdatedAt = null;
                consumer.Updatedat = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Profile photo deleted for consumer {ConsumerId}", consumerId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting profile photo for consumer {ConsumerId}", consumerId);
                return false;
            }
        }

        public async Task<PhotoResponseDto?> GetConsumerPhotoAsync(long consumerId)
        {
            var consumer = await _context.Consumers.FindAsync(consumerId);
            if (consumer == null || string.IsNullOrEmpty(consumer.ProfilePhotoUrl))
                return null;

            return new PhotoResponseDto
            {
                PhotoUrl = consumer.ProfilePhotoUrl,
                FilePath = consumer.ProfilePhotoPath!,
                FileSize = consumer.ProfilePhotoSize ?? 0,
                ContentType = consumer.ProfilePhotoContentType!,
                UploadedAt = consumer.ProfilePhotoUpdatedAt ?? DateTime.UtcNow
            };
        }

        private (bool IsValid, string? ErrorMessage) ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, "File is empty");

            if (file.Length > MaxFileSize)
                return (false, $"File size exceeds {MaxFileSize / 1024 / 1024}MB");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return (false, "Invalid file type. Allowed: " + string.Join(", ", AllowedExtensions));

            if (!AllowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
                return (false, "Invalid content type");

            return (true, null);
        }

        private async Task DeleteExistingPhoto(Consumer consumer)
        {
            if (!string.IsNullOrEmpty(consumer.ProfilePhotoPath) && File.Exists(consumer.ProfilePhotoPath))
            {
                try
                {
                    File.Delete(consumer.ProfilePhotoPath);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete old profile photo for consumer {ConsumerId}", consumer.Consumerid);
                }
            }
        }

        private string GetBaseUrl()
        {
            return _configuration["BaseUrl"] ?? "https://localhost:7135";
        }
    }
}