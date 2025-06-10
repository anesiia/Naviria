package com.example.dyplomproject.data.remote.repository

import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.UserNotification

class NotificationRepository(private val api: ApiService) {//NotificationApi) {
    suspend fun getUserNotifications(userId: String): List<UserNotification>? {
        return try {
            val response = api.getUserNotifications(userId)
            if (response.isSuccessful) response.body() else null
        } catch (e: Exception) {
            null
        }
    }

    suspend fun markAllAsRead(userId: String): Boolean {
        return try {
            val response = api.markNotificationsAsRead(userId)
            response.isSuccessful
        } catch (e: Exception) {
            false
        }
    }
}