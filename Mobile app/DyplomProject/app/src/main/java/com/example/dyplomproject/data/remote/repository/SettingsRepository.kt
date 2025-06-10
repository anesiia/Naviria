package com.example.dyplomproject.data.remote.repository

import android.content.Context
import android.net.Uri
import com.example.dyplomproject.data.remote.dto.UserDto
import com.example.dyplomproject.data.remote.ApiService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.MultipartBody
import okhttp3.RequestBody
import okhttp3.RequestBody.Companion.toRequestBody
import java.io.InputStream

class SettingsRepository(private val api: ApiService) {
    /*
    suspend fun uploadProfilePhoto(
        userId: String,
        imageUri: Uri,
        context: Context
    ): Result<String> {
        return withContext(Dispatchers.IO) {
            try {
                val inputStream: InputStream? =
                    context.contentResolver.openInputStream(imageUri)
                val fileName = "profile.jpg"
                val mimeType = context.contentResolver.getType(imageUri) ?: "image/jpeg"

                val requestBody = inputStream?.readBytes()?.let {
                    RequestBody.create(mimeType.toMediaTypeOrNull(), it)
                } ?: return@withContext Result.failure(Exception("Unable to read image"))

                val multipartBody = MultipartBody.Part.createFormData(
                    name = "file", // must match parameter name in .NET controller
                    filename = fileName,
                    body = requestBody
                )
                val response = api.uploadProfilePhoto(userId, multipartBody)
                if (response.isSuccessful) {
                    Result.success(response.body() ?: "No URL returned")
                } else {
                    val errorBody = response.errorBody()?.string()
                    Result.failure(Exception("Upload failed: $errorBody"))
                }

            } catch (e: Exception) {
                Result.failure(e)
            }
        }
    }*/
    suspend fun uploadProfilePhoto(userId: String, imageUri: Uri, context: Context): Boolean {
        return try {
            val inputStream = context.contentResolver.openInputStream(imageUri) ?: return false
            val bytes = inputStream.readBytes()
            val requestBody = bytes.toRequestBody("image/*".toMediaTypeOrNull())
            val photoPart = MultipartBody.Part.createFormData("file", "profile.jpg", requestBody)

            val response = api.uploadProfilePhoto(userId, photoPart)
            response.isSuccessful
        } catch (e: Exception) {
            e.printStackTrace()
            false
        }
    }
    suspend fun modifyUser(id: String, modifiedUserDto: UserDto): Result<Unit> {
        return try {
            val response = api.modifyUser(id, modifiedUserDto)
            if(response.isSuccessful) {
                Result.success(Unit)
            } else {
                Result.failure(throw Exception("Не вдалось модифікувати користувача"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}
