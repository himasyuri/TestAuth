namespace AuthTest.Models
{
    public class User
    {
        public Guid Id { get; set; } = new Guid();

        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[32];

        public byte[] PasswordSalt { get; set; } = new byte[32];

        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
