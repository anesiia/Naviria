namespace NaviriaAPI.DTOs.UdateDTOs
{
    public class UserUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Points { get; set; } = 0;
        public string[] Friends { get; set; } = [];
        public string[] Achievements { get; set; } = [];
        public string FutureMessage { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
    }
}
