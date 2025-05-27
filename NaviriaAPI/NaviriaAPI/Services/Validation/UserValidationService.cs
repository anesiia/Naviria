using NaviriaAPI.DTOs.User;
using NaviriaAPI.IRepositories;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.Services.Validation
{
    public class UserValidationService
    {
        private readonly IUserRepository _userRepository;

        public UserValidationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(UserCreateDto dto)
        {
            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
                throw new ArgumentException("User with this email already exists");

            if (await _userRepository.GetByNicknameAsync(dto.Nickname) != null)
                throw new ArgumentException("User with this nickname already exists");

            var now = DateTime.UtcNow;
            var age = now.Year - dto.BirthDate.Year;
            if (dto.BirthDate > now)
                throw new ValidationException("Birth date cannot be in the future");
            if (age < 18)
                throw new ValidationException("User must be at least 18 years old");
            if (age > 120)
                throw new ValidationException("User cannot be older than 120 years");
        }

        public static void ValidateAsync(UserUpdateDto dto)
        {
            var now = DateTime.UtcNow;
            if (dto.LastSeen.HasValue && dto.LastSeen.Value > now)
                throw new ValidationException("LastSeen cannot be in the future");
        }

        public static void ValidateAsync(UserPatchDto dto)
        {
            var context = new ValidationContext(dto, serviceProvider: null, items: null);

            var results = new List<ValidationResult>();
            foreach (var prop in typeof(UserPatchDto).GetProperties())
            {
                var value = prop.GetValue(dto);
                if (value != null)
                {
                    var memberContext = new ValidationContext(dto) { MemberName = prop.Name };
                    Validator.ValidateProperty(value, memberContext);
                }
            }

            var now = DateTime.UtcNow;

            if (dto.Points.HasValue && dto.Points.Value < 0)
                throw new ValidationException("Points cannot be negative");

            if (dto.Email != null && !new EmailAddressAttribute().IsValid(dto.Email))
                throw new ValidationException("Email format is invalid");

            if (dto.Photo != null && !Uri.TryCreate(dto.Photo, UriKind.Absolute, out _))
                throw new ValidationException("Photo must be a valid URL");

            if (dto.Password != null && dto.Password.Length < 8)
                throw new ValidationException("Password must be at least 8 characters long");
        }
    }

}