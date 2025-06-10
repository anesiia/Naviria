package com.example.dyplomproject.data.remote.repository

import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.dto.CategoryPieCharSliceDto
import com.example.dyplomproject.data.remote.dto.CompletedTasksLineChartElementDto
import com.example.dyplomproject.data.remote.dto.TopUserDto

class StatisticsRepository(private val apiService: ApiService) {

    suspend fun getUsersCount(): Result<Int> = safeApiCall {
        apiService.getUsersCount().body()?.get("totalUsers") ?: 0
    }

    suspend fun getTasksCount(): Result<Int> = safeApiCall {
        apiService.getTasksCount().body()?.get("totalTasks") ?: 0
    }

    suspend fun getCompletedTasksPercentage(): Result<Int> = safeApiCall {
        apiService.getCompletedTasksPercentageCount().body()?.get("completedPercentage") ?: 0
    }

    suspend fun getDaysSinceUserRegistered(userId: String): Result<Int> = safeApiCall {
        apiService.getDaysCountFromUserRegistration(userId).body()?.get("daysSinceRegistration") ?: 0
    }

    suspend fun getDaysSinceAppCreated(): Result<Int> = safeApiCall {
        apiService.getDaysCountFromAppCreation().body()?.get("daysSinceBirthday") ?: 0
    }

    suspend fun getUserCompletedTasksPerMonth(userId: String): Result<List<CompletedTasksLineChartElementDto>> =
        safeApiCall { apiService.getUserCompletedTasksPerMonthStatistics(userId).body().orEmpty() }

    suspend fun getFriendsCompletedTasksPerMonth(userId: String): Result<List<CompletedTasksLineChartElementDto>> =
        safeApiCall { apiService.getFriendsCompletedTasksPerMonthStatistics(userId).body().orEmpty() }

    suspend fun getGlobalCompletedTasksPerMonth(): Result<List<CompletedTasksLineChartElementDto>> =
        safeApiCall { apiService.getGlobalCompletedTasksPerMonthStatistics().body().orEmpty() }

    suspend fun getUserCategoryStatistics(userId: String): Result<List<CategoryPieCharSliceDto>> =
        safeApiCall { apiService.getUserCategoryStatistics(userId).body().orEmpty() }

    suspend fun getFriendsCategoryStatistics(userId: String): Result<List<CategoryPieCharSliceDto>> =
        safeApiCall { apiService.getFriendsCategoryStatistics(userId).body().orEmpty() }

    suspend fun getGlobalCategoryStatistics(): Result<List<CategoryPieCharSliceDto>> =
        safeApiCall { apiService.getGlobalCategoryStatistics().body().orEmpty() }

    suspend fun getLeaderboard(): Result<List<TopUserDto>> =
        safeApiCall { apiService.getLeaderboard().body().orEmpty() }

    private inline fun <T> safeApiCall(apiCall: () -> T): Result<T> {
        return try {
            Result.success(apiCall())
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}