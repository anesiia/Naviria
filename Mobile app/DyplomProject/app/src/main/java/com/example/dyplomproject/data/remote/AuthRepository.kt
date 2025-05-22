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
                //val errorBody = response.errorBody()?.string() ?: "No error body"
//                Log.e(
//                    "AuthRepository",
//                    "Login failed with code: ${response.code()}, Error: $errorBody"
//                )
//                Result.failure(Exception("Login failed with code ${response.code()}: $errorBody"))
                val userMessage = when (response.code()) {
                    400 -> "Некоректні дані. Перевірте заповнення полів."
                    401 -> "Невірний email або пароль."
                    403 -> "Доступ заборонено."
                    404 -> "Сервер не знайдено."
                    500 -> "Помилка сервера. Спробуйте пізніше."
                    else -> "Невідома помилка: ${response.code()}"
                }
                Result.failure(Exception(userMessage))
            }
        } catch (e: Exception) {
//            Log.e("AuthRepository", "Login exception: ${e.message}", e)
//            Result.failure(e)
            Result.failure(Exception("Не вдалось під'єднатися до сервера, перевірте підключення!"))
        }
    }

    suspend fun register(request: UserRegistrationRequest): Result<AuthResponse> {
        return try {
            val response = apiService.register(request)
            if (response.isSuccessful) {
                Result.success(response.body()!!)
            } else {
//                val errorBody = response.errorBody()?.string() ?: "No error body"
//                Log.e(
//                    "AuthRepository",
//                    "Registration failed with code: ${response.code()}, Error: $errorBody"
//                )
//                Result.failure(Exception("Registration failed with code ${response.code()}: $errorBody"))
                val userMessage = when (response.code()) {
                    409 -> "Пошта вже використовується! Ввійдіть в вже створений акаунт або використайте іншу пошту для реєстрації"
                    422 -> "Нікнейм зайнятий, вигадайте інший!"
                    500 -> "Пошта або нікнейм вже використовуються! Використовуйте іншу пошту або змініть нік"
                    else -> "Невідома помилка: ${response.code()}"
                }
                Result.failure(Exception(userMessage))
            }
        } catch (e: Exception) {
//            Log.e("AuthRepository", "Registration exception: ${e.message}", e)
//            Result.failure(e)
            Result.failure(Exception("Не вдалось під'єднатися до сервера, перевірте підключення!"))
        }
    }
}