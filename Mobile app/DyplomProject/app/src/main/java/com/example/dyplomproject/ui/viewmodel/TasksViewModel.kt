package com.example.dyplomproject.ui.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.dyplomproject.data.remote.Category
import com.example.dyplomproject.data.remote.CreateFolderRequest
import com.example.dyplomproject.data.remote.FolderWithTasksDto
import com.example.dyplomproject.data.remote.dto.SubtaskCreateDto
import com.example.dyplomproject.data.remote.dto.SubtaskRepeatableCreateDto
import com.example.dyplomproject.data.remote.dto.SubtaskScaleCreateDto
import com.example.dyplomproject.data.remote.dto.SubtaskScaleDto
import com.example.dyplomproject.data.remote.dto.SubtaskStandardCreateDto
import com.example.dyplomproject.data.remote.dto.TaskCreateDto
import com.example.dyplomproject.data.remote.dto.TaskDto
import com.example.dyplomproject.data.remote.dto.TaskRepeatableCreateDto
import com.example.dyplomproject.data.remote.dto.TaskRepeatableDto
import com.example.dyplomproject.data.remote.dto.TaskScaleCreateDto
import com.example.dyplomproject.data.remote.dto.TaskScaleDto
import com.example.dyplomproject.data.remote.dto.TaskStandardCreateDto
import com.example.dyplomproject.data.remote.dto.TaskStandardDto
import com.example.dyplomproject.data.remote.dto.TaskWithSubtasksCreateDto
import com.example.dyplomproject.data.remote.dto.TaskWithSubtasksDto
import com.example.dyplomproject.data.remote.UpdateFolderRequest
import com.example.dyplomproject.data.remote.repository.TaskRepository
import com.example.dyplomproject.mappers.toTagList
import com.example.dyplomproject.ui.components.task.create.SubtaskFormState
import com.example.dyplomproject.ui.components.task.create.TaskCreationState
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch

data class Folder(
    val id: String,
    val name: String
)

data class TasksUiState(
    val folders: List<Folder> = emptyList(),
    val foldersWithTasks: List<FolderWithTasksDto> = emptyList(),
    val selectedFolder: Folder? = null,
    val categories: List<Category> = emptyList(),
    val isLoading: Boolean = false,
    val taskCreation: TaskCreationState = TaskCreationState(),
    val error: String? = null
)

