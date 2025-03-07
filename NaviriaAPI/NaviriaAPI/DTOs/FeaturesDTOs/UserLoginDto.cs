using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.FeaturesDTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Login is required")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
