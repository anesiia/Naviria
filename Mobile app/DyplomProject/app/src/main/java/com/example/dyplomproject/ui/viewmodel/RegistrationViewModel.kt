package com.example.dyplomproject.ui.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.AuthRepository
import com.example.dyplomproject.data.remote.request.LoginRequest
import com.example.dyplomproject.data.remote.request.UserRegistrationRequest
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import java.time.LocalDate
import java.time.ZoneOffset
import java.time.format.DateTimeFormatter

data class RegistrationUiState(
    val fullName: String = "",
    val nickname: String = "",
    val gender: String = "f",
    val birthDate: LocalDate? = null,
    val email: String = "",
    val password: String = "",
    val confirmPassword: String = "",
    val futureMessage: String = "",
    val isSubmitting: Boolean = false,
    val error: String? = null
)

class RegistrationViewModel (private val authRepository: AuthRepository): ViewModel()   {
    private val _uiState = MutableStateFlow(RegistrationUiState())
    val uiState: StateFlow<RegistrationUiState> = _uiState.asStateFlow()

    fun updateField(field: String, value: String) {
        _uiState.update {
            when (field) {
                "fullName" -> it.copy(fullName = value)
                "nickname" -> it.copy(nickname = value)
                "gender" -> it.copy(gender = value)
                "email" -> it.copy(email = value)
                "password" -> it.copy(password = value)
                "confirmPassword" -> it.copy(password = value)
                "futureMessage" -> it.copy(futureMessage = value)
                else -> it
            }
        }
    }

    fun updateBirthDate(date: LocalDate) {
        _uiState.update { it.copy(birthDate = date) }
    }

    fun formatBirthDateToUtcString(date: LocalDate): String {
        val startOfDay = date.atStartOfDay(ZoneOffset.UTC) // Midnight UTC
        return startOfDay.format(DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'"))
    }

    private fun validate(state: RegistrationUiState): String? {
        val emailRegex = Regex("^[\\w.-]+@[\\w.-]+\\.[a-zA-Z]{2,}$")

        return when {
            state.fullName.isBlank() -> "Full Name is required."
            state.email.isBlank() -> "Email is required."
            !emailRegex.matches(state.email) -> "Invalid email format."
            state.password.length < 8 -> "Password must be at least 8 characters."
            state.password != state.confirmPassword -> "Passwords do not match."
            //state.birthDate == null -> "Birth date is required."
            else -> null
        }
    }
//Registration failed with code 400: {"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred.","status":400,"errors":{"Nickname":["Nickname can only contain Latin letters and digits."]}
    private fun showError(msg: String) {
        _uiState.update { it.copy(error = msg) }
    }

    private fun launchDataLoad(block: suspend () -> Unit) {
        viewModelScope.launch {
            _uiState.update { it.copy(isSubmitting = true) }
            try {
                block()
            } catch (e: Exception) {
                showError(e.message ?: "Unknown error")
            } finally {
                _uiState.update { it.copy(isSubmitting = false) }
            }
        }
    }

    fun register(authViewModel: AuthViewModel) {
        val state = _uiState.value

        val error = validate(state)
        //VALIDATION
//        if (error != null) {
//            showError(error)
//            return
//        }
        if (state.birthDate == null) {
            _uiState.value = state.copy(error = "Birth date is required")
            return
        }
//        val formattedDate = formatBirthDateToUtcString(state.birthDate)
        val request = UserRegistrationRequest(
            fullName = state.fullName,
            nickname = state.nickname,
            gender = state.gender,
            //birthDate = formattedDate,
            birthDate = formatBirthDateToUtcString(state.birthDate),
            email = state.email,
            password = state.password,
            futureMessage = state.futureMessage
        )

//        TEST DATA
//        val requestTest = UserRegistrationRequest(
//            fullName = "Мартін Час",
//            nickname = "timetraveler",
//            gender = "m",
//            birthDate = formatBirthDateToUtcString(LocalDate.parse("1993-04-22")!!),
//            email = "martin.time@gmail.com",
//            password = "BackToTheFuture34234",
//            futureMessage = "Я вже тут, хоча ще не був."
//        )

        _uiState.update { it.copy(isSubmitting = true, error = null) }
        launchDataLoad {
            val result = authRepository.register(request)
            result.onSuccess { response ->
                val token = response.token
                authViewModel.onLoginSuccess(token)
                Log.d("HTTPS", "Login successful: $token")
            }.onFailure { exception ->
                showError("Login failed: ${exception.message}")
                Log.e("HTTPS", "Login error", exception)
            }
        }
    }
}