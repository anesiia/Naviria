package com.example.dyplomproject.data.remote.dto

import com.example.dyplomproject.data.remote.Tag

interface TaskCreateDto {
    //val type: String
    val userId: String
    val folderId: String
    val title: String
    val description: String?
    val categoryId: String?
    val tags: List<Tag>
    val isDeadlineOn: Boolean
    val deadline: String? // ISO формат
    val isShownProgressOnPage: Boolean
    val isNotificationsOn: Boolean
    val notificationDate: String?
    val priority: Int
    //val subtasks: List<SubtaskCreateDto>
}
//Клас для задачі з підзадачами
data class TaskWithSubtasksCreateDto(
    //override val type: String = "with_subtasks",
    override val userId: String,
    override val folderId: String,
    override val title: String,
    override val description: String?,
    override val categoryId: String?,
    override val tags: List<Tag>,
    override val isDeadlineOn: Boolean,
    override val deadline: String?,
    override val isShownProgressOnPage: Boolean,
    override val isNotificationsOn: Boolean,
    override val notificationDate: String?,
    override val priority: Int,
    val subtasks: List<SubtaskCreateDto>
) : TaskCreateDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "with_subtasks"
//}
//Повторювана задача

data class TaskRepeatableCreateDto(
    //override val type: String = "repeatable",
    override val userId: String,
    override val folderId: String,
    override val title: String,
    override val description: String?,
    override val categoryId: String?,
    override val tags: List<Tag>,
    override val isDeadlineOn: Boolean,
    override val deadline: String?,
    override val isShownProgressOnPage: Boolean,
    override val isNotificationsOn: Boolean,
    override val notificationDate: String?,
    override val priority: Int,
    //override val subtasks: List<SubtaskCreateDto>,
    val repeatDays: List<Int>
) : TaskCreateDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "repeatable"
//}

//Scale задача
data class TaskScaleCreateDto(
    //override val type: String = "scale",
    override val userId: String,
    override val folderId: String,
    override val title: String,
    override val description: String?,
    override val categoryId: String?,
    override val tags: List<Tag>,
    override val isDeadlineOn: Boolean,
    override val deadline: String?,
    override val isShownProgressOnPage: Boolean,
    override val isNotificationsOn: Boolean,
    override val notificationDate: String?,
    override val priority: Int,
    //override val subtasks: List<SubtaskCreateDto>,
    val unit: String,
    val currentValue: Double,//Int,
    val targetValue: Double//Int
) : TaskCreateDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "scale"
//}

//Стандартна задача
data class TaskStandardCreateDto(
    //override val type: String = "standard",
    override val userId: String,
    override val folderId: String,
    override val title: String,
    override val description: String?,
    override val categoryId: String?,
    override val tags: List<Tag>,
    override val isDeadlineOn: Boolean,
    override val deadline: String?,
    override val isShownProgressOnPage: Boolean,
    override val isNotificationsOn: Boolean,
    override val notificationDate: String?,
    override val priority: Int,
    //override val subtasks: List<SubtaskCreateDto>
) : TaskCreateDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "standard"
//}

