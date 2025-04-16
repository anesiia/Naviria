using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices;
using NaviriaAPI.Mappings;
using Microsoft.AspNetCore.Identity;
using NaviriaAPI.Entities;
using OpenAI.Chat;
using NaviriaAPI.Services.Validation;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.Repositories;
using ZstdSharp;

namespace NaviriaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly UserValidationService _validation;
        private readonly string _openAIKey;
        public readonly ICloudinaryService _cloudinaryService;
        public readonly IAchievementRepository _achievementRepository;
        public UserService(
            IUserRepository userRepository, 
            IPasswordHasher<UserEntity> passwordHasher,
            IConfiguration config,
            UserValidationService validation,
            ICloudinaryService cloudinaryService,
            IAchievementRepository achievementRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _validation = validation;
            _openAIKey = config["OpenAIKey"]
                ?? throw new InvalidOperationException("OpenAIKey is missing in configuration.");
            _cloudinaryService = cloudinaryService;
            _achievementRepository = achievementRepository;
        }
        public async Task<UserDto> CreateAsync(UserCreateDto userDto)
        {
            await _validation.ValidateAsync(userDto);

            userDto.LastSeen = userDto.LastSeen.ToUniversalTime();
            var entity = UserMapper.ToEntity(userDto);
            entity.Password = _passwordHasher.HashPassword(entity, userDto.Password);
            await _userRepository.CreateAsync(entity);

            if (userDto.Photo != null)
            {
                await _cloudinaryService.UploadImageAsync(entity.Id, userDto.Photo);
            }

            return UserMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, UserUpdateDto userDto)
        {
            UserValidationService.ValidateAsync(userDto);

            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null)
                return false;

            userDto.LastSeen = userDto.LastSeen.ToUniversalTime();

            if(existing.Points != userDto.Points)
            {
                userDto.LevelInfo = CalculateLevelProgress(userDto.Points);
            }
            var entity = UserMapper.ToEntity(id, userDto);

            return await _userRepository.UpdateAsync(entity);

        }
        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            if (entity == null)
                return null;

            entity.LastSeen = entity.LastSeen.ToLocalTime();
            return UserMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _userRepository.DeleteAsync(id);

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            users.ForEach(user => user.LastSeen = user.LastSeen.ToLocalTime());
            return users.Select(UserMapper.ToDto).ToList();
        }
        public async Task<string> GetAiAnswerAsync(string question)
        {
            var modelName = "gpt-4o-mini";
            var client = new ChatClient(modelName, _openAIKey);

            var responce = await client.CompleteChatAsync(question);

            return responce.Value.Content[0].Text;
        }

        public async Task<bool> GiveAchievementAsync(string userId, string achievementId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            if (user.Achievements.Any(a => a.AchievementId == achievementId && a.IsReceived))
                return false;

            user.Achievements.Add(new UserAchievementInfo
            {
                AchievementId = achievementId,
                IsReceived = true,
                ReceivedAt = DateTime.UtcNow.ToUniversalTime()
            });

            // Update user points
            var achievement = await _achievementRepository.GetByIdAsync(achievementId);
            if (achievement != null)
                user.Points += achievement.Points;

            return await _userRepository.UpdateAsync(user);
        }

        private LevelProgressInfo CalculateLevelProgress(int xp)
        {
            int level = 0;
            int currentLevelXp = 0;
            int nextLevelXp = 0;

            while (true)
            {
                nextLevelXp = GetXpForLevel(level + 1);
                if (xp < nextLevelXp)
                    break;

                currentLevelXp = nextLevelXp;
                level++;
            }

            double progress = (double)(xp - currentLevelXp) / (nextLevelXp - currentLevelXp);

            return new LevelProgressInfo
            {
                Level = level,
                TotalXp = xp,
                XpForNextLevel = nextLevelXp,
                Progress = Math.Round(progress, 2)
            };
        }

        private int GetXpForLevel(int level)
        {
            // Формула "престижу" - нелінійне зростання складності
            double rawXp = 50 * Math.Pow(level, 2.2);

            return RoundXpToNearestTen(rawXp);
        }

        private static int RoundXpToNearestTen(double xp)
        {
            return (int)Math.Ceiling(xp / 10.0) * 10;
        }

    }
}
