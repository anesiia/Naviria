using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
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
            if (dto.LastSeen > now)
                throw new ValidationException("LastSeen cannot be in the future");
        }
    }
}

