package com.example.dyplomproject.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.dto.UserAchievement
import com.example.dyplomproject.data.remote.repository.UserRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch

data class AchievementsUiState(
    val achievements: List<UserAchievement> = emptyList(),
    val isLoading: Boolean = false,
    val errorMessage: String? = null
)

class AchievementsViewModel(
    private val repository: UserRepository,
    private val userId: String
) : ViewModel() {

    private val _uiState = MutableStateFlow(AchievementsUiState())
    val uiState: StateFlow<AchievementsUiState> = _uiState.asStateFlow()

    init {
        loadAchievements()
    }
    fun loadAchievements() {
        viewModelScope.launch {
            _uiState.update { it.copy(isLoading = true, errorMessage = null) }
            val result = repository.getCompleteUserAchievements(userId)
            if (result.isSuccess) {
                _uiState.update { it.copy(achievements = result.getOrThrow(), isLoading = false) }
            } else {
                _uiState.update { it.copy(isLoading = false, errorMessage = result.exceptionOrNull()?.message) }
            }
        }
    }

    fun onGetPointsClick(achievementId: String) {
        // Тут можна оновити achievement, наприклад змінити isPointsReceived
        val updated = _uiState.value.achievements.map {
            if (it.id == achievementId && it.isPointsReceived != true) {
                it.copy(isPointsReceived = true)
            } else it
        }
        _uiState.update { it.copy(achievements = updated) }
    }
}