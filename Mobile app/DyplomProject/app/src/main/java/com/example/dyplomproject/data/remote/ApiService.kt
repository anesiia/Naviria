package com.example.dyplomproject.data.remote

import com.example.dyplomproject.data.remote.request.LoginRequest
import com.example.dyplomproject.data.remote.request.UserRegistrationRequest
import com.example.dyplomproject.data.remote.response.AuthResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path

//
//private val retrofit = Retrofit.Builder().baseUrl("https://localhost:7172/")
//    .addConverterFactory(GsonConverterFactory.create())
//    .build()
//
//val naviriaSevrvice = retrofit.create(ApiService::class.java)

interface ApiService {

    @POST("/api/Auth/login")
    suspend fun login(@Body loginRequest: LoginRequest) : Response<AuthResponse>

    @POST("/api/User")
    suspend fun register(@Body registerRequest: UserRegistrationRequest) : Response<AuthResponse>

    @GET("/api/Friends/{id}")
    suspend fun getFriends(@Path("id") id: String): Response<List<FriendShortDto>>

    @GET("/api/User")
    suspend fun getAllUsers() : Response<List<User>>

    @GET("/api/User/{id}")
    suspend fun getUserInfo(@Path("id") id: String): Response<User>

    @DELETE("/api/User/{id}")
    suspend fun deleteUser(@Path("id") id: String): Response<Unit>

    @PUT("/api/User/{id}")
    suspend fun modifyUser(@Path("id") id: String, @Body modifiedUser: User): Response<Unit>

    @POST("/api/FriendRequest")
    suspend fun sendFriendRequest(@Body friendRequest: FriendRequest): Response<FriendRequestResponse>

    @GET("/api/FriendRequest/{id}")
    suspend fun getIncomingUserRequest(@Path("id") id: String): Response<User>

}

data class FriendRequest (
    val fromUserId: String,
    val toUserId: String
)

data class FriendRequestResponse(
    val id: String,
    val fromUserId: String,
    val toUserId: String,
    val status: String
)

data class IncomingFriendRequestDto(
    val request: FriendRequestInfo,
    val sender: User//userDto
)

data class FriendRequestInfo(
    val id: String,
    val fromUserId: String,
    val toUserId: String,
    val status: String
)