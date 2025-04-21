using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.DTOs.Auth
{
    public class GoogleAuthDto
    {
        [Required(ErrorMessage = "Token is required.")]
        public required string Token { get; set; }
    }
}
