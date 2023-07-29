using System.ComponentModel.DataAnnotations;

namespace AuthTest.Models.Requests
{
    public class EditUserDataRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get;  set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
