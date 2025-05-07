package com.example.dyplomproject.data.remote

import com.example.dyplomproject.data.remote.request.LoginRequest
import com.example.dyplomproject.data.remote.request.UserRegistrationRequest
import com.example.dyplomproject.data.remote.response.AuthResponse
import retrofit2.Response

class AuthRepository(private val apiService: ApiService) {
//    suspend fun login(email: String, password: String): Response<AuthResponse> {
//        return apiService.login(LoginRequest(email, password))
//    }
    suspend fun login(request: LoginRequest): Result<AuthResponse> {
        return try {
            val response = apiService.login(request)
            if (response.isSuccessful) Result.success(response.body()!!)
            else Result.failure(Exception("Login failed"))
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun register(request: UserRegistrationRequest): Result<AuthResponse> {
        return try {
            val response = apiService.register(request)
            if (response.isSuccessful) Result.success(response.body()!!)
            else Result.failure(Exception("Registration failed"))
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}