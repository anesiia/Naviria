package com.example.dyplomproject.data.remote.dto

//Базовий інтерфейс / sealed class
sealed interface SubtaskCreateDto {
    //val subtask_type: String
    val title: String
    val description: String?
}

//Standard
data class SubtaskStandardCreateDto(
    //override val subtask_type: String = "standard",
    override val title: String,
    override val description: String?,
    val isCompleted: Boolean
) : SubtaskCreateDto

//Repeatable
data class SubtaskRepeatableCreateDto(
    //override val subtask_type: String = "repeatable",
    override val title: String,
    override val description: String?,
    val repeatDays: List<Int>
) : SubtaskCreateDto

//Scale
data class SubtaskScaleCreateDto(
    //override val subtask_type: String = "scale",
    override val title: String,
    override val description: String?,
    val unit: String,
    val currentValue: Double,//Float,
    val targetValue: Double//Float
) : SubtaskCreateDto
