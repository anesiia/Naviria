package com.example.dyplomproject.ui.components.task.update

import android.util.Log
import androidx.compose.runtime.Composable
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.dyplomproject.data.remote.Tag
import com.example.dyplomproject.data.remote.dto.TaskDto
import com.example.dyplomproject.data.remote.dto.TaskRepeatableDto
import com.example.dyplomproject.data.remote.dto.TaskScaleDto
import com.example.dyplomproject.data.remote.dto.TaskStandardDto
import com.example.dyplomproject.data.remote.dto.TaskWithSubtasksDto
import com.example.dyplomproject.data.remote.repository.TaskRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.ButtonStyle
import com.example.dyplomproject.ui.components.DateField
import com.example.dyplomproject.ui.components.DateTimeState
import com.example.dyplomproject.ui.components.SecondaryButton
import com.example.dyplomproject.ui.components.SwitchButton
import com.example.dyplomproject.ui.components.TimeField
import com.example.dyplomproject.ui.components.task.WeekdayEditSelectorFromNames
import com.example.dyplomproject.ui.components.task.create.TaskCreationTextFieldColors
import com.example.dyplomproject.ui.components.toIsoUtcString
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.viewmodel.TasksViewModel

@Composable
fun TaskModificationForm(
    task: TaskDto,
    onSave: (TaskDto) -> Unit,
    onCancel: () -> Unit,
    viewModel: TasksViewModel
) {
    Log.d("MODIFICATION FORM", task.toString())
    var editState by remember { mutableStateOf(taskToModificationState(task)) }
    val deadlineDateState = remember { mutableStateOf(DateTimeState(null, null)) }
    val notificationDateTimeState = remember { mutableStateOf(DateTimeState(null, null)) }

    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp)
            .border(1.dp, Color(0xFF023047), RoundedCornerShape(12.dp))
            .background(Color.Transparent/*(0xFFF7F7F7)*/, RoundedCornerShape(12.dp))
            .padding(horizontal = 16.dp)
    ) {
        TextField(
            value = editState.title,
            onValueChange = { editState = editState.copy(title = it) },
            placeholder = { Text("Введіть назву задачі", style = MaterialTheme.typography.labelLarge) },
            modifier = Modifier.fillMaxWidth(),
            colors = TaskCreationTextFieldColors()
        )
//        if (taskCreationState.titleError != null) {
//            Spacer(Modifier.height(8.dp))
//            Text(
//                text = taskCreationState.titleError,
//                color = MaterialTheme.colorScheme.error,
//                style = additionalTypography.mediumText
//            )
//        }
        Spacer(Modifier.height(8.dp))
        TextField(
            value = editState.description!!,
            onValueChange = { editState = editState.copy(description = it) },
            placeholder = { Text("Опис вашої задачі", style = MaterialTheme.typography.labelMedium) },
            modifier = Modifier.fillMaxWidth(),
            colors = TaskCreationTextFieldColors()
        )

//        Spacer(Modifier.height(8.dp))
//        Text("Teги",color = AppColors.DarkBlue)
//        Spacer(Modifier.height(8.dp))
//        TagInputComponent(
//            tags = editState.tags,//tags,
//            onTagsChange = { updatedTags ->
//                tags.clear()
//                tags.addAll(updatedTags)
//                viewModel.updateTaskCreation { copy(tags = updatedTags) }
//            }
//        )

        Spacer(Modifier.height(8.dp))
        Row (verticalAlignment = Alignment.CenterVertically){
            SwitchButton(
                isOn = editState.isDeadlineOn,//isDeadlineOn,
                onToggle = { editState = editState.copy(isDeadlineOn = it)}
            )
            Spacer(Modifier.width(16.dp))
            Text("Дедлайн",color = AppColors.DarkBlue)
        }
        if(editState.isDeadlineOn) {
            Spacer(Modifier.height(8.dp))
            DateField(
                selectedDate = deadlineDateState.value.date,
                onDateSelected = {
                    val updated = deadlineDateState.value.copy(date = it)
                    deadlineDateState.value = updated
                    editState = editState.copy(deadline = updated.toIsoUtcString() ?: "")
                }
            )
        }

        Spacer(Modifier.height(8.dp))
        Row (verticalAlignment = Alignment.CenterVertically){
            SwitchButton(
                isOn = editState.isNotificationsOn,
                onToggle = { editState = editState.copy(isNotificationsOn = it) }
            )
            Spacer(Modifier.width(16.dp))
            Text("Нагадування",color = AppColors.DarkBlue)
        }

        if(editState.isNotificationsOn) {
            Spacer(Modifier.height(16.dp))
            DateField(
                selectedDate = notificationDateTimeState.value.date,
                onDateSelected = {
                    val updated = notificationDateTimeState.value.copy(date = it)
                    notificationDateTimeState.value = updated
                    editState = editState.copy(notificationDate = updated.toIsoUtcString() ?: "")
                }
            )
            Spacer(Modifier.height(8.dp))
            TimeField(
                selectedTime = notificationDateTimeState.value.time,
                onTimeSelected = {
                    //dateTimeState.value = dateTimeState.value.copy(time = it)
                    val updated = notificationDateTimeState.value.copy(time = it)
                    notificationDateTimeState.value = updated
                    editState = editState.copy(notificationDate = updated.toIsoUtcString() ?: "")
                }
            )
        }

//        Spacer(Modifier.height(8.dp))
//        Row(verticalAlignment = Alignment.CenterVertically) {
//            Text("Приорітет:",color = AppColors.DarkBlue)
//            Spacer(Modifier.width(16.dp))
//            DropdownMenuPriority (
//                options = listOf(Priority("Високий", 1), Priority("Середній", 2), Priority("Низький", 1)),
//                selected = editState.priority,
//                onSelected = {viewModel.updateTaskCreation{ copy(priority = it) }}
//            )
//        }

        Spacer(Modifier.height(8.dp))
        if (task is TaskWithSubtasksDto) {
            Text("Тип задачі: cписок", style = MaterialTheme.typography.bodyLarge,color = AppColors.DarkBlue)
        } else if (task is TaskScaleDto) {
            Text("Тип задачі: шкала", style = MaterialTheme.typography.bodyLarge,color = AppColors.DarkBlue)
        } else if (task is TaskRepeatableDto) {
            Text("Тип задачі: повторювальна", style = MaterialTheme.typography.bodyLarge,color = AppColors.DarkBlue)
        } else {
            Text("Тип задачі: звичайна", style = MaterialTheme.typography.bodyLarge,color = AppColors.DarkBlue)
        }

        if (task is TaskRepeatableDto) {
            HorizontalDivider(modifier = Modifier.fillMaxWidth(), 1.dp, AppColors.Orange)
            Spacer(Modifier.height(8.dp))
            Box () {
                WeekdayEditSelectorFromNames(
                    modifier = Modifier,
                    markedDayNames = editState.repeatDays,
                    onMarkedDayNamesChange = { editState = editState.copy(repeatDays = it)}
                )
            }
            Spacer(Modifier.height(16.dp))
        }
//

//        if (task.type == "scale") {
//            HorizontalDivider(modifier = Modifier.fillMaxWidth(), 1.dp, AppColors.Orange)
//            Spacer(Modifier.height(16.dp))
//            Text("Одиниця вимірювання", style = MaterialTheme.typography.bodyLarge, color = AppColors.DarkBlue)
//            TextField(
//                value = taskCreationState.unit,
//                onValueChange = { viewModel.updateTaskCreation { copy(unit = it) }},
//                placeholder = { Text("Введіть символ або назву", style = MaterialTheme.typography.labelMedium) },
//                modifier = Modifier.fillMaxWidth(),
//                colors = TaskCreationTextFieldColors()
//            )
//            Spacer(Modifier.height(8.dp))
//            Text("Ціль", style = MaterialTheme.typography.bodyLarge, color = AppColors.DarkBlue)
//            TextField(
//                value = taskCreationState.targetValue.toString(),
//                onValueChange = { input ->
//                    val intValue = input.toIntOrNull()
//                    if (intValue != null) {
//                        viewModel.updateTaskCreation{ copy(targetValue = intValue) }
//                    }
//                },
//                placeholder = { Text("Введіть число", style = MaterialTheme.typography.labelMedium) },
//                modifier = Modifier.fillMaxWidth(),
//                keyboardOptions = KeyboardOptions.Default.copy(keyboardType = KeyboardType.Number),
//                colors = TaskCreationTextFieldColors()
//            )
//            Spacer(Modifier.height(8.dp))
//        }
        Spacer(Modifier.height(16.dp))
        Row(horizontalArrangement = Arrangement.Center, modifier = Modifier.fillMaxWidth()) {
            SecondaryButton(
                text = "Скасувати зміни",
                onClick = onCancel,
                modifier = Modifier.weight(1f),
                style = ButtonStyle.Outline(Color(0xFF023047))
            )
            Spacer(Modifier.width(8.dp))
            SecondaryButton(
                text = "Зберегти зміни",
                onClick = {
                    val updatedTask = updateTaskFromModificationState(task, editState)
                    //viewModel.updateTask(updatedTask)
                    onSave(updatedTask)
                },
                modifier = Modifier.weight(1f)
            )
        }
        Spacer(Modifier.height(16.dp))

//        Row(horizontalArrangement = Arrangement.End, modifier = Modifier.fillMaxWidth()) {
////            Button(onClick = onCancel) {
////                Text("Cancel")
////            }
////            Spacer(modifier = Modifier.width(8.dp))
//            Button(onClick = {
////                val updatedTask = task.copy(
////                        title = editState.value.title,
////                description = editState.value.description,
//                // Add more fields based on task type
//                //)
//                val updatedTask = updateTaskFromModificationState(task, editState)
//                viewModel.updateTask(updatedTask)
//                onSave(updatedTask)
//            }) {
//                Text("Save")
//            }
//        }
    }
}

