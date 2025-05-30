package com.example.dyplomproject.ui.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.FriendRequest
//import androidx.room.util.copy
import com.example.dyplomproject.data.utils.RetrofitInstance
import com.example.dyplomproject.data.remote.repository.UserRepository
import kotlinx.coroutines.flow.MutableSharedFlow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asSharedFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch

data class FriendsUiState(
    val selectedTab: FriendsViewModel.TabType = FriendsViewModel.TabType.FRIENDS,
    val friends: List<UserShortUiModel> = emptyList(),
    val allUsers: List<UserShortUiModel> = emptyList(),
    val isLoading: Boolean = false,
    val error: String? = null,
    val hasNewFriendRequests: Boolean = false,
    val searchQuery: String = "",
    val searchResults: List<UserShortUiModel>? = null,
)

data class UserShortUiModel(
    val id: String,
    val fullName: String,
    val nickname: String,
    val isOnline: Boolean,
    val isProUser: Boolean,
    val isRequestSent: Boolean
)

//sealed class FriendsField {
//    data class SelectedTab(val value: FriendsViewModel.TabType) : FriendsField()
//    data class Friends(val value: List<UserShortUiModel>) : FriendsField()
//    data class AllUsers(val value: List<UserShortUiModel>) : FriendsField()
//    data class IsLoading(val value: Boolean) : FriendsField()
//    data class Error(val value: String?) : FriendsField()
//    data class HasNewFriendRequests(val value: Boolean) : FriendsField()
//}

class FriendsViewModel(
    private val repository: UserRepository = UserRepository(RetrofitInstance.api),
    private val userId: String
) : ViewModel() {
//WRONG WAY
//    // Use MutableStateFlow internally to manage state
//    private val _uiState = MutableStateFlow(FriendsUiState())
//    // Expose as StateFlow for UI observation
//    val uiState: StateFlow<FriendsUiState> get() = _uiState
    private val _uiState = MutableStateFlow(FriendsUiState())
    val uiState: StateFlow<FriendsUiState> = _uiState.asStateFlow()

    private val _messageFlow = MutableSharedFlow<String>()
    val messageFlow = _messageFlow.asSharedFlow()

    //probably i don't need that method because most of the fields are updated SAFELY IN OTHER FUNCTIONS
//    fun updateField(field: FriendsField) {
//        _uiState.update { current ->
//            when (field) {
//                is FriendsField.SelectedTab -> current.copy(selectedTab = field.value)
//                is FriendsField.Friends -> current.copy(friends = field.value)
//                is FriendsField.AllUsers -> current.copy(allUsers = field.value)
//                is FriendsField.IsLoading -> current.copy(isLoading = field.value)
//                is FriendsField.Error -> current.copy(error = field.value)
//                is FriendsField.HasNewFriendRequests -> current.copy(hasNewFriendRequests = field.value)
//            }
//        }
//    }

    fun onTabSelected(tab: TabType) {
        // Update selected tab and load the appropriate data
        _uiState.value = _uiState.value.copy(selectedTab = tab)
        when (tab) {
            TabType.FRIENDS -> if (_uiState.value.friends.isEmpty()) loadFriends()
            TabType.ALL_USERS -> if (_uiState.value.allUsers.isEmpty()) loadAllUsers()
        }
    }

    private fun loadFriends() = launchWithStateUpdate {
        val result = repository.getFriends(userId)
        _uiState.value = if (result.isSuccess) {
            _uiState.value.copy(friends = result.getOrThrow(), error = null)
        } else {
            _uiState.value.copy(error = result.exceptionOrNull()?.message)
        }
        Log.d("FriendList", "Friends size: ${uiState.value.friends.size}")
    }

    private fun loadAllUsers() = launchWithStateUpdate {
        val result = repository.getAllUsers(userId)
        _uiState.value = if (result.isSuccess) {
            _uiState.value.copy(allUsers = result.getOrThrow(), error = null)
        } else {
            _uiState.value.copy(error = result.exceptionOrNull()?.message)
        }
        Log.d("ALL USERS", ": ${uiState.value.allUsers.size}")
    }

//    fun onAddFriend(user: UserShortUiModel) {
//        // Create a modified copy of the user
//        val updatedUser = user.copy(isRequestSent = true)
//        // Now you can update your state with the modified user
//        // Update the state, for example:
//        val updatedList = _uiState.value.allUsers.map {
//            if (it.id == updatedUser.id) updatedUser else it
//        }
//        _uiState.value = _uiState.value.copy(allUsers = updatedList)
//    }

    private fun launchWithStateUpdate(block: suspend () -> Unit) {
        viewModelScope.launch {
            _uiState.value = _uiState.value.copy(isLoading = true)
            try {
                block()
            } finally {
                _uiState.value = _uiState.value.copy(isLoading = false)
            }
        }
    }

    enum class TabType {
        FRIENDS, ALL_USERS
    }

    fun checkNewFriendRequests(userId: String) = launchWithStateUpdate {
        val result = repository.getIncomingUserRequests(userId)
        _uiState.value = result.fold(
            onSuccess = { list ->
                if (list.isEmpty()) {
                    _uiState.value.copy(hasNewFriendRequests = false)
                } else {
                    _uiState.value.copy(hasNewFriendRequests = true)
                }
            },
            onFailure = { exception ->
                _uiState.value.copy(error = exception.message)
            }
        )
    }

    fun sendFriendRequest(user: UserShortUiModel) = launchWithStateUpdate {
        val request = FriendRequest(userId, user.id)
        val result = repository.sendFriendRequest(request)
        if (result.isSuccess) {
            val updatedUser = user.copy(isRequestSent = true)
            val updatedList = _uiState.value.allUsers.map {
                if (it.id == updatedUser.id) updatedUser else it
            }
            _uiState.value = _uiState.value.copy(allUsers = updatedList)
            _messageFlow.emit("Запит до користувача \"${user.nickname}\" надіслано!")
        } else {
            _messageFlow.emit("Сталася помилка!")
        }
    }

    fun onSearchQueryChanged(query: String) {
        _uiState.update { it.copy(searchQuery = query) }
    }

    fun resetSearch() {
        _uiState.update {
            it.copy(searchQuery = "", searchResults = null)
        }
    }

    fun searchUsers() = launchWithStateUpdate {
        val query = _uiState.value.searchQuery.trim()
        if (query.isEmpty()) {
            _uiState.update { it.copy(searchResults = null) }
            return@launchWithStateUpdate
        }

        val result = when (_uiState.value.selectedTab) {
            TabType.FRIENDS -> repository.searchFriends(userId, query)
            TabType.ALL_USERS -> repository.searchAllUsers(userId, query)
        }

        _uiState.update {
            it.copy(
                searchResults = if (result.isSuccess) {
                    result.getOrNull() ?: emptyList()  // If successful, get the list of users, otherwise an empty list
                } else {
                    emptyList()  // If failure, return an empty list
                },
                error = result.exceptionOrNull()?.message ?: "Unknown error"  // Capture the error message if available
            )
        }
    }
}



//I HAVE TO DECIDE THIS, DO I NEED ADDITIONAL MODEL FOR A FRIEND
//data class FriendUiModel(
//    val friendSince: String,
//    val friendshipStatus: String,
//    id: String,
//    fullName: String,
//    nickname: String,
//    isOnline: Boolean,
//    isProUser: Boolean
//) : UserShortUiModel(id, fullName, nickname, isOnline, isProUser)