package com.example.dyplomproject.ui.viewmodel

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.AssistantChatMessage
import com.example.dyplomproject.data.remote.repository.AssistantChatRepository
import kotlinx.coroutines.launch
import java.time.Instant

class AssistantChatViewModel(
    private val repository: AssistantChatRepository,
    private val userId: String
) : ViewModel() {

    private val _messages = mutableStateListOf<AssistantChatMessage>()
    val messages: List<AssistantChatMessage> = _messages

    var userInput by mutableStateOf("")
    var isTaskRequest by mutableStateOf(false)
    //val userId = "user-123"

    init {
        loadChatHistory()
    }

    private fun loadChatHistory() {
        viewModelScope.launch {
            val history = repository.getChatHistory(userId)
            _messages.addAll(history)
        }
    }

    fun sendMessage() {
        val input = userInput.trim()
        if (input.isEmpty()) return

        val userMessage = AssistantChatMessage(
            userId = userId,
            message = input,
            role = "user",
            createdAt = Instant.now().toString(),
            isTaskRequest = isTaskRequest
        )

        _messages.add(userMessage)
        userInput = ""

        viewModelScope.launch {
            val response = repository.sendMessage(userMessage)
            response?.let {
                val aiMessage = AssistantChatMessage(
                    userId = userId,
                    message = it.response,
                    role = "assistant",
                    createdAt = Instant.now().toString(),
                    isTaskRequest = false
                )
                _messages.add(aiMessage)
            }
        }
    }
}