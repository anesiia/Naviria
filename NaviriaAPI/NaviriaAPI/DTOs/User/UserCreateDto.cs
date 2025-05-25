using NaviriaAPI.Entities.EmbeddedEntities;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.User
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3, ErrorMessage = "FullName must be at least 3 characters long.")]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\\s'-]{1,50}$", ErrorMessage = "FullName can only contain Cyrillic and Latin letters, spaces, apostrophes, and hyphens.")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [MinLength(3, ErrorMessage = "Nickname must be at least 3 characters long.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Nickname can only contain Latin letters and digits.")]
        public string Nickname { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^[fmFM]$", ErrorMessage = "Gender must be 'f' or 'm'.")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public required DateTime BirthDate { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string Password { get; set; } = string.Empty;

        [MaxLength(150)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\\s]{0,150}$", ErrorMessage = "FutureMessage contains invalid characters.")]
        public string FutureMessage { get; set; } = string.Empty;
    }
}
