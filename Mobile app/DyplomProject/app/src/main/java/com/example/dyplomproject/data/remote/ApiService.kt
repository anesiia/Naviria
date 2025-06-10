package com.example.dyplomproject.data.remote

import com.example.dyplomproject.data.remote.dto.SubtaskDto
import com.example.dyplomproject.data.remote.dto.TaskCreateDto
import com.example.dyplomproject.data.remote.dto.TaskDto
import com.example.dyplomproject.data.remote.dto.UserDto
import com.example.dyplomproject.data.remote.dto.UserAchievementDto
import com.example.dyplomproject.data.remote.dto.LoginRequestDto
import com.example.dyplomproject.data.remote.dto.RegistrationRequestDto
import com.example.dyplomproject.data.remote.dto.AuthResponseDto
import com.example.dyplomproject.data.remote.dto.CategoryPieCharSliceDto
import com.example.dyplomproject.data.remote.dto.CompletedTasksLineChartElementDto
import com.example.dyplomproject.data.remote.dto.TopUserDto
import okhttp3.MultipartBody
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.Multipart
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Part
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
    suspend fun login(@Body loginRequestDto: LoginRequestDto) : Response<AuthResponseDto>

    @POST("/api/User")
    suspend fun register(@Body registerRequest: RegistrationRequestDto) : Response<AuthResponseDto>

    @GET("/api/Friends/{id}")
    suspend fun getFriends(@Path("id") id: String): Response<List<UserDto>>//<FriendShortDto>>

    @GET("/api/Friends/{id}/potential-friends")
    suspend fun getAllUsers(@Path("id") id: String) : Response<List<UserDto>>

    @GET("/api/User/{id}")
    suspend fun getUserInfo(@Path("id") id: String): Response<UserDto>

    @GET("/api/Achievements/user/{id}")
    suspend fun getUserAchievements(@Path("id") id: String): Response<List<UserAchievementDto>>


    @PUT("/api/User/{id}")
    suspend fun modifyUser(@Path("id") id: String, @Body modifiedUserDto: UserDto): Response<Unit>

    @POST("/api/FriendRequest")
    suspend fun sendFriendRequest(@Body friendRequest: FriendRequest): Response<FriendRequestResponse>

    @GET("/api/FriendRequest/incoming/{id}")
    suspend fun getIncomingUserRequest(@Path("id") id: String): Response<List<IncomingFriendRequestDto>>

    @PUT("/api/FriendRequest/{id}")
    suspend fun respondToFriendRequest(@Path("id") friendRequestId: String , @Body status: Map<String, String>): Response<Unit>

    //FOLDERS + TASKS
    @GET("/api/Folder/user/{id}")
    suspend fun getUserFolders(@Path("id") id: String): Response<List<FolderDto>>

    @PUT("/api/Folder/{id}")
    suspend fun updateFolder(@Path("id") id: String, @Body updateFolderRequest: UpdateFolderRequest): Response<Unit>

    @POST("/api/Folder")
    suspend fun createFolder(@Body createFolderRequest: CreateFolderRequest): Response<CreateFolderResponse>

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

    @PUT("/api/tasks/{taskId}/subtasks/{subtaskId}")
    suspend fun updateSubtask(@Path("taskId") taskId: String, @Path("subtaskId") subtaskId: String, @Body updatedTask: SubtaskDto): Response<Unit>

    @GET("/api/Task/grouped/user/{id}")
    suspend fun getFoldersWithTasks(@Path("id") userId: String): Response<List<FolderWithTasksDto>>

    @POST("/api/Task/{taskId}/checkin")
    suspend fun checkInTask(
        @Path("taskId") taskId: String,
        @Body checkInDateDto: CheckInDateDto
    ): Response<TaskDto>

    @POST("/api/Task/{taskId}/subtasks/{subtaskId}/checkin")
    suspend fun checkInSubtask(
        @Path("taskId") taskId: String,
        @Path("subtaskId") subtaskId: String,
        @Body checkInDateDto: CheckInDateDto
    ): Response<TaskDto>

    @GET("/api/AssistantChat/user/{userId}")
    suspend fun getUserAssistantChatHistory(@Path("userId") userId: String): Response<List<AssistantChatMessage>>

    @POST("/api/AssistantChat/ask")
    suspend fun askAssistant(@Body assistantChatMessage: AssistantChatMessage): Response<AssistantChatResponse>

    @PUT("/api/Notification/user/{userId}/mark-all-read")
    suspend fun markNotificationsAsRead(@Path("userId") userId: String): Response<Unit>

    @GET("/api/Notification/user/{userId}")
    suspend fun getUserNotifications(@Path("userId") userId: String): Response<List<UserNotification>>

    @GET("/api/UserSearch/search-all")
    suspend fun searchAmongAllUsers(
        @Query("userId") userId: String,
        @Query("categoryId") categoryId: String,
        @Query("query") query: String
    ): Response<List<UserDto>>

    @GET("/api/UserSearch/search-friends")
    suspend fun searchAmongFriends(
        @Query("userId") userId: String,
        @Query("categoryId") categoryId: String,
        @Query("query") query: String
    ): Response<List<UserDto>>

    @POST("/api/User/support/from-{senderId}/to-{receiverId}")
    suspend fun sendFriendSupport(
        @Path("senderId") senderId: String,
        @Path("receiverId") receiverId: String,
    ): Response<Unit>

    @DELETE("/api/Friends/{fromUserId}/to/{friendId}")
    suspend fun removeUserFromFriends(
        @Path("fromUserId") userId: String,
        @Path("friendId") friendId: String,
    ): Response<Unit>

    @GET("/api/StatisticGeneral/users/count")
    suspend fun getUsersCount(): Response<Map<String, Int>>

    @GET("/api/StatisticGeneral/tasks/count")
    suspend fun getTasksCount(): Response<Map<String, Int>>

    @GET("/api/StatisticGeneral/tasks/completed-percentage")
    suspend fun getCompletedTasksPercentageCount(): Response<Map<String, Int>>

    @GET("/api/StatisticGeneral/users/{userId}/days-since-registration")
    suspend fun getDaysCountFromUserRegistration(@Path("userId") userId: String): Response<Map<String, Int>>

    @GET("/api/StatisticGeneral/days-since-birthday")
    suspend fun getDaysCountFromAppCreation(): Response<Map<String, Int>>

    @GET("/api/StatisticsTaskByDate/user/{userId}/completed/monthly")
    suspend fun getUserCompletedTasksPerMonthStatistics(@Path("userId") userId: String): Response<List<CompletedTasksLineChartElementDto>>

    @GET("/api/StatisticsTaskByDate/user/{userId}/friends/completed/monthly")
    suspend fun getFriendsCompletedTasksPerMonthStatistics(@Path("userId") userId: String): Response<List<CompletedTasksLineChartElementDto>>

    @GET("/api/StatisticsTaskByDate/global/completed/monthly")
    suspend fun getGlobalCompletedTasksPerMonthStatistics(): Response<List<CompletedTasksLineChartElementDto>>

    @GET("/api/StatisticByCategory/user/{userId}/piechart")
    suspend fun getUserCategoryStatistics(@Path("userId") userId: String): Response<List<CategoryPieCharSliceDto>>

    @GET("/api/StatisticByCategory/user/{userId}/friends/piechart")
    suspend fun getFriendsCategoryStatistics(@Path("userId") userId: String): Response<List<CategoryPieCharSliceDto>>

    @GET("/api/StatisticByCategory/global/piechart")
    suspend fun getGlobalCategoryStatistics(): Response<List<CategoryPieCharSliceDto>>

    @GET("/api/Leaderboard/top")
    suspend fun getLeaderboard(): Response<List<TopUserDto>>
/*
    @Multipart
    @POST("api/User/{userId}/upload-profile-photo")
    suspend fun uploadProfilePhoto(
        @Path("userId") userId: String,
        @Part file: MultipartBody.Part
    ): Response<String>
*/
    @Multipart
    @POST("api/User/{userId}/upload-profile-photo")
    suspend fun uploadProfilePhoto(
        @Path("userId") userId: String,
        @Part photo: MultipartBody.Part
    ): Response<Unit>
}

data class UserNotification(
    val id: String,
    val userId: String,
    val text: String,
    val recievedAt: String,
    val isNew: Boolean
)

data class AssistantChatMessage(
    val userId: String,
    val message: String,
    val role: String,//user or assistant
    val createdAt: String, //"2025-06-04T01:10:49.407Z"
    val isTaskRequest: Boolean
)

data class AssistantChatResponse(
    val response: String
)

data class CheckInDateDto(
    val date: String
)

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

data class FriendRequest(
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
    val sender: UserDto//userDto
)

