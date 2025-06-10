package com.example.dyplomproject.ui.components.task.display

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.wrapContentWidth
import androidx.compose.foundation.shape.RoundedCornerShape
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
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.dyplomproject.R
import com.example.dyplomproject.data.remote.dto.SubtaskStandardDto
import com.example.dyplomproject.data.remote.Tag
import com.example.dyplomproject.data.remote.dto.TaskDto
import com.example.dyplomproject.data.remote.dto.TaskRepeatableDto
import com.example.dyplomproject.data.remote.dto.TaskScaleDto
import com.example.dyplomproject.data.remote.dto.TaskStandardDto
import com.example.dyplomproject.data.remote.dto.TaskWithSubtasksDto
import com.example.dyplomproject.data.remote.repository.TaskRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.GradientProgressBar
import com.example.dyplomproject.ui.components.task.ValueInput
import com.example.dyplomproject.ui.components.task.WeekdayCheckInView
import com.example.dyplomproject.ui.components.task.update.TaskModificationForm
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.TasksViewModel
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter

@Composable
fun TaskDisplay(task: TaskDto, viewModel: TasksViewModel) {
    var taskInfoVisible by remember { mutableStateOf(false) }//var subtasksVisible by remember { mutableStateOf(false) }
    var isChecked by remember { mutableStateOf(false) }
    var isEnabled by remember { mutableStateOf(true) }
    var isEditing by remember { mutableStateOf(false) } // NEW: edit mode flag

    val createdAtDate = ZonedDateTime.parse(task.createdAt).toLocalDate()
    val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd")

    if(isEditing) {
        TaskModificationForm(
            task = task,
            onSave = { updatedTask ->
                viewModel.updateTask(updatedTask)
                isEditing = false
            },
            onCancel = {
                isEditing = false
            },
            viewModel
        )
    } else {
        //before modifaction
//        Column(
//            modifier = Modifier
//                .fillMaxWidth()
//                .padding(8.dp)
//                .background(AppColors.Gray, RoundedCornerShape(8.dp))
//                .border(0.dp, AppColors.Orange, RoundedCornerShape(12.dp))
//                .padding(8.dp)
//            //.padding(end = 8.dp)
//        )
        val borderModifier = if (taskInfoVisible) {
            Modifier.border(1.dp, AppColors.Orange, RoundedCornerShape(8.dp))
        } else {
            Modifier.border(0.dp, Color.Companion.Transparent, RoundedCornerShape(8.dp))
        }
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .padding(8.dp)
                .background(AppColors.White, RoundedCornerShape(8.dp))
                .then(borderModifier)
                .padding(8.dp)
        ) {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    //.background(AppColors.Gray)
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
                            isEnabled = false
                        }
                        viewModel.updateTaskStatus(task)
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
                    if (taskInfoVisible) {
                        IconButton(onClick = { isEditing = true }) {
                            Icon(
                                painter = painterResource(id = R.drawable.ic_pencil),
                                contentDescription = "Олівець для модифікації"
                            )
                        }
                        IconButton(onClick = { viewModel.deleteTask(task.id) }) {
                            Icon(
                                painter = painterResource(id = R.drawable.ic_trash_can),
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
            }
            if (task is TaskScaleDto && taskInfoVisible) {
                var valueToAdd by remember { mutableStateOf("") }
                Spacer(Modifier.height(8.dp))
                ValueInput(valueToAdd,
                    { valueToAdd = it },
                    {
                        val current = valueToAdd.toIntOrNull() ?: 0
                        valueToAdd = (current + 1).toString()
                    },
                    {
                        val current = valueToAdd.toIntOrNull() ?: 0
                        val newValue = if (current > 0) current - 1 else 0
                        valueToAdd = newValue.toString()
                    },
                    { viewModel.updateTaskCurrentValue(task, valueToAdd.toInt())}
                )
            }
            if (taskInfoVisible) {
                Column(modifier = Modifier
                    .background(AppColors.Gray, RoundedCornerShape(8.dp))
                    .padding(start = 16.dp, end = 16.dp, bottom = 16.dp)) {
                    if (task is TaskScaleDto) {
                        Spacer(Modifier.height(16.dp))
                        Row (
                            verticalAlignment = Alignment.CenterVertically
                        ){

                            GradientProgressBar(
                                progress = (task.currentValue/task.targetValue).toFloat(), // assuming 0f to 1f
                                modifier = Modifier.weight(1f).padding(end = 16.dp), // take the remaining horizontal space
                                height = 12.dp,
                                gradientColors = listOf(Color(0xFF023047), Color(0xFF219DBB))
                            )
                            Text(
                                "XP: ${task.currentValue.toInt()} / ${task.targetValue}",//, ${user.levelInfo.progress}",
                                modifier = Modifier
                                    //.fillMaxWidth()
                                    .wrapContentWidth(Alignment.End)
                                    .padding(end = 16.dp),
                                style = additionalTypography.mediumText
                            )
                        }
                        Spacer(Modifier.height(16.dp))
                    }
                    if (task.description != null && task.description != "") {
                        Spacer(Modifier.height(8.dp))
                        Text(
                            task.description!!,
                            style = additionalTypography.regularText,
                            textAlign = TextAlign.Justify
                        )
                        Spacer(Modifier.height(8.dp))
                    } else {
                        Spacer(Modifier.height(8.dp))
                    }
                    if (task is TaskRepeatableDto && taskInfoVisible) {
                        Spacer(Modifier.height(8.dp))
                        Text("Дні для занять", style = additionalTypography.semiboldText, modifier = Modifier.align(Alignment.Start))
                        Spacer(Modifier.height(8.dp))
                        val context = LocalContext.current
                        WeekdayCheckInView(
                            markedDays = task.repeatDays,
                            checkedInDates = task.checkedInDays,
                            onCheckIn = { message, date ->
                                if (message == "Відмітка успішна!") {
                                    viewModel.checkInTask(task.id, date)
                                }
                                Toast.makeText(context, message, Toast.LENGTH_SHORT).show()
                            }
                        )
                        Spacer(Modifier.height(8.dp).align(Alignment.CenterHorizontally))
                        Column(
                            modifier = Modifier.fillMaxWidth(),
                            horizontalAlignment = Alignment.CenterHorizontally
                        ) {
                            Text(
                                "Відмічайте дні, натиснувши на них",
                                style = additionalTypography.regularText
                            )
                        }
                    }
                    Row (
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.SpaceBetween
                    ){
                        Text("Дата старту", style = additionalTypography.mediumText)
                        Text(createdAtDate.format(formatter), style = additionalTypography.mediumText)
                    }
                    if (task.deadline != null) {
                        val deadlineDate = ZonedDateTime.parse(task.deadline).toLocalDate()
                        Spacer(Modifier.height(8.dp))
                        Row (
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
            } else {
                HorizontalDivider()
            }
            if (task is TaskWithSubtasksDto &&!task.subtasks.isNullOrEmpty()) {
                Column(modifier = Modifier.padding(start = 16.dp)) {
                    task.subtasks.forEach { subtask ->
                        Spacer(Modifier.height(8.dp))
                        SubtaskDisplay(task.id, subtask, viewModel, task.createdAt, task.deadline)
                    }
                }
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun TaskDisplayPreview() {
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
// Креативний приклад TaskStandardDto
    val task1 = TaskStandardDto(
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
//        subtasks = listOf(
//            SubtaskCreateDto("sub1", "Перевірити паливні баки", true),
//            SubtaskCreateDto("sub2", "Провести навчання екіпажу", false)
//        ),
        createdAt = "2029-01-01T00:00:00Z",
        completedAt = null,
        status = "планується",
        //type = "standard"
    )

    val task2 = TaskRepeatableDto(
        id = "task-magic-007",
        userId = "wizard-007",
        folderId = "folder-spells",
        title = "Тренування магії щодня",
        description = "Практикувати заклинання, щоб стати найкращим магом світу.",
        categoryId = "magic",
        tags = listOf(Tag("Магія"), Tag("Щодня")),
        isDeadlineOn = false,
        deadline = null,
        isShownProgressOnPage = true,
        isNotificationsOn = true,
        notificationDate = "2025-05-28T07:00:00Z",
        notificationSent = false,
        priority = 5,
        subtasks = emptyList(),
//        subtasks = listOf(
//            SubtaskCreateDto("sub3", "Відпрацювати заклинання вогню", false)
//        ),
        createdAt = "2025-01-01T00:00:00Z",
        completedAt = null,
        status = "активне",
        repeatDays = listOf("Sunday"),
        checkedInDays = listOf("Вівторок", "Четвер"),
        //type = "repeatable"
    )

    val task3 = TaskScaleDto(
        id = "task-scale-999",
        userId = "step-counter",
        folderId = null,
        title = "Зробити трильйон кроків",
        description = "Крок за кроком до мети – трильйон кроків!",
        categoryId = "health",
        tags = listOf(Tag("Здоров'я"), Tag("Велика мета")),
        isDeadlineOn = true,
        deadline = "2035-12-31T23:59:59Z",
        isShownProgressOnPage = true,
        isNotificationsOn = true,
        notificationDate = "2035-12-30T20:00:00Z",
        notificationSent = false,
        priority = 7,
        subtasks = emptyList(),
        createdAt = "2025-05-28T00:00:00Z",
        completedAt = null,
        status = "в процесі",
        unit = "кроків",
        currentValue = 45.2,
        targetValue = 78.6,
        //type = "scale"
    )

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
//        subtasks = emptyList(),
        subtasks = listOf(
            SubtaskStandardDto("sub4", "Скласти схему електроніки", null, false),
//            SubtaskCreateDto("sub5", "Закодувати поведінку", false),
//            SubtaskCreateDto("sub6", "Зібрати корпус", false)
        ),
        createdAt = "2025-04-01T10:00:00Z",
        completedAt = null,
        status = "розробка",
        //type = "with_subtasks"
    )
    Column {
        //TaskDisplay(task1, viewModel)
        //Spacer(Modifier.height(8.dp))
        TaskDisplay(task2, viewModel)
        Spacer(Modifier.height(8.dp))
        TaskDisplay(task3, viewModel)
        Spacer(Modifier.height(8.dp))
        TaskDisplay(task4, viewModel)
        Spacer(Modifier.height(8.dp))
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