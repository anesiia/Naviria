package com.example.dyplomproject.data.remote.dto

data class UserDto(
    val id: String,
    val fullName: String,
    val nickname: String,
    val gender: String,
    val birthDate: String,
    val description: String,
    val email: String,
    val password: String,
    val points: Int,
    val levelInfo: LevelInfo,
    val friends: List<FriendShortDto> = emptyList(),
    val achievements: List<AchievementDto> = emptyList(),
    val futureMessage: String,
    val photo: String,
    val regitseredAt: String,
    val lastSeen: String,
    val isOnline: Boolean,
    val isProUser: Boolean
)

data class LevelInfo(
    val level: Int,
    val totalXp: Int,
    val xpForNextLevel: Int,
    val progress: Double
)

data class AchievementDto(
    val achievementId: String,
    val receivedAt: String,
    val isPointsReceived: Boolean
)

data class FriendShortDto(
    val userId: String,
    val nickname: String
)

data class UserAchievementDto(
    val id: String,
    val name: String,
    val description: String,
    val points: Int,
    val isRare: Boolean
)

data class UserAchievement(
    val id: String,
    val name: String,
    val description: String,
    val points: Int,
    val isRare: Boolean,
    val receivedAt: String?,
    val isPointsReceived: Boolean?
)