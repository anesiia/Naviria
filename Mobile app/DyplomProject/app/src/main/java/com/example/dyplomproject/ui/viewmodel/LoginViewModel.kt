package com.example.dyplomproject.ui.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.repository.AuthRepository
import com.example.dyplomproject.data.remote.dto.LoginRequestDto
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.flow.update

data class LoginUiState(
    val email: String = "",
    val password: String = "",
    val isLoading: Boolean = false,
    val error: String? = null,
    val fieldErrors: Map<String, String> = emptyMap()
)

class LoginViewModel(
    private val authRepository: AuthRepository
) : ViewModel() {
    private val _uiState = MutableStateFlow(LoginUiState())
    val uiState: StateFlow<LoginUiState> = _uiState.asStateFlow()//good practice to use asStateFlow()

    fun updateField(field: String, value: String) {
        _uiState.update {
            when (field) {
                "email" -> it.copy(email = value)
                "password" -> it.copy(password = value)
                else -> it
            }
        }
    }

    fun onLoginClicked(authViewModel: AuthViewModel) {
        val state = _uiState.value
        if (!validateAndUpdateErrors(state)) {
            return
        }

        launchDataLoad {
            val result = authRepository.login(LoginRequestDto(state.email, state.password))
            //val result = authRepository.login(LoginRequest("elison@gmail.com", "Eli78@son"))
            //val result = authRepository.login(LoginRequest("alexander.davis@example.com", "Alex1234"))
            //val result = authRepository.login(LoginRequest("ben.barnes@gmail.com", "Ben12345"))
            result.onSuccess { response ->
                val token = response.token
                authViewModel.onLoginSuccess(token)
                Log.d("HTTPS", "Login successful: $token")
            }.onFailure { exception ->
                showError(exception.message ?: "Сталася невідома помилка")
                //showError("Login failed: ${exception.message}")
                Log.e("HTTPS", "Login error", exception)
            }
        }
    }

    fun validateAndUpdateErrors(state: LoginUiState): Boolean {
        val errors = mutableMapOf<String, String>()
        val emailRegex = Regex("^[\\w.-]+@[\\w.-]+\\.[a-zA-Z]{2,}$")

        if (state.email.isBlank()) {
            errors["email"] = "Пошта обов’язкова."
        } else if (!emailRegex.matches(state.email)) {
            errors["email"] = "Неправильний формат пошти."
        }

        if (state.password.isBlank()) {
            errors["password"] = "Пароль обов’язковий."
        } else {
            if (state.password.length < 8) {
                errors["password"] = "Пароль має містити щонайменше 8 символів, здається ви забули пароль трошки"
            } else {
                if (!state.password.any { it.isLowerCase() }) {
                    errors["password"] = "Пароль повинен містити хоча б одну велику літеру, здається ви забули пароль трошки"
                }
                if (!state.password.any { it.isUpperCase() }) {
                    errors["password"] = "Пароль повинен містити хоча б одну маленьку літеру, здається ви забули пароль трошки"
                }
            }
        }
        _uiState.update { it.copy(fieldErrors = errors) }
        return errors.isEmpty()
    }

    private fun showError(message: String) {
        _uiState.update { it.copy(error = message) }
    }

    private fun launchDataLoad(block: suspend () -> Unit) {
        viewModelScope.launch {
            _uiState.update { it.copy(isLoading = true) }
            try {
                block()
            } catch (e: Exception) {
                showError(e.message ?: "Unknown error")
            } finally {
                _uiState.update { it.copy(isLoading = false) }
            }
        }
    }
}