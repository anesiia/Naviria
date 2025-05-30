package com.example.dyplomproject.data.remote

import com.example.dyplomproject.data.remote.request.LoginRequest
import com.example.dyplomproject.data.remote.request.UserRegistrationRequest
import com.example.dyplomproject.data.remote.response.AuthResponse
import org.junit.experimental.categories.Categories
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path
import retrofit2.http.Query

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
    suspend fun getFriends(@Path("id") id: String): Response<List<User>>//<FriendShortDto>>

    @GET("/api/Friends/{id}/potential-friends")
    suspend fun getAllUsers(@Path("id") id: String) : Response<List<User>>

    @GET("/api/User/{id}")
    suspend fun getUserInfo(@Path("id") id: String): Response<User>

    @GET("/api/Achievements/user/{id}")
    suspend fun getUserAchievements(@Path("id") id: String): Response<List<UserAchievementDto>>

    @DELETE("/api/User/{id}")
    suspend fun deleteUser(@Path("id") id: String): Response<Unit>

    @PUT("/api/User/{id}")
    suspend fun modifyUser(@Path("id") id: String, @Body modifiedUser: User): Response<Unit>

    @POST("/api/FriendRequest")
    suspend fun sendFriendRequest(@Body friendRequest: FriendRequest): Response<FriendRequestResponse>

    @GET("/api/FriendRequest/incoming/{id}")
    suspend fun getIncomingUserRequest(@Path("id") id: String): Response<List<IncomingFriendRequestDto>>

//    @DELETE("/api/FriendRequest/{id}")
//    suspend fun declineFriendRequest(@Path("id") id: String): Response<Unit>

    @PUT("/api/FriendRequest/{id}")
    suspend fun respondToFriendRequest(@Path("id") friendRequestId: String , @Body status: Map<String, String>): Response<Unit>

    @GET("/api/Friends/{userId}/searchFriends")
    suspend fun searchFriends(@Path("userId") id: String,@Query("query") query: String): Response<List<User>>

    @GET("/api/")
    suspend fun searchUsers(@Path("userId") id: String,@Query("query") query: String): Response<Unit>

    //FOLDERS + TASKS
    @GET("/api/Folder/user/{id}")
    suspend fun getUserFolders(@Path("id") id: String): Response<List<FolderDto>>

    @PUT("/api/Folder/{id}")
    suspend fun updateFolder(@Path("id") id: String, @Body updateFolderRequest: UpdateFolderRequest): Response<Unit>

    @POST("/api/Folder")
    suspend fun createFolder(@Body createFolderRequest: CreateFolderRequest ): Response<CreateFolderResponse>

    @DELETE("/api/Folder/{id}")
    suspend fun deleteFolder(@Path("id") id: String): Response<Unit>

    @GET("/api/Category")
    suspend fun getCategories(): Response<List<Category>>

    @POST("/api/Task")
    suspend fun createTask(@Body task: TaskCreateDto): Response<Unit>

    @DELETE("/api/Task/{id}")
    suspend fun deleteTask(@Path("id") taskId: String): Response<Unit>

    @PUT("/api/Task/{id}")
    suspend fun updateTask(@Path("id") taskId: String, @Body updatedTask: TaskDto): Response<Unit>

    @GET("/api/Task/grouped/user/{id}")
    suspend fun getFoldersWithTasks(@Path("id") userId: String): Response<List<FolderWithTasksDto>>

}

data class FolderWithTasksDto(
    val folderId: String,
    val folderName: String,
    val tasks: List<TaskDto>
)

data class Tag(
    val TagName: String
)

data class Category(
    val id: String = "",
    val name: String = ""
)

data class CreateFolderRequest(
    val userId: String,
    val name: String
)

data class CreateFolderResponse(
    val folder: FolderDto
)

data class UpdateFolderRequest(
    val name: String
)

data class FolderDto (
    val id: String,
    val userId: String,
    val name: String,
    val createdAt: String
)

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
    val request: FriendRequestResponse,
    val sender: User//userDto
)

//data class FriendRequstStatus (
//    val status: String
//)

//data class FriendRequestInfo(
//    val id: String,
//    val fromUserId: String,
//    val toUserId: String,
//    val status: String
//)