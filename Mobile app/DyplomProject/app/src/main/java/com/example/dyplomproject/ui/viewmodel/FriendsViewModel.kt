package com.example.dyplomproject.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.Category
import com.example.dyplomproject.data.remote.FriendRequest
//import androidx.room.util.copy
import com.example.dyplomproject.utils.RetrofitInstance
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
    val categories: List<Category> = emptyList(),
    val isLoading: Boolean = false,
    val error: String? = null,
    val hasNewFriendRequests: Boolean = false,
    val selectedCategory: Category = Category("", "За категоріями"),
    val searchQuery: String = "",
    val searchResults: List<UserShortUiModel>? = null,
)

data class UserShortUiModel(
    val id: String,
    val fullName: String,
    val nickname: String,
    val isOnline: Boolean,
    val isProUser: Boolean,
    val isRequestSent: Boolean,
    val photo: String
)

class FriendsViewModel(
    private val repository: UserRepository = UserRepository(RetrofitInstance.api),
    private val userId: String
) : ViewModel() {
    private val _uiState = MutableStateFlow(FriendsUiState())
    val uiState: StateFlow<FriendsUiState> = _uiState.asStateFlow()

    private val _messageFlow = MutableSharedFlow<String>()
    val messageFlow = _messageFlow.asSharedFlow()

    fun onTabSelected(tab: TabType) {
        resetSearch()
        // Update selected tab and load the appropriate data
        _uiState.value = _uiState.value.copy(selectedTab = tab)
        when (tab) {
            TabType.FRIENDS -> if (_uiState.value.friends.isEmpty()) loadFriends()
            TabType.ALL_USERS -> if (_uiState.value.allUsers.isEmpty()) loadAllUsers()
        }
    }

    fun onRefresh(){
        if (_uiState.value.selectedTab == TabType.FRIENDS) {
            loadFriends()
        } else {
            loadAllUsers()
        }
    }

    private fun loadFriends() = launchWithStateUpdate {
        val result = repository.getFriends(userId)
        _uiState.value = if (result.isSuccess) {
            _uiState.value.copy(friends = result.getOrThrow(), error = null)
        } else {
            _uiState.value.copy(error = result.exceptionOrNull()?.message)
        }
    }

    fun loadAllUsers() = launchWithStateUpdate {
        val result = repository.getAllUsers(userId)
        _uiState.value = if (result.isSuccess) {
            _uiState.value.copy(allUsers = result.getOrThrow(), error = null)
        } else {
            _uiState.value.copy(error = result.exceptionOrNull()?.message)
        }
    }

    fun loadCategories() = launchWithStateUpdate {
        val result = repository.getCategories()
        _uiState.value = if (result.isSuccess) {
            _uiState.value.copy(categories = result.getOrThrow(), error = null)
        } else {
            _uiState.value.copy(error = result.exceptionOrNull()?.message)
        }
    }

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

    fun sendFriendSupport(user: UserShortUiModel) = launchWithStateUpdate {
        val result = repository.sendFriendSupport(userId, user.id)
        if (result.isSuccess) {
            val updatedUser = user.copy(isRequestSent = true)
            val updatedList = _uiState.value.friends.map {
                if (it.id == updatedUser.id) updatedUser else it
            }
            _uiState.value = _uiState.value.copy(friends = updatedList)
            _messageFlow.emit("Підтримка для \"${user.nickname}\" надіслана!")
        } else {
            result.exceptionOrNull()?.message?.let { _messageFlow.emit(it + "\"${user.nickname}\" вже підтриманий!") }
        }
    }

    fun removeUserFromFriends(user: UserShortUiModel) = launchWithStateUpdate {
        val result = repository.removeUserFromFriends(userId, user.id)
        if (result.isSuccess) {
            val updatedList = _uiState.value.friends.filterNot { it.id == user.id }
            _uiState.value = _uiState.value.copy(friends = updatedList)
            _messageFlow.emit("Користувач \"${user.nickname}\" успішно видалений з друзів")
        } else {
            _messageFlow.emit("Не вдалось видалити \"${user.nickname}\" з друзів! Спробуйте ще раз!")
        }
    }

    fun onSearchQueryChanged(query: String) {
        _uiState.update { it.copy(searchQuery = query) }
    }

    fun resetSearch() {
        _uiState.update {
            it.copy(
                searchQuery = "",
                searchResults = null,
                selectedCategory = Category("", "За категоріями")
            )
        }
    }

    fun onSelectedCategoryChanged(category: Category) {
        _uiState.update { it.copy(selectedCategory = category) }
        searchUsers()
    }

    fun searchUsers() = launchWithStateUpdate {
        val query = _uiState.value.searchQuery.trim()
        val categoryId = _uiState.value.selectedCategory.id
        if (query.isEmpty() && categoryId.isEmpty()) {
            _uiState.update { it.copy(searchResults = null) }
            return@launchWithStateUpdate
        }

        val result = when (_uiState.value.selectedTab) {
            TabType.FRIENDS -> repository.searchAmongFriends(userId, categoryId, query)
            TabType.ALL_USERS -> repository.searchAmongAllUsers(userId, categoryId, query)
        }

        _uiState.update {
            it.copy(
                searchResults = if (result.isSuccess) {
                    result.getOrNull() ?: emptyList()
                } else {
                    emptyList()
                },
                error = result.exceptionOrNull()?.message ?: "Unknown error"
            )
        }
    }
}