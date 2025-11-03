namespace SmartMeter.Models.DTOs
{
    public class RefreshTokenRequestDto
    {
        public long UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}