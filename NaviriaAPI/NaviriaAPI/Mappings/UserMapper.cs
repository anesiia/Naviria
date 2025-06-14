﻿using NaviriaAPI.Entities;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.DTOs.User;

namespace NaviriaAPI.Mappings
{
    public class UserMapper
    {
        // Entity → DTO (для відправки в API)
        public static UserDto ToDto(UserEntity entity) =>
            new UserDto {
                Id = entity.Id,
                FullName = entity.FullName,
                Gender = entity.Gender,
                Nickname = entity.Nickname,
                BirthDate = entity.BirthDate,
                Description = entity.Description,
                Achievements = entity.Achievements,
                Email = entity.Email,
                Password = entity.Password,
                Friends = entity.Friends,
                FutureMessage = entity.FutureMessage,
                Photo = entity.Photo,
                Points = entity.Points,
                LevelInfo = entity.LevelInfo,
                RegitseredAt = entity.RegitseredAt,
                LastSeen = entity.LastSeen,
                IsOnline = entity.IsOnline,
                IsProUser = entity.IsProUser
            };

        // 🟩 DTO → Entity (для збереження в БД)
        public static UserEntity ToEntity(UserDto dto) =>
        new UserEntity {
            Id = dto.Id,
            FullName = dto.FullName,
            Gender = dto.Gender,
            Nickname = dto.Nickname,
            BirthDate = dto.BirthDate,
            Description = dto.Description,
            Achievements = dto.Achievements,
            Email = dto.Email,
            Password = dto.Password,
            Friends = dto.Friends,
            FutureMessage = dto.FutureMessage,
            Photo = dto.Photo,
            Points = dto.Points,
            LevelInfo = dto.LevelInfo,
            RegitseredAt = dto.RegitseredAt,
            LastSeen = dto.LastSeen,
            IsOnline = dto.IsOnline,
            IsProUser = dto.IsProUser
        };

        // CreateDto → Entity (для створення)
        public static UserEntity ToEntity(UserCreateDto dto) =>
            new UserEntity
            {
                FullName = dto.FullName,
                Gender = dto.Gender,
                Nickname = dto.Nickname,
                BirthDate = dto.BirthDate.Date,
                Email = dto.Email,
                Password = dto.Password,

                Description = string.Empty,
                Achievements = new List<UserAchievementInfo>(),
                Friends = new List<UserFriendInfo>(),
                FutureMessage = dto.FutureMessage ?? string.Empty,
                Points = 0,
                LevelInfo = new LevelProgressInfo(),
                RegitseredAt = DateTime.UtcNow,
                LastSeen = DateTime.UtcNow,
                IsOnline = true,
                IsProUser = false
            };


        // UpdateDto → Entity (для оновлення)
        public static UserEntity ToEntity(string id, UserUpdateDto dto) =>
            new UserEntity {
                Id = id,
                FullName = dto.FullName,
                Nickname = dto.Nickname,
                Description = dto.Description,
                Achievements = dto.Achievements,
                Email = dto.Email,
                Password = dto.Password,
                Friends = dto.Friends,
                FutureMessage = dto.FutureMessage,
                Photo = dto.Photo,
                Points = dto.Points,
                LevelInfo = dto.LevelInfo,
                IsOnline = dto.IsOnline,
                IsProUser = dto.IsProUser
            };

        public static UserUpdateDto ToUpdateDto(UserDto userDto)
        {
            return new UserUpdateDto
            {
                FullName = userDto.FullName,
                Nickname = userDto.Nickname,
                Description = userDto.Description,
                Achievements = userDto.Achievements,
                Email = userDto.Email,
                Password = userDto.Password,
                Friends = userDto.Friends,
                FutureMessage = userDto.FutureMessage,
                Photo = userDto.Photo,
                Points = userDto.Points,
                LevelInfo = userDto.LevelInfo,
                LastSeen = userDto.LastSeen,
                IsOnline = userDto.IsOnline,
                IsProUser = userDto.IsProUser
            };
        }

        public static UserEntity ToEntity(string id, UserDto userDto)
        {
            var updateDto = ToUpdateDto(userDto);
            return ToEntity(id, updateDto);
        }

    }
}
