package com.example.dyplomproject.data.remote

import retrofit2.Response
//todo:redo the repositories classes logic and handle the server request logic here

class UserRepository(private val apiService: ApiService) {
    suspend fun getFriends(userId: String): Response<List<FriendShortDto>>{
        return apiService.getFriends(userId)
    }

    suspend fun getAllUsers(): Response<List<User>> {
        return apiService.getAllUsers()
    }

//    suspend fun getAllUsers(): Response<List<User>> {
//        return apiService.getAllUsers()
//    }

//    suspend fun getFriends(): List<User> = api.getFriends()
//    suspend fun getFriends(): List<User>
//    suspend fun getAllUsers(): Response<List<User>> {
//        return api.getAllUsers()
//    }
//    suspend fun getFriends(userId: String): List<FriendShortDto> {
//        val response = api.getFriends(userId)
//        return if (response.isSuccessful) {
//            response.body() ?: emptyList()
//        } else {
//            emptyList() // or throw an exception based on your error handling strategy
//        }
//    }
//
//    suspend fun getAllUsers(): List<User> {
//        val response = api.getAllUsers()
//        return if (response.isSuccessful) {
//            response.body() ?: emptyList()
//        } else {
//            emptyList() // or handle the error accordingly
//        }
//    }
}