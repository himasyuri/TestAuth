using System.ComponentModel.DataAnnotations;

namespace AuthTest.Models.Requests
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Login { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
