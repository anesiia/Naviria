package com.example.dyplomproject.ui.viewmodel

import androidx.compose.runtime.State
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.UserNotification
import com.example.dyplomproject.data.remote.repository.NotificationRepository
import kotlinx.coroutines.launch

class NotificationViewModel(
    private val repository: NotificationRepository,
    private val userId: String
) : ViewModel() {

    private val _notifications = mutableStateOf<List<UserNotification>>(emptyList())
    val notifications: State<List<UserNotification>> = _notifications

    val hasUnread = derivedStateOf {
        _notifications.value.any { it.isNew }
    }

    fun loadNotifications(onComplete: (() -> Unit)? = null) {
        viewModelScope.launch {
            val result = repository.getUserNotifications(userId)
            result?.let { _notifications.value = it }
            onComplete?.invoke()
        }
    }

    fun markAllAsRead() {
        viewModelScope.launch {
            if (repository.markAllAsRead(userId)) {
                _notifications.value = _notifications.value.map { it.copy(isNew = false) }
            }
        }
    }
}