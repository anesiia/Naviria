package com.example.dyplomproject.data.remote.repository

import android.util.Log
import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.FriendRequest
import com.example.dyplomproject.data.remote.dto.UserDto
import com.example.dyplomproject.data.remote.dto.UserAchievement
import com.example.dyplomproject.data.remote.Category
import com.example.dyplomproject.mappers.toUiModel
import com.example.dyplomproject.ui.viewmodel.IncomingRequestUiModel
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel

//todo:redo the repositories classes logic and handle the server request logic here

class UserRepository(private val apiService: ApiService) {
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

    suspend fun getUserInfo(userId: String): Result<UserDto> {
        return try {
            val response = apiService.getUserInfo(userId)
            if (response.isSuccessful) {
                val body = response.body()
                if (body != null) {
                    Log.d("GET USER INFO", "USER: ${userId}")
                    Result.success(body)
                } else {
                    Log.d("GET USER INFO", "USER: NULLLLLLL")
                    Result.failure(Exception("Response body is null"))
                }
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun getCompleteUserAchievements(userId: String): Result<List<UserAchievement>> {
        return try {
            val userResponse = apiService.getUserInfo(userId)
            val achievementsResponse = apiService.getUserAchievements(userId)
            Log.e("ACHIEVEMENTS user", userResponse.toString())
            Log.e("ACHIEVEMENTS ACHIEVEMENTS", achievementsResponse.toString())
            if (userResponse.isSuccessful && achievementsResponse.isSuccessful) {
                val user = userResponse.body()
                val achievementMetaList = achievementsResponse.body()
                Log.e("ACHIEVEMENTS", user.toString())
                Log.e("ACHIEVEMENTS", achievementMetaList.toString())
                if (user != null && achievementMetaList != null) {
                    val userAchievementsMap = user.achievements.associateBy { it.achievementId }
                    val completeAchievements = achievementMetaList.map { meta ->
                        val userAchievement = userAchievementsMap[meta.id]
                        UserAchievement(
                            id = meta.id,
                            name = meta.name,
                            description = meta.description,
                            points = meta.points,
                            isRare = meta.isRare,
                            receivedAt = userAchievement?.receivedAt,
                            isPointsReceived = userAchievement?.isPointsReceived
                        )
                    }
                    Log.e("ACHIEVEMENTS AAAA", completeAchievements.toString())
                    Result.success(completeAchievements)
                } else {

                    Result.failure(Exception("Response body is null"))
                }
            } else {
                Log.e("ACHIEVEMENTS", achievementsResponse.code().toString())
                Result.failure(
                    Exception("API call failed: ${userResponse.code()} / ${achievementsResponse.code()}")
                )
            }
        } catch (e: Exception) {
            Log.e("ACHIEVEMENTS", e.toString())
            Result.failure(e)
        }
    }
}