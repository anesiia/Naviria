package com.example.dyplomproject.data.remote

import android.util.Log
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
            if (response.isSuccessful) {
                Result.success(response.body()!!)
            } else {
                // Log the error details such as the response body or error message
                val errorBody = response.errorBody()?.string() ?: "No error body"
                Log.e(
                    "AuthRepository",
                    "Login failed with code: ${response.code()}, Error: $errorBody"
                )
                Result.failure(Exception("Login failed with code ${response.code()}: $errorBody"))
            }
        } catch (e: Exception) {
            // Log more detailed exception info
            Log.e("AuthRepository", "Login exception: ${e.message}", e)
            Result.failure(e)
        }
    }

    suspend fun register(request: UserRegistrationRequest): Result<AuthResponse> {
        return try {
            val response = apiService.register(request)
            if (response.isSuccessful) {
                Result.success(response.body()!!)
            } else {
                // Log the error details such as the response body or error message
                val errorBody = response.errorBody()?.string() ?: "No error body"
                Log.e(
                    "AuthRepository",
                    "Registration failed with code: ${response.code()}, Error: $errorBody"
                )
                Result.failure(Exception("Registration failed with code ${response.code()}: $errorBody"))
            }
        } catch (e: Exception) {
            // Log more detailed exception info
            Log.e("AuthRepository", "Registration exception: ${e.message}", e)
            Result.failure(e)
        }
    }
}