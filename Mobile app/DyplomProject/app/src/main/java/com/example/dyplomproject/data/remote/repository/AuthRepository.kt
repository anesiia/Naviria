package com.example.dyplomproject.data.remote.repository

import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.dto.LoginRequestDto
import com.example.dyplomproject.data.remote.dto.RegistrationRequestDto
import com.example.dyplomproject.data.remote.dto.AuthResponseDto

class AuthRepository(private val apiService: ApiService) {
    suspend fun login(request: LoginRequestDto): Result<AuthResponseDto> {
        return try {
            val response = apiService.login(request)
            if (response.isSuccessful) {
                Result.success(response.body()!!)
            } else {
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
            Result.failure(Exception("Не вдалось під'єднатися до сервера, перевірте підключення!"))
        }
    }

    suspend fun register(request: RegistrationRequestDto): Result<AuthResponseDto> {
        return try {
            val response = apiService.register(request)
            if (response.isSuccessful) {
                Result.success(response.body()!!)
            } else {
                val userMessage = when (response.code()) {
                    409 -> "Пошта вже використовується! Ввійдіть в вже створений акаунт або використайте іншу пошту для реєстрації"
                    422 -> "Нікнейм зайнятий, вигадайте інший!"
                    500 -> "Пошта або нікнейм вже використовуються! Використовуйте іншу пошту або змініть нік"
                    else -> "Невідома помилка: ${response.code()}"
                }
                Result.failure(Exception(userMessage))
            }
        } catch (e: Exception) {
            Result.failure(Exception("Не вдалось під'єднатися до сервера, перевірте підключення!"))
        }
    }
}