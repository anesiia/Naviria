using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class AchievementUpdateDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2, ErrorMessage = "Achievement name must be at least 2 characters long.")]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9\\s'-]{1,50}$",
            ErrorMessage = "Achievement name can only contain Cyrillic and Latin letters, spaces, apostrophes, and hyphens.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(150)]
        [RegularExpression("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\"'\\-\\s]{0,150}$", ErrorMessage = "Description contains invalid characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue)]
        public int Points { get; set; } = 0;

        [Required]
        public bool IsRare { get; set; }
    }
}
