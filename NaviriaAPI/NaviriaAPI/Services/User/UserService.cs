using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using Microsoft.AspNetCore.Identity;
using NaviriaAPI.Entities;
using NaviriaAPI.Services.Validation;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.IServices.IGamificationLogic;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.Helpers;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.IServices.ICleanupServices;
using NaviriaAPI.DTOs.User;
using NaviriaAPI.IServices.ICloudStorage;

namespace NaviriaAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly UserValidationService _validation;
        private readonly IAchievementRepository _achievementRepository;
        private readonly ILevelService _levelService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UserService> _logger;
        private readonly IAchievementManager _achievementManager;
        private readonly IUserCleanupService _userCleanupService;
        private readonly ICloudinaryService _cloudinaryService;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<UserEntity> passwordHasher,
            UserValidationService validation,
            IAchievementRepository achievementRepository,
            ILevelService levelService,
            IJwtService jwtService,
            ILogger<UserService> logger,
            IAchievementManager achievementManager,
            IUserCleanupService userCleanupService,
            ICloudinaryService cloudinaryService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _validation = validation;
            _achievementRepository = achievementRepository;
            _levelService = levelService;
            _jwtService = jwtService;
            _logger = logger;
            _achievementManager = achievementManager;
            _userCleanupService = userCleanupService;
            _cloudinaryService = cloudinaryService;
        }

        /// <inheritdoc />
        public async Task<string> CreateAsync(UserCreateDto userDto)
        {
            var userByEmail = await _userRepository.GetByEmailAsync(userDto.Email);
            var userByNickname = await _userRepository.GetByNicknameAsync(userDto.Nickname);

            if (userByEmail != null)
                throw new EmailAlreadyExistException("User with such email already exists");
            if (userByNickname != null)
                throw new NicknameAlreadyExistException("User with such nickname already exists");


            await _validation.ValidateCreateAsync(userDto);

            var entity = UserMapper.ToEntity(userDto);
            entity.Password = _passwordHasher.HashPassword(entity, userDto.Password);
            entity.Points = 50;
            entity.LevelInfo = _levelService.CalculateFirstLevelProgress(entity.Points);

            await _userRepository.CreateAsync(entity);
            await _achievementManager.EvaluateAsync(entity.Id, AchievementTrigger.OnRegistration);

            return _jwtService.GenerateUserToken(entity);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(string id, UserUpdateDto userDto)
        {
            UserValidationService.ValidateUpdate(userDto);

            var userDtoFromDb = await GetByIdAsync(id);
            if (userDtoFromDb == null)
                throw new NotFoundException($"User with ID {id} not found.");

            if(userDto.Password == null)
                userDto.Password = userDtoFromDb.Password;

            if(string.IsNullOrEmpty(userDtoFromDb.Photo) && userDto.Photo != null)
                await _achievementManager.EvaluateAsync(id, AchievementTrigger.OnPhotoUploading);

            // Recalculate level info if points changed
            if (userDtoFromDb.Points != userDto.Points)
            {
                int additionalXp = userDto.Points - userDtoFromDb.Points;
                userDto.LevelInfo = await _levelService.CalculateLevelProgressAsync(userDtoFromDb, additionalXp);
            }
            else
            {
                userDto.LevelInfo = userDtoFromDb.LevelInfo;
            }

            var updatedUserEntity = UserMapper.ToEntity(id, userDto);
            updatedUserEntity.RegitseredAt = userDtoFromDb.RegitseredAt;
            updatedUserEntity.LastSeen = DateTime.Now;

            return await _userRepository.UpdateAsync(updatedUserEntity);
        }

        /// <inheritdoc />
        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.LastSeen = entity.LastSeen.ToLocalTime();
            return UserMapper.ToDto(entity);
        }

        /// <inheritdoc />
        public async Task<bool> UserExistsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            var user = await _userRepository.GetByIdAsync(userId);
            return user != null;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            users.ForEach(u => u.LastSeen = u.LastSeen.ToLocalTime());
            return users.Select(UserMapper.ToDto).ToList();
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(string id)
        {
            return await _userCleanupService.DeleteUserAndRelatedDataAsync(id);
        }

        /// <inheritdoc />
        public async Task<bool> GiveAchievementAsync(string userId, string achievementId)
        {
            var user = await GetUserOrThrowAsync(userId);

            if (user.Achievements.Any(a => a.AchievementId == achievementId))
            {
                _logger.LogWarning("User {UserId} already has achievement {AchievementId}.", userId, achievementId);
                throw new AlreadyExistException($"User already has achievement {achievementId}");
            }

            var achievement = await _achievementRepository.GetByIdAsync(achievementId);
            if (achievement == null)
            {
                _logger.LogWarning("Achievement with ID {AchievementId} not found.", achievementId);
                throw new NotFoundException("Achievement not found.");
            }

            user.Achievements.Add(new UserAchievementInfo
            {
                AchievementId = achievementId,
                ReceivedAt = DateTime.UtcNow,
                IsPointsReceived = false
            });

            return await _userRepository.UpdateAsync(user);
        }

        /// <inheritdoc />
        public async Task<UserEntity> GetUserOrThrowAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            return user;
        }

        /// <inheritdoc />
        public async Task<bool> UploadUserProfilePhotoAsync(string userId, IFormFile file)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new InvalidOperationException("User not found!");

            bool hadNoPhoto = string.IsNullOrEmpty(user.Photo);

            var imageUrl = await _cloudinaryService.UploadImageAndGetUrlAsync(file);

            user.Photo = imageUrl;
            await _userRepository.UpdateAsync(user);

            if (hadNoPhoto)
                await _achievementManager.EvaluateAsync(userId, AchievementTrigger.OnPhotoUploading);

            return true;
        }

        public async Task<bool> PatchAsync(string id, UserPatchDto patchDto)
        {
            var userEntity = await _userRepository.GetByIdAsync(id);
            if (userEntity == null)
                throw new NotFoundException($"User with ID {id} not found.");

            UserValidationService.ValidatePatch(patchDto);

            if (patchDto.FullName != null)
                userEntity.FullName = patchDto.FullName;

            if (patchDto.Nickname != null)
                userEntity.Nickname = patchDto.Nickname;

            if (patchDto.Description != null)
                userEntity.Description = patchDto.Description;

            if (patchDto.Email != null)
                userEntity.Email = patchDto.Email;

            if (patchDto.Password != null)
                userEntity.Password = _passwordHasher.HashPassword(userEntity, patchDto.Password);

            // Points and LevelInfo
            if (patchDto.Points.HasValue)
            {
                int oldPoints = userEntity.Points;
                userEntity.Points = patchDto.Points.Value;

                int additionalXp = userEntity.Points - oldPoints;

                userEntity.LevelInfo = await _levelService.CalculateLevelProgressAsync(UserMapper.ToDto(userEntity), additionalXp);
            }

            if (patchDto.LevelInfo != null)
                userEntity.LevelInfo = patchDto.LevelInfo;

            if (patchDto.Friends != null)
                userEntity.Friends = patchDto.Friends;

            if (patchDto.Achievements != null)
                userEntity.Achievements = patchDto.Achievements;

            if (patchDto.FutureMessage != null)
                userEntity.FutureMessage = patchDto.FutureMessage;

            if (patchDto.Photo != null)
                userEntity.Photo = patchDto.Photo;

            if (patchDto.IsOnline.HasValue)
                userEntity.IsOnline = patchDto.IsOnline.Value;

            if (patchDto.IsProUser.HasValue)
                userEntity.IsProUser = patchDto.IsProUser.Value;

            return await _userRepository.UpdateAsync(userEntity);
        }
    }
}
