using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.CreateDTOs
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3, ErrorMessage = "FullName must be at least 3 characters long.")]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\\s'-]{1,50}$", ErrorMessage = "FullName can only contain Cyrillic and Latin letters, spaces, apostrophes, and hyphens.")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MinLength(3, ErrorMessage = "Nickname must be at least 3 characters long.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Nickname can only contain Latin letters and digits.")]
        public string Nickname { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^[fmFM]$", ErrorMessage = "Gender must be 'f' or 'm'.")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; } = new DateTime(2000, 1, 1).ToUniversalTime();

        [MaxLength(150)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\\s]{0,150}$", ErrorMessage = "Description contains invalid characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string Password { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Points { get; set; } = 0;

        public string[] Friends { get; set; } = [];

        public string[] Achievements { get; set; } = [];

        [MaxLength(150)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\\s]{0,150}$", ErrorMessage = "FutureMessage contains invalid characters.")]
        public string FutureMessage { get; set; } = string.Empty;

        [Url]
        public string Photo { get; set; } = string.Empty;

        public DateTime LastSeen { get; set; } = DateTime.Now;

        public bool IsOnline { get; set; } = false;
        public bool IsProUser { get; set; } = false;
    }
}
