using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class UserUpdateDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\\s'-]{1,50}$")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Z0-9]+$")]
        public string Nickname { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^[fmFM]$")]
        public string Gender { get; set; } = string.Empty;

        [MaxLength(150)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\\s]{0,150}$")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
        public string Email { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Points { get; set; } = 0;

        public string[] Friends { get; set; } = [];

        public string[] Achievements { get; set; } = [];

        [MaxLength(150)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\\s]{0,150}$")]
        public string FutureMessage { get; set; } = string.Empty;

        [Url]
        public string Photo { get; set; } = string.Empty;
        public DateTime LastSeen { get; set; } = DateTime.Now;
        public bool IsOnline { get; set; } = false;
        public bool IsProUser { get; set; } = false;
    }

}
