package com.example.dyplomproject.data.remote

import com.example.dyplomproject.data.remote.request.LoginRequest
import com.example.dyplomproject.data.remote.response.LoginResponse
import retrofit2.Response
import javax.inject.Inject

class AuthRepository(private val apiService: ApiService) {
    suspend fun login(email: String, password: String): Response<LoginResponse> {
        return apiService.login(LoginRequest(email, password))
    }
}