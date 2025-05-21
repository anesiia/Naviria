package com.example.dyplomproject.data.remote

import android.util.Log
import com.example.dyplomproject.mappers.toUiModel
import com.example.dyplomproject.ui.viewmodel.IncomingRequestUiModel
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel
import retrofit2.Response
//todo:redo the repositories classes logic and handle the server request logic here

class UserRepository(private val apiService: ApiService) {
//    suspend fun getFriends(userId: String): Response<List<FriendShortDto>> {
//        return apiService.getFriends(userId)
//    }
//
//    suspend fun getAllUsers(): Response<List<User>> {
//        return apiService.getAllUsers()
//    }
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

    suspend fun getAllUsers(): Result<List<UserShortUiModel>> {
        return try {
            val response = apiService.getAllUsers()
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

    suspend fun declineFriendRequest(requestId: String): Result<Unit> {
        return try {
            val response = apiService.declineFriendRequest(requestId)
            if (response.isSuccessful) {
                Result.success(Unit)
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun searchFriends(userId: String, query: String): Result<List<UserShortUiModel>> {
        return try {
            val response = apiService.searchFriends(userId, query)
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

    suspend fun searchAllUsers(userId: String, query: String): Result<List<UserShortUiModel>> {
        return Result.success(emptyList())
    }

    //    suspend fun searchAllUsers(query: String): Result<List<UserShortUiModel>> {
//        return try {
//            val response = api.searchAllUsers(query)
//            if (response.isSuccessful) {
//                Result.success(response.body() ?: emptyList())
//            } else {
//                Result.failure(Exception("User search failed: ${response.code()}"))
//            }
//        } catch (e: Exception) {
//            Result.failure(e)
//        }
//    }
    suspend fun getUserInfo(userId: String): Result<User> {
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

            if (userResponse.isSuccessful && achievementsResponse.isSuccessful) {
                val user = userResponse.body()
                val achievementMetaList = achievementsResponse.body()

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

                    Result.success(completeAchievements)
                } else {
                    Result.failure(Exception("Response body is null"))
                }
            } else {
                Result.failure(
                    Exception("API call failed: ${userResponse.code()} / ${achievementsResponse.code()}")
                )
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}