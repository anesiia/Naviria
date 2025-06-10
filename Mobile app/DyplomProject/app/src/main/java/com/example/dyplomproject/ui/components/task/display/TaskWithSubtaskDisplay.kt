package com.example.dyplomproject.ui.components.task.display

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ExpandLess
import androidx.compose.material.icons.filled.ExpandMore
import androidx.compose.material3.Checkbox
import androidx.compose.material3.CheckboxDefaults
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.dyplomproject.R
import com.example.dyplomproject.data.remote.dto.SubtaskRepeatableDto
import com.example.dyplomproject.data.remote.dto.SubtaskScaleDto
import com.example.dyplomproject.data.remote.dto.SubtaskStandardDto
import com.example.dyplomproject.data.remote.Tag
import com.example.dyplomproject.data.remote.dto.TaskWithSubtasksDto
import com.example.dyplomproject.data.remote.repository.TaskRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.TasksViewModel
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter

@Composable
fun TaskWithSubtasksDisplay(task: TaskWithSubtasksDto, viewModel: TasksViewModel) {
    var taskInfoVisible by remember { mutableStateOf(true) }//var subtasksVisible by remember { mutableStateOf(false) }
    var isChecked by remember { mutableStateOf(false) }
    var isEnabled by remember { mutableStateOf(true) }
    val createdAtDate = ZonedDateTime.parse(task.createdAt).toLocalDate()
    val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd")

    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(8.dp)
            .background(Color.Transparent)//, RoundedCornerShape(8.dp))
            //.border(0.dp, AppColors.Orange, RoundedCornerShape(12.dp))
            .padding(8.dp)
        //.padding(end = 8.dp)
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .clickable {
                    if (!task.subtasks.isNullOrEmpty()) {
                        //subtasksVisible = !subtasksVisible??
                        //taskInfoVisible = !taskInfoVisible
                    }
                },
            verticalAlignment = Alignment.CenterVertically
        ) {
            Checkbox(
                checked = isChecked,
                onCheckedChange = { checked ->
                    if (checked) {
                        isChecked = true
                        isEnabled = false // Disable further changes
                    }
                    // If unchecked is attempted, ignore it
                },
                enabled = isEnabled,
                modifier = Modifier
                    .size(24.dp)
                    .align(Alignment.CenterVertically),
                colors = CheckboxDefaults.colors(
                    checkedColor = AppColors.DarkBlue,
                    uncheckedColor = AppColors.Orange,
                    checkmarkColor = AppColors.Yellow
                )
            )
            Text(
                text = task.title,
                style = MaterialTheme.typography.bodyLarge,
                maxLines = 2,
                overflow = TextOverflow.Ellipsis,
                modifier = Modifier
                    .weight(1f)
                    .padding(start = 8.dp, end = 8.dp)
            )
            Row (
                horizontalArrangement = Arrangement.spacedBy(0.dp),
                verticalAlignment = Alignment.CenterVertically
            ){
                if(taskInfoVisible) {
                    IconButton(onClick = { }) {
                        Icon(
                            painter = painterResource(id = R.drawable.ic_pencil), // Replace with your icon resource
                            contentDescription = "Олівець для модифікації"
                        )
                    }
                    IconButton(onClick = { viewModel.deleteTask(task.id) }) {
                        Icon(
                            painter = painterResource(id = R.drawable.ic_trash_can), // Replace with your icon resource
                            contentDescription = ""
                        )
                    }
                }

                IconButton(onClick = { taskInfoVisible = !taskInfoVisible}) {//subtasksVisible = !subtasksVisible }) {
                    Icon(
                        imageVector = if(!taskInfoVisible) Icons.Default.ExpandLess else Icons.Default.ExpandMore,//(!subtasksVisible) Icons.Default.ExpandLess else Icons.Default.ExpandMore,
                        contentDescription = if(!taskInfoVisible)  "Show info" else "Hide info"//(!subtasksVisible) "Show subtasks" else "Hide subtasks"
                    )
                }
            }


            //Spacer(Modifier.width(4.dp))
//            if (!task.subtasks.isNullOrEmpty()) {
//                Icon(
//                    imageVector = if (subtasksVisible) Icons.Default.ExpandLess else Icons.Default.ExpandMore,
//                    contentDescription = null
//                )
//                Spacer(Modifier.width(4.dp))
//            }

        }
        HorizontalDivider()
        if (taskInfoVisible) {
            Column(modifier = Modifier
                .background(Color.Transparent)
                .padding(16.dp)) {
//                tasks.forEach { task ->
//                    Text("- ${task.title}", style = MaterialTheme.typography.bodySmall)
//                }
                if (task.description != null) {
                    Text(
                        task.description!!,
                        style = additionalTypography.regularText,
                        textAlign = TextAlign.Justify
                    )
                    Spacer(Modifier.height(8.dp))
                }
                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.SpaceBetween
                ) {
                    Text("Дата старту", style = additionalTypography.mediumText)
                    Text(createdAtDate.format(formatter), style = additionalTypography.mediumText)
                }
                if (task.deadline != null) {
                    val deadlineDate = ZonedDateTime.parse(task.deadline).toLocalDate()
                    Spacer(Modifier.height(8.dp))
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.SpaceBetween
                    ) {
                        Text("Дедлайн", style = additionalTypography.mediumText)
                        Text(
                            deadlineDate.format(formatter),
                            style = additionalTypography.mediumText
                        )
                    }
                }
            }
        }
        if (taskInfoVisible && !task.subtasks.isNullOrEmpty()) {
            Column(modifier = Modifier.padding(start = 16.dp)) {
                task.subtasks.forEach { subtask ->
                    Spacer(Modifier.height(8.dp))
                    SubtaskDisplay(task.id, subtask, viewModel, task.createdAt, task.deadline)
                }
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun TaskWithSubtasksDisplayPreview() {
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

    val task4 = TaskWithSubtasksDto(
        id = "task-robot-2025",
        userId = "robot-engineer",
        folderId = "folder-robots",
        title = "Побудувати робота помічника",
        description = "Робот, що допоможе у повсякденних справах і готує каву!",
        categoryId = "engineering",
        tags = listOf(Tag("Роботи"), Tag("Інженерія")),
        isDeadlineOn = false,
        deadline = null,
        isShownProgressOnPage = true,
        isNotificationsOn = false,
        notificationDate = null,
        notificationSent = false,
        priority = 8,
        //subtasks = emptyList(),
        subtasks = listOf(
            SubtaskStandardDto("sub4", "Скласти схему електроніки", "Гарний опис - це короткий опис!", false),
            SubtaskRepeatableDto("sub5", "Закодувати поведінку", "Гарний опис - короткий", repeatDays = listOf("Sunday"), checkedInDays = emptyList()),
            SubtaskScaleDto("sub5", "Закодувати поведінку", "Гарний опис - короткий", unit = "к", currentValue = 100.toDouble(), targetValue = 1000.toDouble()),
//            SubtaskCreateDto("sub6", "Зібрати корпус", false)
        ),
        createdAt = "2025-04-01T10:00:00Z",
        completedAt = null,
        status = "розробка",
        //type = "with_subtasks"
    )
    Column {
        TaskWithSubtasksDisplay(task4, viewModel)
        Spacer(Modifier.height(8.dp))
    }
}