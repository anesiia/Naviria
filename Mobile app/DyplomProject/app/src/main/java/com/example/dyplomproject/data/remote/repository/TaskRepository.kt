package com.example.dyplomproject.data.remote.repository

import android.util.Log
import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.Category
import com.example.dyplomproject.data.remote.CreateFolderRequest
import com.example.dyplomproject.data.remote.FolderWithTasksDto
import com.example.dyplomproject.data.remote.TaskCreateDto
import com.example.dyplomproject.data.remote.TaskDto
import com.example.dyplomproject.data.remote.TaskStandardCreateDto
import com.example.dyplomproject.data.remote.UpdateFolderRequest
import com.example.dyplomproject.mappers.toUiModel
import com.example.dyplomproject.ui.screen.Task
import com.example.dyplomproject.ui.viewmodel.Folder
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel

class TaskRepository(private val apiService: ApiService) {
    suspend fun getUserFolders(userId: String): Result<List<Folder>> {
        return try {
            val response = apiService.getUserFolders(userId)
            if (response.isSuccessful) {
                val body = response.body() ?: emptyList()
                Result.success(body.map {it.toUiModel()})
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun getCategories(): Result<List<Category>> {
        return try {
            val response = apiService.getCategories()
            if (response.isSuccessful) {
                val body = response.body() ?: emptyList()
                Result.success(body)
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun createFolder(createFolderRequest: CreateFolderRequest): Result<Folder> {
        return try {
            val response = apiService.createFolder(createFolderRequest)
            if (response.isSuccessful) {
                val body = response.body()
                val folderDto = body?.folder
                if (folderDto != null) {
                    Log.d("Folder", "NOT NULL")
                    Result.success(folderDto.toUiModel())
                } else {
                    Log.d("Folder", "NULLLLLLL")
                    Result.failure(Exception("Response body is null"))
                }
            } else {
                Result.failure(Exception("Failed: ${response.code()} ${response.message()}"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun deleteFolder(folderId: String): Result<Unit> {
        return try {
            val response = apiService.deleteFolder(folderId)
            if (response.isSuccessful) {
                Result.success(Unit)
            } else {
                val errorMessage = response.errorBody()?.string() ?: "Невідома помилка"
                Result.failure(Exception("Не вдалося видалити папку: $errorMessage"))
            }
        } catch (e: Exception) {
            Result.failure(Exception("Помилка з'єднання з сервером: ${e.message}", e))
        }
    }

    suspend fun updateFolder(folderId: String, updateFolderRequest: UpdateFolderRequest): Result<Unit>{
        return try {
            val response = apiService.updateFolder(folderId, updateFolderRequest)
            if (response.isSuccessful) {
                Result.success(Unit)
            } else {
                val errorMessage = response.errorBody()?.string() ?: "Невідома помилка"
                Result.failure(Exception("Не вдалося оновити папку: $errorMessage"))
            }
        } catch (e: Exception) {
            Result.failure(Exception("Помилка з'єднання з сервером: ${e.message}", e))
        }
    }

    suspend fun createTask(task: TaskCreateDto): Result<Unit>{
        Log.d("TASK CREATING", task.toString())
        return try {
            val response = apiService.createTask(task)
            if (response.isSuccessful) {
                Result.success(Unit)
            } else {
                val errorMessage = response.errorBody()?.string() ?: "Невідома помилка"
                Log.d("TASK CREATING", errorMessage)
                Result.failure(Exception("Не вдалось додати завдання: $errorMessage"))
            }
        } catch (e: Exception) {
            Log.e("TASK CREATING", e.message?:"")
            Result.failure(Exception("Помилка з'єднання з сервером: ${e.message}", e))
        }
    }

    suspend fun deleteTask(taskId: String): Result<Unit>{
        return try {
            val response = apiService.deleteTask(taskId)
            if (response.isSuccessful) {
                Result.success(Unit)
            } else {
                val errorMessage = response.errorBody()?.string() ?: "Невідома помилка"
                Log.d("TASK DELETING", errorMessage)
                Result.failure(Exception("Не вдалось додати завдання: $errorMessage"))
            }
        } catch (e: Exception) {
            Log.e("TASK DELETING", e.message?:"")
            Result.failure(Exception("Помилка з'єднання з сервером: ${e.message}", e))
        }
    }

    suspend fun updateTask(taskId: String, updatedTask: TaskDto): Result<Unit> {
        Log.e("TASK UPDATING", updatedTask.toString())
        return try {
            val response = apiService.updateTask(taskId, updatedTask)
            if (response.isSuccessful) {
                Result.success(Unit)
            } else {
                val errorMessage = response.errorBody()?.string() ?: "Невідома помилка"
                Log.d("TASK UPDATING", errorMessage)
                Result.failure(Exception("Не вдалось додати завдання: $errorMessage"))
            }
        } catch (e: Exception) {
            Log.e("TASK UPDATING", e.message?:"")
            Result.failure(Exception("Помилка з'єднання з сервером: ${e.message}", e))
        }
    }

    suspend fun getFoldersWithTasks(userId: String): Result<List<FolderWithTasksDto>> {
        return try {
            val response = apiService.getFoldersWithTasks(userId)
            if (response.isSuccessful) {
                Log.e("GET FOLDERS WITH TASKS", response.body().toString())
                val folders = response.body() ?: emptyList()
                Result.success(folders)
            } else {
                Log.e("GET FOLDERS WITH TASKS", response.body().toString())
                Result.failure(Exception("Error: ${response.errorBody()?.string()}"))
            }
        } catch (e: Exception) {
            Log.e("GET FOLDERS WITH TASKS", e.message.toString())
            Result.failure(Exception("Server error: ${e.message}", e))
        }
    }
}