package com.example.dyplomproject.data.remote.dto

data class TopUserDto(
    val userId: String,
    val fullName: String,
    val nickname: String,
    val level: Int,
    val points: Int,
    val completionRate: Float,
    val achievementsCount: Int,
    val photo: String
)

data class CategoryPieCharSliceDto(
    val categoryId: String,
    val categoryName: String,
    val value: Int
)

data class CompletedTasksLineChartElementDto(
    val year: Int,
    val month: Int,
    val completedCount: Int
)