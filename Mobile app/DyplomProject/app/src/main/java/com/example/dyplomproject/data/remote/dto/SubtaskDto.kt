package com.example.dyplomproject.data.remote.dto

//Базовий інтерфейс / sealed class
sealed interface SubtaskDto {
    val id: String
    //val type: String
    val title: String
    val description: String?
}

//Standard
data class SubtaskStandardDto(
    override val id: String,
    //override val subtask_type: String = "standard",
    override val title: String,
    override val description: String?,
    val isCompleted: Boolean
) : SubtaskDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "standard"
//}


//Repeatable
data class SubtaskRepeatableDto(
    override val id: String,
    //override val subtask_type: String = "repeatable",
    override val title: String,
    override val description: String?,
    val repeatDays: List<String>,
    val checkedInDays: List<String>
) : SubtaskDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "repeatable"
//}

//Scale
data class SubtaskScaleDto(
    override val id: String,
    //override val subtask_type: String = "scale",
    override val title: String,
    override val description: String?,
    val unit: String,
    val currentValue: Double,//Float,
    val targetValue: Double//Float
) : SubtaskDto
//{
//    @Expose(serialize = false, deserialize = false)
//    @Transient
//    override val type: String = "scale"
//}
