package com.example.dyplomproject.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.repository.UserRepository
import kotlinx.coroutines.flow.MutableSharedFlow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asSharedFlow
import kotlinx.coroutines.launch

data class IncomingRequestUiModel(
    val requestId: String,
    val fromUserId: String,
    val toUserId: String,
    val status: String,
    val sender: UserShortUiModel
)

class FriendRequestsViewModel(
    private val repository: UserRepository,
    private val userId: String
) : ViewModel() {

    private val _requests = MutableStateFlow<List<IncomingRequestUiModel>>(emptyList())
    val requests: StateFlow<List<IncomingRequestUiModel>> = _requests
    private val _messageFlow = MutableSharedFlow<String>()
    val messageFlow = _messageFlow.asSharedFlow()
    //?????what is the difference between state flow and shared flow

    fun loadRequests() {
        viewModelScope.launch {
            val result = repository.getIncomingUserRequests(userId)
            if (result.isSuccess) {
                _requests.value = result.getOrThrow()
            }
        }
    }

    fun declineFriendRequest(requestId: String) {
        viewModelScope.launch {
            val result = repository.respondToFriendRequest(requestId, mapOf("status" to "rejected"))
            if (result.isSuccess) {
                _requests.value = _requests.value.filterNot { it.requestId == requestId }
                _messageFlow.emit("Запит успішно відхилено")//Request declined successfully
            } else {
                _messageFlow.emit("Невдалось відхилити запит у друзі!")//"Failed to decline request"
            }
        }
    }

    fun acceptFriendRequest(requestId: String) {
        viewModelScope.launch {
            val result = repository.respondToFriendRequest(requestId, mapOf("status" to "accepted"))
            if (result.isSuccess) {
                _requests.value = _requests.value.filterNot { it.requestId == requestId }
                _messageFlow.emit("Запит успішно підтверджено")//Request declined successfully
            } else {
                _messageFlow.emit("Невдалось  підтвердити запит у друзі!")//"Failed to decline request"
            }
        }
    }
}
