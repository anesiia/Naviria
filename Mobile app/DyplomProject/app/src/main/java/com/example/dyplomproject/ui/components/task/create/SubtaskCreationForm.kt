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
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.dyplomproject.data.remote.repository.TaskRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.ButtonStyle
import com.example.dyplomproject.ui.components.RadioButton
import com.example.dyplomproject.ui.components.SecondaryButton
import com.example.dyplomproject.ui.components.task.WeekdayEditSelector
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.TasksViewModel
import kotlinx.coroutines.delay
import java.util.UUID

data class SubtaskFormState(
    val id: String = UUID.randomUUID().toString(),
    val type: String = "standard", // "repeatable" or "scale"
    val title: String = "",
    val description: String = "",
    // Standard
    //val isCompleted: Boolean = false,
    // Repeatable
    val repeatDays: List<Int> = emptyList(),
    // Scale
    val unit: String = "",
    val currentValue: Double = 0.0,
    val targetValue: Double = 0.0,
    // Validation
    val titleError: String? = null,
    val targetValueError: String? = null,
    val repeatDaysError: String? = null,
    val unitValueError: String? = null,
    val error: String? = null
)

@Composable
fun SubtaskCreationForm(
    //onSave: () -> Unit,
    subtaskId: String,//remove for preview method
    onCancel: () -> Unit,
    viewModel: TasksViewModel
) {
    val creationState = viewModel.uiState.collectAsState().value.taskCreation
    //val subtaskForm = SubtaskFormState(type = "scale")// for preview method
    val subtaskFormState = creationState.subtaskCreationForms.find { it.id == subtaskId } ?: return
    val showErrorTime = 5000L

    //val newForm = creationState.subtaskForms.find { it.id == newSubtaskId }
    //viewModel.updateTaskCreation { copy(subtasks = subtasks+)}

    //if (subtaskForm == null) return
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp)
            .border(1.dp, Color(0xFF023047), RoundedCornerShape(12.dp))
            .background(AppColors.MilkBlue/*(0xFFF7F7F7)*/, RoundedCornerShape(12.dp))
            .padding(horizontal = 16.dp)

    ) {
        TextField(
            value = subtaskFormState.title,//title,
            onValueChange = {  viewModel.updateSubtaskForm(subtaskId) { copy(title = it) }},//{ title = it },
            //label = { Text("Назва") },
            placeholder = { Text("Введіть назву задачі", style = MaterialTheme.typography.labelLarge) },
            modifier = Modifier.fillMaxWidth(),
            colors = TaskCreationTextFieldColors()
        )
        if (subtaskFormState.titleError != null) {
            LaunchedEffect(subtaskFormState.titleError) {
                delay(showErrorTime)
                viewModel.updateSubtaskForm(subtaskId) { copy(titleError = null) }
            }
            Spacer(Modifier.height(8.dp))
            Text(
                text = subtaskFormState.titleError,
                color = MaterialTheme.colorScheme.error,
                style = additionalTypography.mediumText
            )
        }
        Spacer(Modifier.height(8.dp))

        TextField(
            value = creationState.description, //description,
            onValueChange = {viewModel.updateTaskCreation { copy(description = it) }},
            placeholder = { Text("Опис вашої задачі", style = MaterialTheme.typography.labelMedium) },
            modifier = Modifier.fillMaxWidth(),
            colors = TaskCreationTextFieldColors()
        )
        Spacer(modifier = Modifier.height(8.dp))
        Text("Тип задачі", style = MaterialTheme.typography.bodyLarge)
        Spacer(modifier = Modifier.height(4.dp))
        Column {
            RadioButton(selected = subtaskFormState.type == "standard",//typeTask == "standard",
                text = "Звичайна",
                onClick = { viewModel.updateSubtaskForm(subtaskId) { copy(type = "standard") }})//{ typeTask = "standart" })//{ registrationViewModel.updateField("gender", "f") })
            Spacer(modifier = Modifier.height(8.dp))
            RadioButton(selected = subtaskFormState.type == "scale",
                text = "Шкала",
                onClick = { viewModel.updateSubtaskForm(subtaskId) { copy(type = "scale") }})

            Spacer(modifier = Modifier.height(8.dp))
            RadioButton(selected = subtaskFormState.type == "repeatable",
                text = "Повторювальна",
                onClick = { viewModel.updateSubtaskForm(subtaskId) { copy(type = "repeatable") }})
            Spacer(modifier = Modifier.height(8.dp))
        }

        if(subtaskFormState.type == "scale") {
            Text("Одиниця вимірювання", style = MaterialTheme.typography.bodyLarge)
            TextField(
                value = subtaskFormState.unit,
                onValueChange = { viewModel.updateSubtaskForm(subtaskId) { copy(unit = it) }},
                placeholder = { Text("Введіть символ або назву", style = MaterialTheme.typography.labelMedium) },
                modifier = Modifier.fillMaxWidth(),
                colors = TaskCreationTextFieldColors()
            )
            if (subtaskFormState.unitValueError != null) {
                LaunchedEffect(subtaskFormState.unitValueError) {
                    delay(showErrorTime)
                    viewModel.updateSubtaskForm(subtaskId) { copy(unitValueError = null) }
                }
                Spacer(Modifier.height(8.dp))
                Text(
                    text = subtaskFormState.unitValueError,
                    color = MaterialTheme.colorScheme.error,
                    style = additionalTypography.mediumText
                )
            }
            Spacer(Modifier.height(8.dp))

            var targetValueInput by remember { mutableStateOf("") }
            Text("Ціль", style = MaterialTheme.typography.bodyLarge)
            TextField(
                value = targetValueInput,
                onValueChange = { input ->
                    targetValueInput = input
                    val doubleValue = input.toDoubleOrNull()
                    if (doubleValue != null) {
                        viewModel.updateSubtaskForm(subtaskId) { copy(targetValue = doubleValue)}
                    }
//                    val floatValue = input.toFloatOrNull()
//                    if (floatValue != null) {
//                        viewModel.updateSubtaskForm(subtaskId) { copy(targetValue = floatValue) }
//                    }
                },
                placeholder = { Text("Введіть число", style = MaterialTheme.typography.labelMedium) },
                modifier = Modifier.fillMaxWidth(),
                keyboardOptions = KeyboardOptions.Default.copy(keyboardType = KeyboardType.Number),
                colors = TaskCreationTextFieldColors()
            )
            if (subtaskFormState.targetValueError != null) {
                LaunchedEffect(subtaskFormState.targetValueError) {
                    delay(showErrorTime)
                    viewModel.updateSubtaskForm(subtaskId) { copy(targetValueError = null) }
                }
                Spacer(Modifier.height(8.dp))
                Text(
                    text = subtaskFormState.targetValueError,
                    color = MaterialTheme.colorScheme.error,
                    style = additionalTypography.mediumText
                )
            }
            Spacer(Modifier.height(8.dp))
        }

        if(subtaskFormState.type == "repeatable") {
            WeekdayEditSelector(
                markedDays = subtaskFormState.repeatDays,
                onMarkedDaysChange = { viewModel.updateSubtaskForm(subtaskId) { copy(repeatDays = it) } }
            )
            if (subtaskFormState.repeatDaysError != null) {
                LaunchedEffect(subtaskFormState.repeatDaysError) {
                    delay(showErrorTime)
                    viewModel.updateSubtaskForm(subtaskId) { copy(repeatDaysError = null) }
                }
                Spacer(Modifier.height(8.dp))
                Text(
                    text = subtaskFormState.repeatDaysError,
                    color = MaterialTheme.colorScheme.error,
                    style = additionalTypography.mediumText
                )
            }
        }
        Spacer(modifier = Modifier.height(8.dp))
        Row(horizontalArrangement = Arrangement.Center, modifier = Modifier.fillMaxWidth()) {

            SecondaryButton(
                text = "Видалити",
                onClick = onCancel,
                modifier = Modifier.weight(1f),
                style = ButtonStyle.Outline(Color(0xFF023047))
            )
        }
        Spacer(modifier = Modifier.height(8.dp))
    }
}

@Preview(showBackground = true)
@Composable
fun SubtaskCreationFormPreview() {
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
    SubtaskCreationForm(
        //{},
        "",
        {},
        viewModel
    )
}