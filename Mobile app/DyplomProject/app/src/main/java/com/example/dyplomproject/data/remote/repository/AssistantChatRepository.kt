package com.example.dyplomproject.data.remote.repository

import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.AssistantChatMessage
import com.example.dyplomproject.data.remote.AssistantChatResponse

class AssistantChatRepository(
    private val api: ApiService
) {
    suspend fun getChatHistory(userId: String): List<AssistantChatMessage> {
        return try {
            val response = api.getUserAssistantChatHistory(userId)
            if (response.isSuccessful) response.body() ?: emptyList()
            else emptyList()
        } catch (e: Exception) {
            emptyList()
        }
    }

    suspend fun sendMessage(message: AssistantChatMessage): AssistantChatResponse? {
        return try {
            val response = api.askAssistant(message)
            if (response.isSuccessful) response.body()
            else null
        } catch (e: Exception) {
            null
        }
    }
}