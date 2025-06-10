package com.example.dyplomproject.ui.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.dto.UserDto
import com.example.dyplomproject.data.remote.dto.UserAchievement
import com.example.dyplomproject.data.remote.repository.UserRepository
import com.example.dyplomproject.utils.RetrofitInstance
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

data class ProfileViewState(
    val userDto: UserDto? = null,
    val achievements: List<UserAchievement>? = emptyList(),
    val isLoading: Boolean = true,
    val error: String? = null
)

class ProfileViewModel(
    private val repository: UserRepository = UserRepository(RetrofitInstance.api),
    private val userId: String
) : ViewModel() {
    private val _state = MutableStateFlow(ProfileViewState())
    val state: StateFlow<ProfileViewState> = _state.asStateFlow()


    init {
        loadProfile()
        loadAchievements()
    }

    private fun loadProfile() {
        viewModelScope.launch {
            val result = repository.getUserInfo(userId)

            if (result.isSuccess) {
                _state.value = _state.value.copy(
                    userDto = result.getOrNull(),
                    isLoading = false
                )
            } else {
                _state.value = _state.value.copy(
                    error = result.exceptionOrNull()?.message,
                    isLoading = false
                )
            }

            Log.d("Profile", "Profile: ${state.value.userDto}")
        }
    }


    private fun loadAchievements() {
        viewModelScope.launch {
            val result = repository.getCompleteUserAchievements(userId)

            if (result.isSuccess) {
                val achievements = result.getOrNull()
                _state.value = _state.value.copy(
                    achievements = achievements,
                    isLoading = false
                )
                Log.d("Profile", "IS SUCCESS: ${state.value.achievements}")
            } else {
                _state.value = _state.value.copy(
                    error = result.exceptionOrNull()?.message,
                    isLoading = false
                )
                Log.d("Profile", "ERROR : ${state.value.achievements}")
            }
            Log.d("Profile", "ACHIEVEMENTS: ${state.value.achievements}")
        }
    }
}