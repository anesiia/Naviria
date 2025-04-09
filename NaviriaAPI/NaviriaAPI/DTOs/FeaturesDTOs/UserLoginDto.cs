using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.FeaturesDTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain upper, lower letters and digits")]
        public string? Password { get; set; }
    }
}
