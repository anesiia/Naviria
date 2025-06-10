package com.example.dyplomproject.data.remote.repository

import android.util.Log
import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.Category
import com.example.dyplomproject.data.remote.FriendRequest
import com.example.dyplomproject.mappers.toUiModel
import com.example.dyplomproject.ui.viewmodel.IncomingRequestUiModel
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel

class FriendRepository(private val apiService: ApiService) {
    suspend fun getFriends(userId: String): Result<List<UserShortUiModel>> {
        return try {
            val response = apiService.getFriends(userId)
            if (response.isSuccessful) {
                val body = response.body() ?: emptyList()
                Log.d("GET FRIENDS", "FRIENDS: ${body}")
                Result.success(body.map { it.toUiModel() })
//                val mapped = body.map {
//                    UserUiModel(
//                        id = it.userId,
//                        fullName = "", // You could enrich this later
//                        nickname = it.nickname,
//                        isOnline = false,
//                        isProUser = false
//                    )
//                }
//                Result.success(mapped)
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun getAllUsers(userId: String): Result<List<UserShortUiModel>> {
        return try {
            val response = apiService.getAllUsers(userId)
            if (response.isSuccessful) {
                val body = response.body() ?: emptyList()
                Result.success(body.map { it.toUiModel() })
//                val mapped = body.map {
//                    UserUiModel(
//                        id = it.id,
//                        fullName = it.fullName,
//                        nickname = it.nickname,
//                        isOnline = it.isOnline,
//                        isProUser = it.isProUser
//                    )
//                }
//                Result.success(mapped)
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun sendFriendRequest(friendRequest: FriendRequest): Result<String> {
        return try {
            val response = apiService.sendFriendRequest(friendRequest)
            if (response.isSuccessful) {
                val body = response.body()
                val status = body?.status
                Result.success(status?: "error")
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun getIncomingUserRequests(userId: String): Result<List<IncomingRequestUiModel>> {
        return try {
            val response = apiService.getIncomingUserRequest(userId)
            if (response.isSuccessful) {
                val body = response.body() ?: emptyList()
                Result.success(body.map { it.toUiModel() })
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun respondToFriendRequest(requestId: String, status: Map<String, String>): Result<Unit> {
        return try {
            val response = apiService.respondToFriendRequest(requestId, status)
            if (response.isSuccessful) {
                Result.success(Unit)
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun searchAmongFriends(userId: String, categoryId: String, query: String): Result<List<UserShortUiModel>> {
        return try {
            val response = apiService.searchAmongFriends(userId, categoryId, query)
            if (response.isSuccessful) {
                val body = response.body() ?: emptyList()
                Result.success(body.map { it.toUiModel() })
            } else {
                Result.failure(Exception("Friend search failed: ${response.code()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun searchAmongAllUsers(userId: String, categoryId: String, query: String): Result<List<UserShortUiModel>> {
        return try {
            val response = apiService.searchAmongAllUsers(userId, categoryId, query)
            if (response.isSuccessful) {
                val body = response.body() ?: emptyList()
                Result.success(body.map { it.toUiModel() })
            } else {
                Result.failure(Exception("User search failed: ${response.code()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun getCategories(): Result<List<Category>> {
        return try {
            val response = apiService.getCategories()
            if (response.isSuccessful) {
                val body = response.body() ?: emptyList()
                Result.success(body)
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun sendFriendSupport(senderId: String, receiverId: String): Result<String> {
        return try {
            val response = apiService.sendFriendSupport(senderId, receiverId)
            if (response.isSuccessful) {
                Result.success("Підтримка надіслана!")
            } else {
                val messsage = when (response.code()) {
                    400 -> "Відправляти підтримку можна лише 1 раз на 24 години!"
                    500 -> "Сталась помилка під час надсилання підтримки спробуйте ще раз!"
                    else -> "Невідома помилка: ${response.code()}"
                }
                Result.failure(Exception(messsage))
            }
        } catch (e: Exception) {
            Result.failure(Exception("Не вдалось під'єднатися до сервера, перевірте підключення!"))
        }
    }

    suspend fun removeUserFromFriends(userId: String, friendId: String): Result<Unit> {
        return try {
            val response = apiService.removeUserFromFriends(userId, friendId)
            if (response.isSuccessful) {
                Result.success(Unit)
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}