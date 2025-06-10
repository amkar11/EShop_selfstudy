namespace User.Domain.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpiresInMinutes { get; set; } = default!;
    }
}