class TasksViewModel(
    private val repository: TaskRepository, // your API access point
    private val userId: String
) : ViewModel() {

    private val _uiState = MutableStateFlow(TasksUiState())
    val uiState: StateFlow<TasksUiState> = _uiState.asStateFlow()

    // Reusable helper to manage loading and error states
    private inline fun launchWithLoading(
        crossinline block: suspend () -> Unit,
        crossinline onError: (String) -> Unit = { message ->
            _uiState.update { it.copy(error = message) }
        }
    ) {
        viewModelScope.launch {
            _uiState.update { it.copy(isLoading = true) }
            try {
                block()
            } catch (e: Exception) {
                onError(e.message ?: "Unknown error")
            } finally {
                _uiState.update { it.copy(isLoading = false) }
            }
        }
    }


    fun loadUserFolders(onComplete: () -> Unit = {}) {
        launchWithLoading(
            block = {
                val result = repository.getUserFolders(userId)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception(result.exceptionOrNull())
                }

                val folders = result.getOrNull().orEmpty()
                _uiState.update {
                    it.copy(
                        folders = folders,
                        selectedFolder = folders.firstOrNull(),
                        error = null
                    )
                }
                Log.d(
                    "ViewModel",
                    "THE COUNT OF FOLDERS AT THE START: ${uiState.value.folders.size}"
                )
                Log.d("LOAD ALL USER FOLDERS", "USER_ID: ${userId}")
                onComplete()
            }

        )
    }

    fun loadTasks() {//(onComplete: () -> Unit = {}) {
        launchWithLoading(
            block = {
                val result = repository.getFoldersWithTasks(userId)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception(result.exceptionOrNull())
                }

                val foldersWithTasks = result.getOrNull().orEmpty()
                _uiState.update {
                    it.copy(
                        foldersWithTasks = foldersWithTasks,
                        error = null
                    )
                }
                //onComplete()
            }

        )
    }

    fun loadCategories() {//(onComplete: () -> Unit = {}) {
        launchWithLoading(
            block = {
                val result = repository.getCategories()
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception(result.exceptionOrNull())
                }

                val categories = result.getOrNull().orEmpty()
                _uiState.update {
                    it.copy(
                        categories = categories,
                        error = null
                    )
                }
                //onComplete()
            }

        )
    }

    fun addFolderOnServer(folderName: String, onComplete: () -> Unit = {}) {
//        launchWithLoading(
//            block = {
//                val request = CreateFolderRequest(userId, folderName)
//                val result = repository.createFolder(request)
//                if (result.isFailure) {
//                    throw result.exceptionOrNull() ?: Exception(result.exceptionOrNull())
//                }
//
//                val folder = result.getOrNull() ?: throw Exception("Folder creation returned null")
//                Log.d("FOLDER CREATING", "FOLDER: ${folder.name}")
//                _uiState.update {
//                    it.copy(
//                        folders = it.folders + folder,
//                        selectedFolder = folder,
//                        error = null
//                    )
//                }
//                Log.d("ViewModel", "State updated: folders count = ${_uiState.value.folders.size}")
//                //Log.d("ViewModel", "Updated folders count: ${uiState.value.folders.size}")
//                onComplete()
//            }
//        )
        viewModelScope.launch {
            try {
                val request = CreateFolderRequest(userId, folderName)
                val result = repository.createFolder(request)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception("Unknown error")
                }

                val folder = result.getOrNull() ?: throw Exception("Folder creation returned null")
                Log.d("FOLDER CREATING", "FOLDER: ${folder.name}")

                _uiState.update {
                    it.copy(
                        folders = it.folders + folder,
                        selectedFolder = folder,
                        error = null
                    )
                }
                Log.d("ViewModel", "State updated: folders count = ${_uiState.value.folders.size}")
                onComplete()
            } catch (e: Exception) {
                Log.e("ViewModel", "Error adding folder", e)
            }
        }
    }

    fun updateFolderOnServer(folderId: String, newFolderName: String, onComplete: () -> Unit = {}) {
        launchWithLoading(
            block = {
                val request = UpdateFolderRequest(newFolderName)
                val result = repository.updateFolder(folderId, request)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception("Unknown error while updating folder")
                }
                Log.d("", "")
                //val updated = result.getOrNull() ?: throw Exception("Updated folder is null")

                _uiState.update {
                    it.copy(
                        folders = it.folders.map { folder ->
                            if (folder.id == folderId) folder.copy(name = newFolderName) else folder
                        },
                        error = null
                    )
                }
                onComplete()
            },
            onError = { msg ->
                _uiState.update {
                    it.copy(error = "Failed to update folder: $msg")
                }
            }
        )
    }

    fun deleteFolderOnServer(folderId: String, onComplete: () -> Unit = {}) {
        launchWithLoading(
            block = {
                val result = repository.deleteFolder(folderId)

                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception("Unknown error while deleting folder")
                }

                _uiState.update {
                    val newFolders = it.folders.filterNot { folder -> folder.id == folderId }
                    val newSelected = if (it.selectedFolder?.id == folderId) null else it.selectedFolder

                    it.copy(
                        folders = newFolders,
                        selectedFolder = newSelected,
                        error = null
                    )
                }

                onComplete()
            },
            onError = { msg ->
                _uiState.update {
                    it.copy(error = "Failed to delete folder: $msg")
                }
            }
        )
    }

    fun selectFolder(folder: Folder) {
        _uiState.update { it.copy(selectedFolder = folder) }
    }

    fun updateTaskCreation(update: TaskCreationState.() -> TaskCreationState) {
        _uiState.update { it.copy(taskCreation = it.taskCreation.update()) }
    }
    //Subtask
    fun SubtaskFormState.toDto(): SubtaskCreateDto? {
        if (title.isBlank()) return null

        return when (type) {
            "standard" -> SubtaskStandardCreateDto(
                title = title,
                description = description,
                isCompleted = false
            )
            "repeatable" -> if (repeatDays.isEmpty()) null else SubtaskRepeatableCreateDto(
                title = title,
                description = description,
                repeatDays = repeatDays
            )
            "scale" -> if (targetValue <= 0f) null else SubtaskScaleCreateDto(
                title = title,
                description = description,
                unit = unit,
                currentValue = currentValue,
                targetValue = targetValue
            )
            else -> null
        }
    }

    fun addSubtaskForm(): String {
        val form = SubtaskFormState(type = "standard")
        updateTaskCreation { copy(subtaskCreationForms = subtaskCreationForms + form) }
        return form.id
    }

