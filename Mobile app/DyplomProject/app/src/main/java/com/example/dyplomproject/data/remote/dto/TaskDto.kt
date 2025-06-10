package com.example.dyplomproject.data.remote.dto

import com.example.dyplomproject.data.remote.Tag

//Sealed Interface
interface TaskDto {
    val id: String
    val userId: String
    val folderId: String?
    val title: String
    val description: String?
    val categoryId: String?
    val tags: List<Tag>
    val isDeadlineOn: Boolean
    val deadline: String?
    val isShownProgressOnPage: Boolean
    val isNotificationsOn: Boolean
    val notificationDate: String?
    val notificationSent: Boolean
    val priority: Int
    val subtasks: List<SubtaskDto>///IT IS NOT CORRECT
    val createdAt: String
    val completedAt: String?
    val status: String
    //val type: String
}

//TaskStandardDto.kt
data class TaskStandardDto(
    override val id: String,
    override val userId: String,
    override val folderId: String?,
    override val title: String,
    override val description: String?,
    override val categoryId: String?,
    override val tags: List<Tag>,
    override val isDeadlineOn: Boolean,
    override val deadline: String?,
    override val isShownProgressOnPage: Boolean,
    override val isNotificationsOn: Boolean,
    override val notificationDate: String?,
    override val notificationSent: Boolean,
    override val priority: Int,
    override val subtasks: List<SubtaskDto>,
    override val createdAt: String,
    override val completedAt: String?,
    override val status: String,
    //override val type: String = "standard"
) : TaskDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "standard"
//}

//TaskRepeatableDto.kt
data class TaskRepeatableDto(
    override val id: String,
    override val userId: String,
    override val folderId: String?,
    override val title: String,
    override val description: String?,
    override val categoryId: String?,
    override val tags: List<Tag>,
    override val isDeadlineOn: Boolean,
    override val deadline: String?,
    override val isShownProgressOnPage: Boolean,
    override val isNotificationsOn: Boolean,
    override val notificationDate: String?,
    override val notificationSent: Boolean,
    override val priority: Int,
    override val subtasks: List<SubtaskDto>,
    override val createdAt: String,
    override val completedAt: String?,
    override val status: String,
    val repeatDays: List<String>,
    val checkedInDays: List<String>,
    //override val type: String = "repeatable"
) : TaskDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "repeatable"
//}

//TaskScaleDto.kt
data class TaskScaleDto(
    override val id: String,
    override val userId: String,
    override val folderId: String?,
    override val title: String,
    override val description: String?,
    override val categoryId: String?,
    override val tags: List<Tag>,
    override val isDeadlineOn: Boolean,
    override val deadline: String?,
    override val isShownProgressOnPage: Boolean,
    override val isNotificationsOn: Boolean,
    override val notificationDate: String?,
    override val notificationSent: Boolean,
    override val priority: Int,
    override val subtasks: List<SubtaskDto>,
    override val createdAt: String,
    override val completedAt: String?,
    override val status: String,
    val unit: String,
    val currentValue: Double,
    val targetValue: Double,
    //override val type: String = "scale"
) : TaskDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "scale"
//}

//TaskWithSubtasksDto.kt
data class TaskWithSubtasksDto(
    override val id: String,
    override val userId: String,
    override val folderId: String?,
    override val title: String,
    override val description: String?,
    override val categoryId: String?,
    override val tags: List<Tag>,
    override val isDeadlineOn: Boolean,
    override val deadline: String?,
    override val isShownProgressOnPage: Boolean,
    override val isNotificationsOn: Boolean,
    override val notificationDate: String?,
    override val notificationSent: Boolean,
    override val priority: Int,
    override val subtasks: List<SubtaskDto>,
    override val createdAt: String,
    override val completedAt: String?,
    override val status: String,
    //override val type: String = "with_subtasks"
) : TaskDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "with_subtasks"
//}