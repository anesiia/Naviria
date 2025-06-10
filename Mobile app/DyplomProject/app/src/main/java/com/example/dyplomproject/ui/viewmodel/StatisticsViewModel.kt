package com.example.dyplomproject.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.dto.CategoryPieCharSliceDto
import com.example.dyplomproject.data.remote.dto.CompletedTasksLineChartElementDto
import com.example.dyplomproject.data.remote.dto.TopUserDto
import com.example.dyplomproject.data.remote.repository.StatisticsRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch

enum class StatisticsTab {
    USER, FRIENDS, GLOBAL
}

data class StatisticsUiState(
    val usersCount: Int = 0,
    val tasksCount: Int = 0,
    val completedTasksPercentage: Int = 0,
    val daysSinceUserRegistered: Int = 0,
    val daysSinceAppCreated: Int = 0,
    val userChart: List<CompletedTasksLineChartElementDto> = emptyList(),
    val friendsChart: List<CompletedTasksLineChartElementDto> = emptyList(),
    val globalChart: List<CompletedTasksLineChartElementDto> = emptyList(),
    val userCategoryChart: List<CategoryPieCharSliceDto> = emptyList(),
    val friendsCategoryChart: List<CategoryPieCharSliceDto> = emptyList(),
    val globalCategoryChart: List<CategoryPieCharSliceDto> = emptyList(),
    val leaderboard: List<TopUserDto> = emptyList(),
    val selectedTab: StatisticsTab = StatisticsTab.USER,
    val isLoading: Boolean = false,
    val error: String? = null
)

class StatisticsViewModel(
    private val repository: StatisticsRepository,
    private val userId: String
) : ViewModel() {

    private val _uiState = MutableStateFlow(StatisticsUiState())
    val uiState: StateFlow<StatisticsUiState> = _uiState.asStateFlow()

    init {
        loadAllStatistics()
    }

    fun loadAllStatistics() = launchWithLoading {
        fetchCounts()
        fetchCharts()
        fetchLeaderboard()
        fetchPieCharts()
    }

    private suspend fun fetchCounts() {
        _uiState.update {
            it.copy(
                usersCount = repository.getUsersCount().getOrDefault(0),
                tasksCount = repository.getTasksCount().getOrDefault(0),
                completedTasksPercentage = repository.getCompletedTasksPercentage().getOrDefault(0),
                daysSinceUserRegistered = repository.getDaysSinceUserRegistered(userId)
                    .getOrDefault(0),
                daysSinceAppCreated = repository.getDaysSinceAppCreated().getOrDefault(0)
            )
        }
    }

    private suspend fun fetchCharts() {
        val userChartResult = repository.getUserCompletedTasksPerMonth(userId)
        val friendsChartResult = repository.getFriendsCompletedTasksPerMonth(userId)
        val globalChartResult = repository.getGlobalCompletedTasksPerMonth()

        _uiState.update {
            it.copy(
                userChart = userChartResult.getOrDefault(emptyList()),
                friendsChart = friendsChartResult.getOrDefault(emptyList()),
                globalChart = globalChartResult.getOrDefault(emptyList())
            )
        }
    }

    private suspend fun fetchPieCharts() {
        val userChartResult = repository.getUserCategoryStatistics(userId)
        val friendsChartResult = repository.getFriendsCategoryStatistics(userId)
        val globalChartResult = repository.getGlobalCategoryStatistics()
        _uiState.update {
            it.copy(
                userCategoryChart = userChartResult.getOrDefault(emptyList()),
                friendsCategoryChart = friendsChartResult.getOrDefault(emptyList()),
                globalCategoryChart = globalChartResult.getOrDefault(emptyList())
            )
        }
    }

    private suspend fun fetchLeaderboard() {
        val leaderboardResult = repository.getLeaderboard()
        _uiState.update {
            it.copy(leaderboard = leaderboardResult.getOrDefault(emptyList()))
        }
    }

    fun changeTab(tab: StatisticsTab) {
        _uiState.update { it.copy(selectedTab = tab) }
    }

    private fun launchWithLoading(block: suspend () -> Unit) {
        viewModelScope.launch {
            _uiState.update { it.copy(isLoading = true, error = null) }
            try {
                block()
            } catch (e: Exception) {
                _uiState.update { it.copy(error = e.message ?: "Unknown error") }
            } finally {
                _uiState.update { it.copy(isLoading = false) }
            }
        }
    }
}
