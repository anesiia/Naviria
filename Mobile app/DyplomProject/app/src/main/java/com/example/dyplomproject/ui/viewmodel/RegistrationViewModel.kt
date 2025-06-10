package com.example.dyplomproject.ui.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.repository.AuthRepository
import com.example.dyplomproject.data.remote.dto.RegistrationRequestDto
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
    val error: String? = null,
    val fieldErrors: Map<String, String> = emptyMap()
)

class RegistrationViewModel(private val authRepository: AuthRepository) : ViewModel() {
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
                "confirmPassword" -> it.copy(confirmPassword = value)
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

    //    private fun validate(state: RegistrationUiState): String? {
//        val emailRegex = Regex("^[\\w.-]+@[\\w.-]+\\.[a-zA-Z]{2,}$")
//
//        return when {
//            state.fullName.isBlank() -> "Full Name is required."
//            state.email.isBlank() -> "Email is required."
//            !emailRegex.matches(state.email) -> "Invalid email format."
//            state.password.length < 8 -> "Password must be at least 8 characters."
//            state.password != state.confirmPassword -> "Passwords do not match."
//            //state.birthDate == null -> "Birth date is required."
//            else -> null
//        }
//    }

    fun validateAndUpdateErrors(state: RegistrationUiState): Boolean {
        val errors = mutableMapOf<String, String>()
        val emailRegex = Regex("^[\\w.-]+@[\\w.-]+\\.[a-zA-Z]{2,}$")
        val fullName = state.fullName.trim()
        val today = LocalDate.now()

        if (fullName.isBlank()) {
            errors["fullName"] = "Ім’я та прізвище обов’язкове."
        } else {
            val nameRegex = Regex("^[a-zA-Zа-яА-ЯіІїЇєЄґҐ]+\\s[a-zA-Zа-яА-ЯіІїЇєЄґҐ]+$")

            if (!fullName.matches(nameRegex)) {
                errors["fullName"] = "Ім’я та прізвище повинні складатися лише з літер і містити пробіл між ними."
            } else if (fullName.length < 3 || fullName.length > 20) {
                errors["fullName"] = "Ім’я та прізвище повинні бути від 3 до 20 символів."
            }
        }

        if (state.birthDate == null) {
            errors["birthDate"] = "Дата народження обов’язкова."
        } else {
            val age = java.time.Period.between(state.birthDate, today).years
            if (age < 18) {
                errors["birthDate"] = "Вам має бути щонайменше 18 років."
            }
        }

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

        if (state.password != state.confirmPassword) {
            errors["confirmPassword"] = "Паролі не співпадають."
        }

        if (state.nickname.isBlank()) {
            errors["nickname"] = "Нікнейм обов’язковий."
        } else if (!state.nickname.matches(Regex("^[a-zA-Z0-9]+$"))) {
            errors["nickname"] = "Нікнейм повинен містити лише латинські букви та цифри."
        }

        _uiState.update { it.copy(fieldErrors = errors) }

        return errors.isEmpty()
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

        if (!validateAndUpdateErrors(state)) {
            return
        }
        val request = RegistrationRequestDto(
            fullName = state.fullName,
            nickname = state.nickname,
            gender = state.gender,
            birthDate = formatBirthDateToUtcString(state.birthDate!!),
            email = state.email,
            password = state.password,
            futureMessage = state.futureMessage
        )

        //TEST DATA
//        val requestTest = UserRegistrationRequest(
//            fullName = "Мартін Час",
//            nickname = "bogdan17",
//            gender = "m",
//            birthDate = formatBirthDateToUtcString(LocalDate.parse("1993-04-22")!!),
//            email = "elison1@gmail.com",
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
                //showError("Login failed: ${exception.message}")
                showError(exception.message ?: "Сталася невідома помилка")
                Log.e("HTTPS", "Login error", exception)
            }
        }
    }
}