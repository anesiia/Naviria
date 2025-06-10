package com.example.dyplomproject.ui.viewmodel

import android.util.Base64
import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.utils.DataStoreManager
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import org.json.JSONObject

class AuthViewModel(
    private val dataStoreManager: DataStoreManager
) : ViewModel() {

    private val _isAuthenticated = MutableStateFlow(false)
    val isAuthenticated: StateFlow<Boolean> = _isAuthenticated

    private val _userId = MutableStateFlow<String?>(null)
    val userId: StateFlow<String?> = _userId

    init {
        viewModelScope.launch {
            dataStoreManager.authTokenFlow.collect { token ->
                if (!token.isNullOrEmpty()) {
                    _isAuthenticated.value = true

                    val decoded = decodeJwt(token)
                    _userId.value = decoded["sub"] as? String
                } else {
                    _isAuthenticated.value = false
                    _userId.value = null
                }
            }
        }
    }

    fun decodeJwt(token: String): Map<String, Any?> {
        val parts = token.split(".")
        if (parts.size < 2) throw IllegalArgumentException("Invalid JWT token format")

        val payload = parts[1]
        val decodedBytes = Base64.decode(payload, Base64.URL_SAFE or Base64.NO_WRAP)
        val decodedString = String(decodedBytes, Charsets.UTF_8)

        val json = JSONObject(decodedString)
        val result = mutableMapOf<String, Any?>()
        json.keys().forEach { key ->
            result[key] = json.get(key)
        }
        return result
    }
//    init {
//        viewModelScope.launch {
//            dataStoreManager.authTokenFlow.collect { token ->
//                _isAuthenticated.value = !token.isNullOrEmpty()
//            }
//        }
//    }

    fun onLoginSuccess(token: String) {
        viewModelScope.launch {
            dataStoreManager.saveAuthToken(token)
            _isAuthenticated.value = true
            val decoded = decodeJwt(token)
            _userId.value = decoded["sub"] as? String
        }
    }

    fun logout() {
        viewModelScope.launch {
            dataStoreManager.clearAuthToken()
            _isAuthenticated.value = false
            _userId.value = null
        }
    }
}