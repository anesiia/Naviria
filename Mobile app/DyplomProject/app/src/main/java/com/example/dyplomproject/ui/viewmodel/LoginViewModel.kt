package com.example.dyplomproject.ui.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.AuthRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch


class LoginViewModel (
    private val authRepository: AuthRepository) : ViewModel() {

    private val _email = MutableStateFlow("")
    val email: StateFlow<String> = _email

    private val _password = MutableStateFlow("")
    val password: StateFlow<String> = _password

    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error

    fun onEmailChanged(newEmail: String) {
        _email.value = newEmail
    }

    fun onPasswordChanged(newPassword: String) {
        _password.value = newPassword
    }

    fun onLoginClicked(authViewModel: AuthViewModel) {
        viewModelScope.launch {
            try {
                //val response = authRepository.login(_email.value, _password.value)
                val response = authRepository.login("elison@gmail.com", "Eli78@son")
                if (response.isSuccessful && response.body() != null) {
                    val token = response.body()!!.token
                    authViewModel.onLoginSuccess(token)
                    Log.d("HTTPS", "Response Code: $response")
                } else {
                    _error.value = "Invalid credentials"
                }
            } catch (e: Exception) {
                _error.value = "Login failed: ${e.message}"
            }
        }
    }
}
//    private val _rememberMe = MutableStateFlow(false)
//    val rememberMe: StateFlow<Boolean> = _rememberMe
//    fun onRememberMeChanged(isChecked: Boolean) {
//        _rememberMe.value = isChecked
//    }