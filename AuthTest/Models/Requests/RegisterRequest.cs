using System.ComponentModel.DataAnnotations;

namespace AuthTest.Models.Requests
{
    public class RegisterRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password", ErrorMessage = "Passwords dont match")]
        [DataType(DataType.Password)]
        public string PasswordConfirmed { get; set; } = string.Empty;
    }
}
