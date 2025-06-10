package com.example.dyplomproject.ui.viewmodel

import android.content.Context
import android.net.Uri
import androidx.compose.runtime.mutableStateOf
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.repository.SettingsRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch


data class SettingsViewState(
    val isUploading: Boolean = false,
    val uploadSuccess: Boolean? = null,
    val selectedImageUri: Uri? = null,
    val errorMessage: String? = null
)

class SettingsViewModel(
    private val repository: SettingsRepository,
    private val context: Context,
    private val userId: String
) : ViewModel() {

    //    private val _uiState = mutableStateOf(SettingsViewState())
//    val uiState: State<SettingsViewState> = _uiState
    private val _uiState = MutableStateFlow(SettingsViewState())
    val uiState: StateFlow<SettingsViewState> = _uiState.asStateFlow()

    fun onImageSelected(uri: Uri) {
        _uiState.value = _uiState.value.copy(
            selectedImageUri = uri,
            uploadSuccess = null
        )
        uploadPhoto(uri)
    }

    private fun uploadPhoto(uri: Uri) {
        viewModelScope.launch {
            _uiState.value = _uiState.value.copy(isUploading = true)

            val success = repository.uploadProfilePhoto(
                userId = userId, // or pass as param
                imageUri = uri,
                context = context
            )

            _uiState.value = _uiState.value.copy(
                isUploading = false,
                uploadSuccess = success
            )
        }
        /*
    private fun uploadPhoto(uri: Uri) {
        viewModelScope.launch {
            _uiState.value = _uiState.value.copy(
                isUploading = true, uploadSuccess = null, errorMessage = null
            )

            val result = repository.uploadProfilePhoto(
                userId = userId, imageUri = uri, context
            )

            _uiState.value = when {
                result.isSuccess -> {
                    _uiState.value.copy(
                        isUploading = false,
                        uploadSuccess = true//, uploadPhoto() = result.getOrNull()
                    )
                }

                result.isFailure -> {
                    _uiState.value.copy(
                        isUploading = false,
                        uploadSuccess = false,
                        errorMessage = result.exceptionOrNull()?.message ?: "Unknown error"
                    )
                }

                else -> {
                    _uiState.value.copy(isUploading = false)
                }
            }
        }
    }*/
    }
}