@Preview(showBackground = true)
@Composable
fun TaskModificationFormPreview() {
    val repository = remember { TaskRepository(RetrofitInstance.api) }
    val viewModel: TasksViewModel = viewModel(factory = object: ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T: ViewModel> create(modelClass: Class<T>) : T {
            if (modelClass.isAssignableFrom(TasksViewModel::class.java)) {
                return TasksViewModel(repository, "") as T
            }
            throw IllegalArgumentException("Unknown ViewModel class")
        }
    })

    val task = TaskStandardDto(
        id = "task-cosmos-001",
        userId = "astro-user",
        folderId = "folder-space",
        title = "Запустити ракету до Марса",
        description = null,//"Перевірити всі системи, запустити двигуни і зібрати екіпаж для місії.",
        categoryId = "space-mission",
        tags = listOf(Tag("Космос"), Tag("Надважливо")),
        isDeadlineOn = true,
        deadline = "2030-09-15T14:00:00Z",
        isShownProgressOnPage = true,
        isNotificationsOn = true,
        notificationDate = "2030-09-10T09:00:00Z",
        notificationSent = false,
        priority = 10,
        subtasks = emptyList(),
        createdAt = "2029-01-01T00:00:00Z",
        completedAt = null,
        status = "планується",
        //type = "standard"
    )

    Column(){
        TaskModificationForm(
            task,
            {},
            {},
            viewModel = viewModel
        )
    }
}