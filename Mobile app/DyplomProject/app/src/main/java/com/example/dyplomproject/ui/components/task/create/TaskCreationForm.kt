package com.example.dyplomproject.ui.components.task.create

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.dyplomproject.data.remote.Category
import com.example.dyplomproject.data.remote.repository.TaskRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.ButtonStyle
import com.example.dyplomproject.ui.components.DateField
import com.example.dyplomproject.ui.components.DateTimeState
import com.example.dyplomproject.ui.components.DropdownMenuCategory
import com.example.dyplomproject.ui.components.DropdownMenuPriority
import com.example.dyplomproject.ui.components.RadioButton
import com.example.dyplomproject.ui.components.SecondaryButton
import com.example.dyplomproject.ui.components.SwitchButton
import com.example.dyplomproject.ui.components.TimeField
import com.example.dyplomproject.ui.components.task.TagInputComponent
import com.example.dyplomproject.ui.components.task.WeekdayEditSelector
import com.example.dyplomproject.ui.components.toIsoUtcString
import com.example.dyplomproject.ui.screen.AddTaskButton
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.TasksViewModel
import kotlinx.coroutines.delay

data class TaskCreationState(
    val title: String = "",
    val description: String = "",
    val selectedCategory: Category = Category("", "Категорія"),
    val deadline: String = "",
    val isDeadlineOn: Boolean = false,
    val isNotificationsOn: Boolean = false,
    val notificationDate: String = "",
    val type: String = "repeatable",
    val subtaskCreationForms: List<SubtaskFormState> = emptyList(),
    val tags: List<String> = emptyList(),//Map<String, String> = emptyMap(),
    val priority: Priority = Priority("Низький", 1),
    val isShownProgressOnPage: Boolean = false,
    //repeatable
    val repeatDays: List<Int> = emptyList(),
    //scale
    val unit: String = "",
    val currentValue: Double = 0.0,
    val targetValue: Double = 0.0,
    //validation
    val titleError: String? = null,
    //val categoryError: String? = null,
    val targetValueError: String? = null,
    val repeatDaysError: String? = null,
    val subtasksError: String? = null,
    val unitValueError: String? = null,
    val deadlineDateError: String? = null,
    val notificationDateError: String? = null
)

data class Priority(
    val name: String,
    val level: Int
)

