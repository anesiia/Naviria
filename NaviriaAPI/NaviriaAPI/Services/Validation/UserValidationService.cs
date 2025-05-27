using NaviriaAPI.DTOs.User;
using NaviriaAPI.IRepositories;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NaviriaAPI.Services.Validation
{
    public class UserValidationService
    {
        private readonly IUserRepository _userRepository;

        public UserValidationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateCreateAsync(UserCreateDto dto)
        {
            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
                throw new ArgumentException("User with this email already exists");

            if (await _userRepository.GetByNicknameAsync(dto.Nickname) != null)
                throw new ArgumentException("User with this nickname already exists");

            ValidateFullName(dto.FullName, required: true);
            ValidateNickname(dto.Nickname, required: true);
            ValidateEmail(dto.Email, required: true);
            ValidatePassword(dto.Password, required: true);
            ValidateFutureMessage(dto.FutureMessage);

            var now = DateTime.UtcNow;
            if (dto.BirthDate > now)
                throw new ValidationException("Birth date cannot be in the future.");
            var age = now.Year - dto.BirthDate.Year;
            if (age < 18)
                throw new ValidationException("User must be at least 18 years old.");
            if (age > 120)
                throw new ValidationException("User cannot be older than 120 years.");
        }
        public static void ValidatePatch(UserPatchDto dto)
        {
            ValidateFullName(dto.FullName);
            ValidateNickname(dto.Nickname);
            ValidateDescription(dto.Description);
            ValidateEmail(dto.Email);
            ValidatePassword(dto.Password);
            ValidatePoints(dto.Points);
            ValidateFutureMessage(dto.FutureMessage);
            ValidatePhoto(dto.Photo);
        }

        public static void ValidateUpdate(UserUpdateDto dto)
        {
            ValidateFullName(dto.FullName);
            ValidateNickname(dto.Nickname);
            ValidateDescription(dto.Description);
            ValidateEmail(dto.Email);
            ValidatePassword(dto.Password);
            ValidatePoints(dto.Points);
            ValidateFutureMessage(dto.FutureMessage);
            ValidatePhoto(dto.Photo);
        }

        private static void ValidateFullName(string? fullName, bool required = false)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                if (required)
                    throw new ValidationException("FullName is required.");
                return;
            }
            if (fullName.Length < 3 || fullName.Length > 50)
                throw new ValidationException("FullName must be between 3 and 50 characters.");
            var regex = new Regex("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\\s'-]{1,50}$");
            if (!regex.IsMatch(fullName))
                throw new ValidationException("FullName contains invalid characters.");
        }

        private static void ValidateNickname(string? nickname, bool required = false)
        {
            if (string.IsNullOrEmpty(nickname))
            {
                if (required)
                    throw new ValidationException("Nickname is required.");
                return;
            }
            if (nickname.Length < 3 || nickname.Length > 20)
                throw new ValidationException("Nickname must be between 3 and 20 characters.");
            var regex = new Regex("^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(nickname))
                throw new ValidationException("Nickname must contain only letters and digits.");
        }

        private static void ValidateDescription(string? description)
        {
            if (string.IsNullOrEmpty(description)) return;
            if (description.Length > 150)
                throw new ValidationException("Description must be less than 150 characters.");
            var regex = new Regex("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\\s]{0,150}$");
            if (!regex.IsMatch(description))
                throw new ValidationException("Description contains invalid characters.");
        }

        private static void ValidateEmail(string? email, bool required = false)
        {
            if (string.IsNullOrEmpty(email))
            {
                if (required)
                    throw new ValidationException("Email is required.");
                return;
            }
            if (email.Length > 100)
                throw new ValidationException("Email must be less than 100 characters.");
            var regex = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
            if (!regex.IsMatch(email))
                throw new ValidationException("Email format is invalid.");
            if (!new EmailAddressAttribute().IsValid(email))
                throw new ValidationException("Email is not valid.");
        }

        private static void ValidatePassword(string? password, bool required = false)
        {
            if (string.IsNullOrEmpty(password))
            {
                if (required)
                    throw new ValidationException("Password is required.");
                return;
            }
            if (password.Length < 8)
                throw new ValidationException("Password must be at least 8 characters long.");
            var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$");
            if (!regex.IsMatch(password))
                throw new ValidationException("Password must contain at least one uppercase letter, one lowercase letter, and one digit.");
        }

        private static void ValidatePoints(int? points, bool required = false)
        {
            if (!points.HasValue)
            {
                if (required)
                    throw new ValidationException("Points is required.");
                return;
            }
            if (points.Value < 0)
                throw new ValidationException("Points cannot be negative.");
        }

        private static void ValidateFutureMessage(string? futureMessage)
        {
            if (string.IsNullOrEmpty(futureMessage)) return;
            if (futureMessage.Length > 150)
                throw new ValidationException("FutureMessage must be less than 150 characters.");
            var regex = new Regex("^[a-zA-Zа-яА-ЯёЁіІїЇєЄ0-9.,!\\s]{0,150}$");
            if (!regex.IsMatch(futureMessage))
                throw new ValidationException("FutureMessage contains invalid characters.");
        }

        private static void ValidatePhoto(string? photo)
        {
            if (string.IsNullOrEmpty(photo)) return;
            if (!Uri.TryCreate(photo, UriKind.Absolute, out _))
                throw new ValidationException("Photo must be a valid URL.");
        }
    }

}