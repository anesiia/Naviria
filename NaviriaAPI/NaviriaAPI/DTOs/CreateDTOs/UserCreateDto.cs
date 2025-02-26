namespace NaviriaAPI.DTOs.CreateDTOs
{
    public class UserCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = new DateTime(2000, 1, 1);
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Points { get; set; } = 0;
        public string[] Friends { get; set; } = [];
        public string[] Achivements { get; set; } = [];
        public string FutureMessage { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
    }
}
