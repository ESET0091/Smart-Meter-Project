using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMeter.Models.DTOs;
using SmartMeter.Services;
using System.Security.Claims;

namespace SmartMeter.Controllers
{
    [Route("api/consumers/{consumerId}/[controller]")]
    [ApiController]
    [Authorize]
    public class PhotoController : ControllerBase
    {
        private readonly IConsumerPhotoService _consumerPhotoService;
        private readonly ILogger<PhotoController> _logger;

        public PhotoController(IConsumerPhotoService consumerPhotoService, ILogger<PhotoController> logger)
        {
            _consumerPhotoService = consumerPhotoService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<PhotoResponseDto>> UploadConsumerPhoto(long consumerId, [FromForm] PhotoUploadDto request)
        {
            // Check if consumer exists
            if (!await _consumerPhotoService.ConsumerExistsAsync(consumerId))
                return NotFound($"Consumer with ID {consumerId} not found");

            if (request.File == null)
                return BadRequest("No file uploaded");

            var result = await _consumerPhotoService.UploadConsumerPhotoAsync(consumerId, request.File);

            if (result == null)
                return BadRequest("Failed to upload photo. Please check file size and type.");

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteConsumerPhoto(long consumerId)
        {
            // Check if consumer exists
            if (!await _consumerPhotoService.ConsumerExistsAsync(consumerId))
                return NotFound($"Consumer with ID {consumerId} not found");

            var result = await _consumerPhotoService.DeleteConsumerPhotoAsync(consumerId);

            if (!result)
                return BadRequest("Failed to delete profile photo");

            return Ok("Profile photo deleted successfully");
        }

        [HttpGet]
        public async Task<ActionResult<PhotoResponseDto>> GetConsumerPhoto(long consumerId)
        {
            // Check if consumer exists
            if (!await _consumerPhotoService.ConsumerExistsAsync(consumerId))
                return NotFound($"Consumer with ID {consumerId} not found");

            var result = await _consumerPhotoService.GetConsumerPhotoAsync(consumerId);

            if (result == null)
                return NotFound("No profile photo found for this consumer");

            return Ok(result);
        }

        [HttpGet("download")]
        [AllowAnonymous]
        public async Task<ActionResult> DownloadConsumerPhoto(long consumerId)
        {
            var result = await _consumerPhotoService.GetConsumerPhotoAsync(consumerId);

            if (result == null || string.IsNullOrEmpty(result.FilePath) || !System.IO.File.Exists(result.FilePath))
                return NotFound("Photo not found");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(result.FilePath);
            return File(fileBytes, result.ContentType, $"consumer_{consumerId}_photo{Path.GetExtension(result.FilePath)}");
        }
    }
}