@Composable
fun TaskCreationForm(
    onSave: () -> Unit,
    onCancel: () -> Unit,
    categories: List<Category>,
    viewModel: TasksViewModel
) {
    val taskCreationState = viewModel.uiState.collectAsState().value.taskCreation
    val tags = remember { mutableStateListOf<String>() }
    val deadlineDateState = remember { mutableStateOf(DateTimeState(null, null)) }
    val notificationDateTimeState = remember { mutableStateOf(DateTimeState(null, null)) }
    val showErrorTime = 5000L
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp)
            .border(1.dp, Color(0xFF023047), RoundedCornerShape(12.dp))
            .background(Color.Transparent, RoundedCornerShape(12.dp))
            .padding(horizontal = 16.dp)
    ) {
        TextField(
            value = taskCreationState.title,
            onValueChange = {viewModel.updateTaskCreation { copy(title = it) }},//{ title = it },
            //label = { Text("Назва") },
            placeholder = { Text("Введіть назву задачі", style = MaterialTheme.typography.labelLarge) },
            modifier = Modifier.fillMaxWidth(),
            colors = TaskCreationTextFieldColors()
        )
        if (taskCreationState.titleError != null) {
            LaunchedEffect(taskCreationState.titleError) {
                delay(showErrorTime)
                viewModel.updateTaskCreation { copy(titleError = null) }
            }
            Spacer(Modifier.height(8.dp))
            Text(
                text = taskCreationState.titleError,
                color = MaterialTheme.colorScheme.error,
                style = additionalTypography.mediumText
            )
        }
        Spacer(Modifier.height(8.dp))
        TextField(
            value = taskCreationState.description, //description,
            onValueChange = {viewModel.updateTaskCreation { copy(description = it) }}, //{ description = it },
            placeholder = { Text("Опис вашої задачі", style = MaterialTheme.typography.labelMedium) },
            modifier = Modifier.fillMaxWidth(),
            colors = TaskCreationTextFieldColors()
        )

        Spacer(Modifier.height(8.dp))
        if(taskCreationState.selectedCategory.id == ""){
            viewModel.updateTaskCreation{copy(selectedCategory = categories[0])}
        }
        Row(verticalAlignment = Alignment.CenterVertically) {
            Text("Категорія:",color = AppColors.DarkBlue)
            Spacer(Modifier.width(8.dp))
            DropdownMenuCategory(
                options = categories,
                selected = taskCreationState.selectedCategory,//category,
                onSelected = { viewModel.updateTaskCreation{ copy(selectedCategory = it)} }//{ category = it }
            )
        }

        Spacer(Modifier.height(8.dp))
        Text("Teги",color = AppColors.DarkBlue)
        Spacer(Modifier.height(8.dp))
        TagInputComponent(
            tags = taskCreationState.tags,//tags,
            onTagsChange = { updatedTags ->
                tags.clear()
                tags.addAll(updatedTags)
                viewModel.updateTaskCreation { copy(tags = updatedTags) }
            }
        )

        Spacer(Modifier.height(8.dp))
        Row (verticalAlignment = Alignment.CenterVertically){
            SwitchButton(
                isOn = taskCreationState.isDeadlineOn,//isDeadlineOn,
                onToggle = { viewModel.updateTaskCreation { copy(isDeadlineOn = it) }} //{ isDeadlineOn = it }
            )
            Spacer(Modifier.width(16.dp))
            Text("Дедлайн",color = AppColors.DarkBlue)
        }
        if(taskCreationState.isDeadlineOn) {//(isDeadlineOn) {
            Spacer(Modifier.height(8.dp))
            DateField(
                selectedDate = deadlineDateState.value.date,
                onDateSelected = {
                    val updated = deadlineDateState.value.copy(date = it)
                    deadlineDateState.value = updated
                    viewModel.updateTaskCreation {
                        copy(
                            deadline = updated.toIsoUtcString() ?: ""
                        )
                    }
                }
            )
            if (taskCreationState.deadlineDateError != null) {
                LaunchedEffect(taskCreationState.deadlineDateError) {
                    delay(showErrorTime)
                    viewModel.updateTaskCreation { copy(deadlineDateError = null) }
                }
                Spacer(Modifier.height(8.dp))
                Text(
                    text = taskCreationState.deadlineDateError,
                    color = MaterialTheme.colorScheme.error,
                    style = additionalTypography.mediumText
                )
            }
        }

        Spacer(Modifier.height(8.dp))
        Row (verticalAlignment = Alignment.CenterVertically){
            SwitchButton(
                isOn = taskCreationState.isShownProgressOnPage, //toShowProgressOnPage,
                onToggle = { viewModel.updateTaskCreation { copy(isShownProgressOnPage = it) }} //{ toShowProgressOnPage = it }
            )
            Spacer(Modifier.width(16.dp))
            Text("Відображати прогрес на сторінці",color = AppColors.DarkBlue)
        }

        Spacer(Modifier.height(8.dp))
        Row (verticalAlignment = Alignment.CenterVertically){
            SwitchButton(
                isOn = taskCreationState.isNotificationsOn,
                onToggle = { viewModel.updateTaskCreation { copy(isNotificationsOn = it) }}
            )
            Spacer(Modifier.width(16.dp))
            Text("Нагадування",color = AppColors.DarkBlue)
        }

        if(taskCreationState.isNotificationsOn) {
            Spacer(Modifier.height(16.dp))
            DateField(
                selectedDate = notificationDateTimeState.value.date,
                onDateSelected = {
                    val updated = notificationDateTimeState.value.copy(date = it)
                    notificationDateTimeState.value = updated
                    viewModel.updateTaskCreation {
                        copy(
                            notificationDate = updated.toIsoUtcString() ?: ""
                        )
                    }
                }
            )
            Spacer(Modifier.height(8.dp))
            TimeField(
                selectedTime = notificationDateTimeState.value.time,
                onTimeSelected = {
                    val updated = notificationDateTimeState.value.copy(time = it)
                    notificationDateTimeState.value = updated
                    viewModel.updateTaskCreation {
                        copy(
                            notificationDate = updated.toIsoUtcString() ?: ""
                        )
                    }
                }
            )
            if (taskCreationState.notificationDateError != null) {
                LaunchedEffect(taskCreationState.notificationDateError) {
                    delay(showErrorTime)
                    viewModel.updateTaskCreation { copy(notificationDateError = null) }
                }
                Spacer(Modifier.height(8.dp))
                Text(
                    text = taskCreationState.notificationDateError,
                    color = MaterialTheme.colorScheme.error,
                    style = additionalTypography.mediumText
                )
            }
        }

        Spacer(Modifier.height(8.dp))
        Row(verticalAlignment = Alignment.CenterVertically) {
            Text("Приорітет:",color = AppColors.DarkBlue)
            Spacer(Modifier.width(16.dp))
            DropdownMenuPriority (
                options = listOf(Priority("Високий", 1), Priority("Середній", 2), Priority("Низький", 1)),
                selected = taskCreationState.priority,
                onSelected = {viewModel.updateTaskCreation{ copy(priority = it) }}
            )
        }

        Spacer(Modifier.height(8.dp))

        Text("Тип задачі", style = MaterialTheme.typography.bodyLarge,color = AppColors.DarkBlue)
        Spacer(modifier = Modifier.height(4.dp))
        Column {
            RadioButton(selected = taskCreationState.type == "standard",//typeTask == "standard",
                text = "Звичайна",
                onClick = { viewModel.updateTaskCreation { copy(type = "standard") }})//{ typeTask = "standart" })//{ registrationViewModel.updateField("gender", "f") })

            Spacer(modifier = Modifier.height(8.dp))
            RadioButton(selected = taskCreationState.type == "with_subtasks",
                text = "Список",
                onClick = { viewModel.updateTaskCreation { copy(type = "with_subtasks") }})

            Spacer(modifier = Modifier.height(8.dp))
            RadioButton(selected = taskCreationState.type == "scale",
                text = "Шкала",
                onClick = { viewModel.updateTaskCreation { copy(type = "scale") }})

            Spacer(modifier = Modifier.height(8.dp))
            RadioButton(selected = taskCreationState.type == "repeatable",
                text = "Повторювальна",
                onClick = { viewModel.updateTaskCreation { copy(type = "repeatable") }})
            Spacer(modifier = Modifier.height(8.dp))
        }

        if (taskCreationState.type == "repeatable") {
            HorizontalDivider(modifier = Modifier.fillMaxWidth(), 1.dp, AppColors.Orange)
            Spacer(Modifier.height(8.dp))
            WeekdayEditSelector(
                markedDays = taskCreationState.repeatDays,
                onMarkedDaysChange = { viewModel.updateTaskCreation { copy(repeatDays = it) }}
            )
            if (taskCreationState.repeatDaysError != null) {
                LaunchedEffect(taskCreationState.repeatDaysError) {
                    delay(showErrorTime)
                    viewModel.updateTaskCreation { copy(repeatDaysError = null) }
                }
                Spacer(Modifier.height(8.dp))
                Text(
                    text = taskCreationState.repeatDaysError,
                    color = MaterialTheme.colorScheme.error,
                    style = additionalTypography.mediumText
                )
            }
            Spacer(Modifier.height(16.dp))
        }

        if (taskCreationState.type == "with_subtasks") {
            HorizontalDivider(modifier = Modifier.fillMaxWidth(), 1.dp, AppColors.Orange)
            Spacer(Modifier.height(8.dp))
            Column {
                taskCreationState.subtaskCreationForms.forEach { form ->
                    SubtaskCreationForm(
                        subtaskId = form.id,
                        viewModel = viewModel,
                        onCancel = {
                            viewModel.removeSubtaskForm(form.id)
                        }
                    )
                }
                if (taskCreationState.subtasksError != null) {
                    LaunchedEffect(taskCreationState.subtasksError) {
                        delay(showErrorTime)
                        viewModel.updateTaskCreation { copy(subtasksError = null) }
                    }
                    Spacer(Modifier.height(8.dp))
                    Text(
                        text = taskCreationState.subtasksError,
                        color = MaterialTheme.colorScheme.error,
                        style = additionalTypography.mediumText
                    )
                    //viewModel.updateTaskCreation { copy(subtasksError = null) }
                }
                AddTaskButton(
                    onClick = {
                        viewModel.addSubtaskForm()
                    }
                )
            }
        }

        if (taskCreationState.type == "scale") {
            HorizontalDivider(modifier = Modifier.fillMaxWidth(), 1.dp, AppColors.Orange)
            Spacer(Modifier.height(16.dp))
            Text("Одиниця вимірювання", style = MaterialTheme.typography.bodyLarge, color = AppColors.DarkBlue)
            TextField(
                value = taskCreationState.unit,
                onValueChange = { viewModel.updateTaskCreation { copy(unit = it) }},
                placeholder = { Text("Введіть символ або назву", style = MaterialTheme.typography.labelMedium) },
                modifier = Modifier.fillMaxWidth(),
                colors = TaskCreationTextFieldColors()
            )
            if (taskCreationState.unitValueError != null) {
                Spacer(Modifier.height(8.dp))
                LaunchedEffect(taskCreationState.unitValueError) {
                    delay(showErrorTime) // Show error for 3 seconds
                    viewModel.updateTaskCreation { copy(unitValueError = null) }
                }
                Text(
                    text = taskCreationState.unitValueError,
                    color = MaterialTheme.colorScheme.error,
                    style = additionalTypography.mediumText
                )
            }
            Spacer(Modifier.height(8.dp))

            var targetValueInput by remember { mutableStateOf("") }
            Text("Ціль", style = MaterialTheme.typography.bodyLarge, color = AppColors.DarkBlue)
            TextField(
                value = targetValueInput,
                onValueChange = { input ->
                    //in case the input value has to be integer value
                    targetValueInput = input
                    val doubleValue = input.toDoubleOrNull()
                    if (doubleValue != null) {
                        viewModel.updateTaskCreation { copy(targetValue = doubleValue) }
                    }
                    //in case the input value has to be float value
//                    targetValueInput = input
//                    val floatValue = input.toFloatOrNull()
//                    if (floatValue != null) {
//                        viewModel.updateTaskCreation { copy(targetValue = floatValue) }
//                    }
                },
                placeholder = { Text("Введіть число", style = MaterialTheme.typography.labelMedium) },
                modifier = Modifier.fillMaxWidth(),
                keyboardOptions = KeyboardOptions.Default.copy(keyboardType = KeyboardType.Number),
                colors = TaskCreationTextFieldColors()
            )
            if (taskCreationState.targetValueError != null) {
                LaunchedEffect(taskCreationState.targetValueError) {
                    delay(showErrorTime) // Show error for 3 seconds
                    viewModel.updateTaskCreation { copy(targetValueError = null) }
                }
                Spacer(Modifier.height(8.dp))
                Text(
                    text = taskCreationState.targetValueError,
                    color = MaterialTheme.colorScheme.error,
                    style = additionalTypography.mediumText
                )
            }
            Spacer(Modifier.height(8.dp))
        }
        Row(horizontalArrangement = Arrangement.Center, modifier = Modifier.fillMaxWidth()) {
//            TextButton(onClick = onCancel) {
//                Text("Скасувати")
//            }
//            Spacer(Modifier.width(8.dp))
//            Button(onClick = {
//                val newTask = Task(
//                    id = UUID.randomUUID().toString(),
//                    title = title,
//                    status = status,
//                    subtasks = listOf(), // Можна додати логіку для підзадач
//                    // Інші поля
//                )
//                onSave(newTask)
//            }) {
//                Text("Зберегти")
//            }
            SecondaryButton(
                text = "Скасувати",
                onClick = onCancel,
                modifier = Modifier.weight(1f),
                style = ButtonStyle.Outline(Color(0xFF023047))
            )
            Spacer(Modifier.width(8.dp))
            SecondaryButton(
                text = "Зберегти",
                onClick = { viewModel.createTask{ onSave() } },
                modifier = Modifier.weight(1f)
            )
        }
        Spacer(Modifier.height(16.dp))
    }
}

@Preview(showBackground = true)
@Composable
fun TaskCreationPreview() {
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
    TaskCreationForm(
        {},
        {},
        listOf(Category("", "sport")),
        viewModel
    )
}