namespace AuthTest.Models
{
    public class RefreshToken
    {
        public long Id { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(30);

        public DateTime InActiveLifeTime { get; set; } = DateTime.UtcNow.AddDays(365);

        public bool IsUsed { get; set; } = false;

        //hash or uid
        public string Fingerprint { get; set; } = string.Empty;

        //device
        public string DeviceName { get; set; } = string.Empty; // maybe delete after

        public bool IsExpired => DateTime.UtcNow > Expires;

        public bool IsRevoked { get; set; } = false;
        
        public Guid UserId { get; set; }

        public User? User { get; set; }
    }
}