//    fun updateSubtaskForm(id: String, update: SubtaskFormState) {
//        updateTaskCreation {
//            copy(subtaskCreationForms = subtaskCreationForms.map { if (it.id == id) update else it })
//        }
//    }

    fun updateSubtaskForm(id: String, transform: SubtaskFormState.() -> SubtaskFormState) {
        updateTaskCreation {
            copy(
                subtaskCreationForms = subtaskCreationForms.map { form ->
                    if (form.id == id) form.transform() else form
                }
            )
        }
    }

    fun removeSubtaskForm(id: String) {
        updateTaskCreation {
            copy(subtaskCreationForms = subtaskCreationForms.filterNot { it.id == id })
        }
    }

    fun prepareSubtasks(): List<SubtaskCreateDto> {
        return uiState.value.taskCreation.subtaskCreationForms.mapNotNull { it.toDto() }
    }

    //Subtask
    fun validateTask(): Boolean {
        val task = uiState.value.taskCreation
        val subtasks = uiState.value.taskCreation.subtaskCreationForms
        var isValid = true

        val titleError = if (task.title.isBlank()) {
            isValid = false
            Log.d("VALIDATION","Назва завдання об'язкова!")
            "Назва завдання об'язкова!"
        } else null

        //by default the category selected
//        val categoryError = if (task.selectedCategory.id.isBlank()) {
//            isValid = false
//            "Category must be selected"
//        } else null

        val unitValueError = if (task.type == "scale" && task.unit.isBlank()) {
            isValid = false
            Log.d("VALIDATION","\"Одиниця вимірювання обов'язкова!\"")
            "Одиниця вимірювання обов'язкова!"
        } else null

        val targetValueError = if (task.type == "scale" && task.targetValue <= 0.0) {
            isValid = false
            Log.d("VALIDATION","\"Значення цілі повинно бути вказано цілою цифрою більшу за 0\"")
            "Значення цілі повинно бути вказано цілою цифрою більшу за 0"//
        } else null

        val repeatDaysError = if (task.type == "repeatable" && task.repeatDays.isEmpty()) {
            isValid = false
            Log.d("VALIDATION","Оберіть що найменше один день для відмітки практикування задачі або змініть тип головної задачі!")
            "Оберіть що найменше один день для відмітки практикування задачі або змініть тип головної задачі!"
        } else null

        val subtasksError = if (task.type == "with_subtasks" && task.subtaskCreationForms.isEmpty()) {
            isValid = false
            Log.d("VALIDATION","Задача типу список повинна містити прийнамні одну підзадачу! Додайте задачу за допомогою кнопки \"Додати підзадачу\" або змініть тип головної задачі!")
            "Задача типу список повинна містити прийнамні одну підзадачу! Додайте задачу за допомогою кнопки \"Додати підзадачу\" або змініть тип головної задачі!"
        } else null

        val notificationDateError = if (task.isNotificationsOn && task.notificationDate.isBlank()) {
            isValid = false
            Log.d("VALIDATION","Оберіть дату дня та час нагадування!")
            "Оберіть дату дня та час нагадування!"
        } else null

        val deadlineDateError = if (task.isDeadlineOn && task.deadline.isBlank()) {
            isValid = false
            Log.d("VALIDATION"," \"Оберіть дату дня та час для дедлайну!\"")
            "Оберіть дату дня та час для дедлайну!"
        } else null
        updateTaskCreation {
            copy(
                titleError = titleError,
                //categoryError = categoryError,
                targetValueError = targetValueError,
                repeatDaysError = repeatDaysError,
                subtasksError = subtasksError,
                unitValueError = unitValueError,
                deadlineDateError = deadlineDateError,
                notificationDateError = notificationDateError
            )
        }
        if (task.type == "with_subtasks") {
            subtasks.forEach{ subtask ->
                val subtaskTitleError = if (subtask.title.isBlank()) {
                    isValid = false
                    Log.d("VALIDATION","\"Назва підзадачі об'язкова!\"")
                    "Назва підзадачі об'язкова!"
                } else null

                val subtaskUnitValueError = if (subtask.type == "scale" && subtask.unit.isBlank()) {
                    isValid = false
                    Log.d("VALIDATION","\"Одиниця вимірювання обов'язкова!\"")
                    "Одиниця вимірювання обов'язкова!"
                } else null

                val subtaskTargetValueError = if (subtask.type == "scale" && subtask.targetValue <= 0.0) {
                    isValid = false
                    Log.d("VALIDATION","\"Значення цілі повинно бути вказано цілою цифрою більшу за 0\"//")
                    "Значення цілі повинно бути вказано цілою цифрою більшу за 0"//
                } else null

                val subtaskRepeatDaysError = if (subtask.type == "repeatable" && subtask.repeatDays.isEmpty()) {
                    isValid = false
                    Log.d("VALIDATION","\"Оберіть що найменше один день для відмітки практикування підзадачі або змініть тип підзадачі!\"")
                    "Оберіть що найменше один день для відмітки практикування підзадачі або змініть тип підзадачі!"
                } else null

                updateSubtaskForm(subtask.id) {
                    copy(
                        titleError = subtaskTitleError,
                        repeatDaysError = subtaskRepeatDaysError,
                        unitValueError = subtaskUnitValueError,
                        targetValueError = subtaskTargetValueError
                    )
                }
            }
        }
        return isValid
    }

    fun createTask(onComplete: () -> Unit = {}) {
        if (!validateTask()) return
        val state = uiState.value
        val folderId = state.selectedFolder?.id ?: return
        val taskCreationState = state.taskCreation
        val taskCreation = uiState.value.taskCreation
        var task: TaskCreateDto;
        if (taskCreation.type == "standard") {
            task = TaskStandardCreateDto(
                userId = userId,
                folderId = folderId,
                title = state.taskCreation.title,
                description = state.taskCreation.description,
                categoryId = state.taskCreation.selectedCategory.id,
                tags = state.taskCreation.tags.toTagList(),
                isDeadlineOn = state.taskCreation.isDeadlineOn,
                deadline = if(state.taskCreation.isDeadlineOn) state.taskCreation.deadline else null,
                isShownProgressOnPage = state.taskCreation.isShownProgressOnPage,
                isNotificationsOn = state.taskCreation.isNotificationsOn,
                notificationDate = if(state.taskCreation.isNotificationsOn) state.taskCreation.notificationDate else null,
                priority = state.taskCreation.priority.level
            )
        } else if(taskCreation.type == "repeatable") {
            task = TaskRepeatableCreateDto(
                userId = userId,
                folderId = folderId,
                title = state.taskCreation.title,
                description = state.taskCreation.description,
                categoryId = state.taskCreation.selectedCategory.id,
                tags = state.taskCreation.tags.toTagList(),
                isDeadlineOn = state.taskCreation.isDeadlineOn,
                deadline = if(state.taskCreation.isDeadlineOn) state.taskCreation.deadline else null,
                isShownProgressOnPage = state.taskCreation.isShownProgressOnPage,
                isNotificationsOn = state.taskCreation.isNotificationsOn,
                notificationDate = if(state.taskCreation.isNotificationsOn) state.taskCreation.notificationDate else null,
                priority = state.taskCreation.priority.level,
                repeatDays = state.taskCreation.repeatDays
            )
        }
        else if(taskCreation.type == "scale") {
            task = TaskScaleCreateDto(
                userId = userId,
                folderId = folderId,
                title = taskCreationState.title,
                description = taskCreationState.description,
                categoryId = taskCreationState.selectedCategory.id,
                tags = taskCreationState.tags.toTagList(),
                isDeadlineOn = taskCreationState.isDeadlineOn,
                deadline = if(taskCreationState.isDeadlineOn) taskCreationState.deadline else null,
                isShownProgressOnPage = taskCreationState.isShownProgressOnPage,
                isNotificationsOn = taskCreationState.isNotificationsOn,
                notificationDate = if(taskCreationState.isNotificationsOn) state.taskCreation.notificationDate else null,
                priority = taskCreationState.priority.level,
                unit = taskCreationState.unit,
                currentValue = 0.0,
                targetValue = taskCreationState.targetValue
            )
        }
        else if(taskCreation.type == "with_subtasks") {
            task = TaskWithSubtasksCreateDto(
                userId = userId,
                folderId = folderId,
                title = taskCreationState.title,
                description = taskCreationState.description,
                categoryId = taskCreationState.selectedCategory.id,
                tags = taskCreationState.tags.toTagList(),
                isDeadlineOn = taskCreationState.isDeadlineOn,
                deadline = if(taskCreationState.isDeadlineOn) taskCreationState.deadline else null,
                isShownProgressOnPage = taskCreationState.isShownProgressOnPage,
                isNotificationsOn = taskCreationState.isNotificationsOn,
                notificationDate = if(taskCreationState.isNotificationsOn) state.taskCreation.notificationDate else null,
                priority = taskCreationState.priority.level,
                subtasks = prepareSubtasks()
            )
        }
        else  {
            return
        }
        launchWithLoading(
            block = {
                //val result = repository.createStandardTask(task)
                val result = repository.createTask(task)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception("Unknown error while creating task")
                }
                // Ideally reload or append task list here
                loadTasks()
                onComplete()
                // Optionally clear form after submission
                _uiState.update { it.copy(taskCreation = TaskCreationState()) }
            },
            onError = { msg ->
                _uiState.update { it.copy(error = "Failed to create task: $msg") }
            }
        )
    }

    fun deleteTask(taskId: String, onComplete: () -> Unit = {}) {
        launchWithLoading(
            block = {
                //val result = repository.createStandardTask(task)
                val result = repository.deleteTask(taskId)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception("Unknown error while creating task")
                }
                // Ideally reload or append task list here
                loadTasks()
                onComplete()
            },
            onError = { msg ->
                _uiState.update { it.copy(error = "Failed to create task: $msg") }
            }
        )
    }
