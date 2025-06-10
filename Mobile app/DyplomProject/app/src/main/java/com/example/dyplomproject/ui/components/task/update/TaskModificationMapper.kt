package com.example.dyplomproject.ui.components.task.update

import com.example.dyplomproject.data.remote.Tag
import com.example.dyplomproject.data.remote.dto.TaskDto
import com.example.dyplomproject.data.remote.dto.TaskRepeatableDto
import com.example.dyplomproject.data.remote.dto.TaskScaleDto
import com.example.dyplomproject.data.remote.dto.TaskStandardDto
import com.example.dyplomproject.data.remote.dto.TaskWithSubtasksDto

data class TaskModificationState(
    val title: String = "",
    val description: String? = null,
    val deadline: String? = null,
    val isDeadlineOn: Boolean = false,
    val isNotificationsOn: Boolean = false,
    val notificationDate: String? = null,
    val priority: Int = 0,
    val categoryId: String? = null,
    val folderId: String? = null,
    val tags: List<Tag> = emptyList(),
    val repeatDays: List<String> = emptyList(),
    val unit: String = "",
    val targetValue: Double = 0.0,
    val currentValue: Double = 0.0
)

fun taskToModificationState(task: TaskDto): TaskModificationState {
    return when (task) {
        is TaskStandardDto -> TaskModificationState(
            title = task.title,
            description = task.description,
            deadline = task.deadline,
            isDeadlineOn = task.isDeadlineOn,
            isNotificationsOn = task.isNotificationsOn,
            notificationDate = task.notificationDate,
            priority = task.priority,
            categoryId = task.categoryId,
            folderId = task.folderId,
            tags = task.tags,
        )

        is TaskWithSubtasksDto -> TaskModificationState(
            title = task.title,
            description = task.description,
            deadline = task.deadline,
            isDeadlineOn = task.isDeadlineOn,
            isNotificationsOn = task.isNotificationsOn,
            notificationDate = task.notificationDate,
            priority = task.priority,
            categoryId = task.categoryId,
            folderId = task.folderId,
            tags = task.tags
        )

        is TaskRepeatableDto -> TaskModificationState(
            title = task.title,
            description = task.description,
            deadline = task.deadline,
            isDeadlineOn = task.isDeadlineOn,
            isNotificationsOn = task.isNotificationsOn,
            notificationDate = task.notificationDate,
            priority = task.priority,
            categoryId = task.categoryId,
            folderId = task.folderId,
            tags = task.tags,
            repeatDays = task.repeatDays
        )

        is TaskScaleDto -> TaskModificationState(
            title = task.title,
            description = task.description,
            deadline = task.deadline,
            isDeadlineOn = task.isDeadlineOn,
            isNotificationsOn = task.isNotificationsOn,
            notificationDate = task.notificationDate,
            priority = task.priority,
            categoryId = task.categoryId,
            folderId = task.folderId,
            tags = task.tags,
            unit = task.unit,
            targetValue = task.targetValue,
            currentValue = task.currentValue
        )
        else -> throw IllegalArgumentException("Unsupported TaskDto type: ${task::class.simpleName}")
    }
}

fun updateTaskFromModificationState(task: TaskDto, state: TaskModificationState): TaskDto {
    return when (task) {
        is TaskStandardDto -> task.copy(
            title = state.title,
            description = state.description,
            deadline = state.deadline,
            isDeadlineOn = state.isDeadlineOn,
            isNotificationsOn = state.isNotificationsOn,
            notificationDate = state.notificationDate,
            priority = state.priority,
            categoryId = state.categoryId,
            folderId = state.folderId,
            tags = state.tags,
            //type = "standard"
        )

        is TaskWithSubtasksDto -> task.copy(
            title = state.title,
            description = state.description,
            deadline = state.deadline,
            isDeadlineOn = state.isDeadlineOn,
            isNotificationsOn = state.isNotificationsOn,
            notificationDate = state.notificationDate,
            priority = state.priority,
            categoryId = state.categoryId,
            folderId = state.folderId,
            tags = state.tags,
            //type = "with_subtasks"
        )

        is TaskRepeatableDto -> task.copy(
            title = state.title,
            description = state.description,
            deadline = state.deadline,
            isDeadlineOn = state.isDeadlineOn,
            isNotificationsOn = state.isNotificationsOn,
            notificationDate = state.notificationDate,
            priority = state.priority,
            categoryId = state.categoryId,
            folderId = state.folderId,
            tags = state.tags,
            repeatDays = state.repeatDays,
            //type = "repeatable"
        )

        is TaskScaleDto -> task.copy(
            title = state.title,
            description = state.description,
            deadline = state.deadline,
            isDeadlineOn = state.isDeadlineOn,
            isNotificationsOn = state.isNotificationsOn,
            notificationDate = state.notificationDate,
            priority = state.priority,
            categoryId = state.categoryId,
            folderId = state.folderId,
            tags = state.tags,
            unit = state.unit,
            targetValue = state.targetValue,
            currentValue = state.currentValue,
            //type = "scale"
        )
        else -> throw IllegalArgumentException("Unsupported TaskDto type: ${task::class.simpleName}")
    }
}