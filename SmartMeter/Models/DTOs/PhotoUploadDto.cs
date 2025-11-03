namespace SmartMeter.Models.DTOs
{
    public class PhotoUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }

    public class PhotoResponseDto
    {
        public string PhotoUrl { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}