//    fun copyTaskWithChanges(task: TaskDto, update: (TaskDto) -> TaskDto): TaskDto {
//        return update(task)
//    }
//    val updated = copyTaskWithChanges(task) {
//        when (it) {
//            is TaskStandardDto -> it.copy(title = "New Title", priority = 2)
//            is TaskRepeatableDto -> it.copy(title = "New Title", priority = 2)
//            is TaskScaleDto -> it.copy(title = "New Title", priority = 2)
//            is TaskWithSubtasksDto -> it.copy(title = "New Title", priority = 2)
//        }
//    }
//
//    fun updateTaskCreation(update: TaskCreationState.() -> TaskCreationState) {
//        _uiState.update { it.copy(taskCreation = it.taskCreation.update()) }
//    }
    fun updateTask(updateTask: TaskDto, onComplete: () -> Unit = {}) {
        launchWithLoading(
            block = {
                //val result = repository.createStandardTask(task)
                val result = repository.updateTask(updateTask.id, updateTask)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception("Unknown error while creating task")
                }
                // Ideally reload or append task list here
                loadTasks()
                onComplete()
            },
            onError = { msg ->
                _uiState.update { it.copy(error = "Failed to create task: $msg") }
            }
        )
    }

    fun updateTaskField(updateTask: TaskDto, onComplete: () -> Unit = {}) {
        launchWithLoading(
            block = {
                //val result = repository.createStandardTask(task)
                val result = repository.updateTask(updateTask.id, updateTask)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception("Unknown error while creating task")
                }
                // Ideally reload or append task list here
                loadTasks()
                onComplete()
            },
            onError = { msg ->
                _uiState.update { it.copy(error = "Failed to create task: $msg") }
            }
        )
    }

    fun copyTaskWithChanges(task: TaskDto, update: (TaskDto) -> TaskDto): TaskDto {
        return update(task)
    }

    fun updateTaskStatus(task: TaskDto, onComplete: () -> Unit = {}) {
        val updated = copyTaskWithChanges(task) {
            when (it) {
                is TaskStandardDto -> it.copy(status = "Completed")
                is TaskRepeatableDto -> it.copy(status = "Completed")
                is TaskScaleDto -> it.copy(status = "Completed")
                is TaskWithSubtasksDto -> it.copy(status = "Completed")
                else -> throw Exception("there's an error in updateTaskStatus")
            }
        }
        launchWithLoading(
            block = {
                val result = repository.updateTask(task.id, updated)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception("Unknown error while creating task")
                }
                loadTasks()
                onComplete()
            },
            onError = { msg ->
                _uiState.update { it.copy(error = "Failed to create task: $msg") }
            }
        )
    }

    fun updateTaskCurrentValue(task: TaskScaleDto, value: Int, onComplete: () -> Unit = {}) {
        val newValue = task.currentValue + value
        val updated = task.copy(currentValue = newValue)
        launchWithLoading(
            block = {
                val result = repository.updateTask(task.id, updated)
                if (result.isFailure) {
                    throw result.exceptionOrNull() ?: Exception("Unknown error while creating task")
                }
                loadTasks()
                onComplete()
            },
            onError = { msg ->
                _uiState.update { it.copy(error = "Failed to create task: $msg") }
            }
        )
    }

    fun updateSubtaskCurrentValue(taskId: String, subtask: SubtaskScaleDto, value: Int, onComplete: () -> Unit = {}) {
        val newValue = subtask.currentValue + value
        val updated = subtask.copy(currentValue = newValue)
        launchWithLoading(
            block = {
                val result = repository.updateSubtask(taskId, updated.id, updated)
                if (result.isFailure) {
                    throw result.exceptionOrNull()
                        ?: Exception("Unknown error while creating task")
                }
                loadTasks()
                onComplete()
            },
            onError = { msg ->
                _uiState.update { it.copy(error = "Failed to create task: $msg") }
            }
        )
    }

    fun checkInTask(taskId: String, date:String, onComplete: () -> Unit = {} ) {
        launchWithLoading(
            block = {
                val result = repository.checkInTask(taskId, date)
                if (result.isFailure) {
                    throw result.exceptionOrNull()
                        ?: Exception("Помилка відмітки задачі!")
                }
                //loadTasks()
                val updatedTask = result.getOrNull()
                if (updatedTask != null) {
                    updateTaskInUiState(updatedTask)
                }
                onComplete()
            },
            onError = { msg ->
                _uiState.update { it.copy(error = "Помилка відмітки задачі!: $msg") }
            }
        )
    }

    private fun updateTaskInUiState(updatedTask: TaskDto) {
        _uiState.update { currentState ->
            val updatedFoldersWithTasks = currentState.foldersWithTasks.map { folder ->
                val updatedTasks = folder.tasks.map { task ->
                    if (task.id == updatedTask.id) {
                        updatedTask
                    } else {
                        task
                    }
                }
                folder.copy(tasks = updatedTasks)
            }
            currentState.copy(foldersWithTasks = updatedFoldersWithTasks)
        }
    }
}
