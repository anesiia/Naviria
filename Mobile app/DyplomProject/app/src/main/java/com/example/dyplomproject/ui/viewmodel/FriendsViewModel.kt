package com.example.dyplomproject.ui.viewmodel

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.utils.RetrofitInstance
import com.example.dyplomproject.data.remote.UserRepository
import kotlinx.coroutines.launch

class FriendsViewModel(
    private val repository: UserRepository = UserRepository(RetrofitInstance.api),
    private val userId: String
) : ViewModel() {

    var selectedTab by mutableStateOf(TabType.FRIENDS)
        private set

//    var friends by mutableStateOf<List<FriendShortDto>>(emptyList())
//        private set
//
//    var allUsers by mutableStateOf<List<User>>(emptyList())
//        private set
//
//    var isLoading by mutableStateOf(false)
//        private set

    private val _allUsers = mutableStateListOf<UserUiModel>()
    val allUsers: List<UserUiModel> get() = _allUsers

    private val _friends = mutableStateListOf<UserUiModel>()
    val friends: List<UserUiModel> get() = _friends

    var isLoading by mutableStateOf(false)
        private set

    fun onTabSelected(tab: TabType) {
        selectedTab = tab
        when (tab) {
            TabType.FRIENDS -> if (friends.isEmpty()) loadFriends()
            TabType.ALL_USERS -> if (allUsers.isEmpty()) loadAllUsers()
        }
    }

    private fun loadFriends() {
        isLoading = true
//        viewModelScope.launch {
////            try {
////                friends = repository.getFriends(userId)
////            } catch (e: Exception) {
////                // Обработай ошибку
////            } finally {
////                isLoading = false
////            }
//            try {
//                val response = repository.getFriends(userId)
//                if (response.isSuccessful && response.body() != null) {
//                    friends = response.body() ?: emptyList()
//                    //Log.d("HTTPS", "Response Code: $response")
//                } else {
//                    //_error.value = "Invalid credentials"
//                    //Toast.makeText(this, "Invalid credentials", Toast.LENGTH_SHORT).show()
//                }
//            } catch (e: Exception) {
//                //_error.value = "Login failed: ${e.message}"
//                //Toast.makeText(this, "Login failed: ${e.message}", Toast.LENGTH_SHORT).show()
//            }
//        }
        viewModelScope.launch {
            try {
                val response = repository.getFriends(userId)
                if (response.isSuccessful && response.body() != null) {
                    val friendsList = response.body()
                    _friends.clear()
                    if (friendsList != null) {
                        _friends.addAll(
                            friendsList.map {
                                UserUiModel(
                                    id = it.userId,
                                    fullName = "", // if needed, you can fetch more user info here
                                    nickname = it.nickname,
                                    isOnline = false,
                                    isProUser = false
                                )
                            }
                        )
                    }
                }
            } catch (e: Exception) {
                // handle error
            } finally {
                isLoading = false
            }
        }
    }

    //Toast.makeText(context, "This is a toast", Toast.LENGTH_SHORT).show()

    private fun loadAllUsers() {
//        isLoading = true
////        viewModelScope.launch {
////            try {
////                allUsers = repository.getAllUsers()
////            } catch (e: Exception) {
////                // Обработай ошибку
////            } finally {
////                isLoading = false
////            }
////        }
//        viewModelScope.launch {
//        try {
//            val response = repository.getAllUsers()
//            if (response.isSuccessful && response.body() != null) {
//                Log.d("HTTPS", "Response Code: $response")
//            } else {
//                allUsers = response.body() ?: emptyList()
//                //_error.value = "Invalid credentials"
//                //Toast.makeText(this, "Invalid credentials", Toast.LENGTH_SHORT).show()
//            }
//        } catch (e: Exception) {
//            //_error.value = "Login failed: ${e.message}"
//            //Toast.makeText(this, "Login failed: ${e.message}", Toast.LENGTH_SHORT).show()
//        }
//        }
        isLoading = true
        viewModelScope.launch {
            try {
                //val users = repository.getAllUsers()
                val response = repository.getAllUsers()
                if (response.isSuccessful && response.body() != null) {
                    val users = response.body()
                    _allUsers.clear()
                    if (users != null) {
                        _allUsers.addAll(
                            users.map {
                                UserUiModel(
                                    id = it.id,
                                    fullName = it.fullName,
                                    nickname = it.nickname,
                                    isOnline = it.isOnline,
                                    isProUser = it.isProUser
                                )
                            }
                        )
                    }
                }
            } catch (e: Exception) {
                // handle error
            } finally {
                isLoading = false
            }
        }
    }

    enum class TabType {
        FRIENDS, ALL_USERS
    }
}

data class UserUiModel(
    val id: String,
    val fullName: String,
    val nickname: String,
    val isOnline: Boolean,
    val isProUser: Boolean
)