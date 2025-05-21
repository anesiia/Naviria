package com.example.dyplomproject.ui.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.AuthRepository
import com.example.dyplomproject.data.remote.request.LoginRequest
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.flow.update

data class LoginUiState(
    val email: String = "",
    val password: String = "",
    val isLoading: Boolean = false,
    val error: String? = null
)

class LoginViewModel(
    private val authRepository: AuthRepository
) : ViewModel() {

//    private val _email = MutableStateFlow("")
//    val email: StateFlow<String> = _email
//    private val _password = MutableStateFlow("")
//    val password: StateFlow<String> = _password
//    private val _error = MutableStateFlow<String?>(null)
//    val error: StateFlow<String?> = _error

    private val _uiState = MutableStateFlow(LoginUiState())
    val uiState: StateFlow<LoginUiState> = _uiState


    fun onEmailChanged(newEmail: String) {
        //_email.value = newEmail
        _uiState.value = _uiState.value.copy(email = newEmail)
    }

    fun onPasswordChanged(newPassword: String) {
        //_password.value = newPassword
        _uiState.value = _uiState.value.copy(password = newPassword)
    }

//    fun onLoginClicked(authViewModel: AuthViewModel) {
//        val state = _uiState.value
//        viewModelScope.launch {
//            _uiState.value = state.copy(isLoading = true, error = null)
//            try {
//                //val response = authRepository.login(state.email, state.password)
//                val response = authRepository.login("elison@gmail.com", "Eli78@son")
//                if (response.isSuccessful && response.body() != null) {
//                    val token = response.body()!!.token
//                    authViewModel.onLoginSuccess(token)
//                    Log.d("HTTPS", "Response Code: $response")
//                } else {
//                    _uiState.value = state.copy(isLoading = false, error = "Invalid credentials")
//                    Log.d("HTTPS", "Response Code: $response")
//                }
//            } catch (e: Exception) {
//                _uiState.value = state.copy(isLoading = false, error = "Login failed: ${e.message}")
//                Log.d("HTTPS", "Response Code: ${e.message}")
//            }
//        }
//    }

    fun onLoginClicked(authViewModel: AuthViewModel) {
        val state = _uiState.value
        val error = validate(state)
        if (error != null) {
            showError(error)
            return
        }

        launchDataLoad {
            //val result = authRepository.login(LoginRequest(state.email, state.password))
            //val result = authRepository.login(LoginRequest("elison@gmail.com", "Eli78@son"))
            val result = authRepository.login(LoginRequest("alexander.davis@example.com", "Alex1234"))
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

    private fun validate(state: LoginUiState): String? {
        return null
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