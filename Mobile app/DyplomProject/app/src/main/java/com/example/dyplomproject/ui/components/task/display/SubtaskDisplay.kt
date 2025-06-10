package com.example.dyplomproject.ui.components.task.display

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.border
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
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.R
import com.example.dyplomproject.data.remote.dto.SubtaskDto
import com.example.dyplomproject.data.remote.dto.SubtaskRepeatableDto
import com.example.dyplomproject.data.remote.dto.SubtaskScaleDto
import com.example.dyplomproject.ui.components.GradientProgressBar
import com.example.dyplomproject.ui.components.task.ValueInput
import com.example.dyplomproject.ui.components.task.WeekdayCheckInView
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.TasksViewModel
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter

@Composable
fun SubtaskDisplay(
    taskId: String,
    subtask: SubtaskDto,
    viewModel: TasksViewModel,
    createDateString: String,
    deadlineDateString: String?
) {
    var taskInfoVisible by remember { mutableStateOf(true) }//var subtasksVisible by remember { mutableStateOf(false) }
    var isChecked by remember { mutableStateOf(false) }
    var isEnabled by remember { mutableStateOf(true) }
    //val createdAtDate = ZonedDateTime.parse(subtask.createdAt).toLocalDate()
    val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd")

    val backgroundColor = if (taskInfoVisible) AppColors.White else AppColors.White
    //val roundedCornerShapeSize = if (taskInfoVisible) 20.dp else 20.dp//20.dp else 50.dp
    val borderModifier = if (taskInfoVisible) {
        Modifier.border(1.dp, AppColors.Orange, RoundedCornerShape(8.dp))
    } else {
        Modifier.border(0.dp, Color.Companion.Transparent, RoundedCornerShape(8.dp))
    }
    val mainTopRowPadding = if (taskInfoVisible) 0.dp else 0.dp
    Column(
        modifier = Modifier
            .fillMaxWidth()
            //.padding(8.dp)
            .background(backgroundColor, RoundedCornerShape(8.dp))
            .then(borderModifier)
            //.padding(8.dp)
        //.padding(end = 8.dp)
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth().padding(8.dp).padding(top = mainTopRowPadding, bottom = mainTopRowPadding),//
//                .clickable {
//                    if (!.subtasks.isNullOrEmpty()) {
//                        //subtasksVisible = !subtasksVisible??
//                        //taskInfoVisible = !taskInfoVisible
//                    }
//                },
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
                text = subtask.title,
                style = MaterialTheme.typography.bodyLarge,
                maxLines = 2,
                overflow = TextOverflow.Ellipsis,
                modifier = Modifier
                    .weight(1f)
                    .padding(start = 8.dp, end = 8.dp)
            )
            Row(
                horizontalArrangement = Arrangement.spacedBy(0.dp),
                verticalAlignment = Alignment.CenterVertically
            ) {
                if (taskInfoVisible) {
                    IconButton(onClick = { }) {
                        Icon(
                            painter = painterResource(id = R.drawable.ic_pencil), // Replace with your icon resource
                            contentDescription = "Олівець для модифікації"
                        )
                    }
                    IconButton(onClick = { }) {
                        Icon(
                            painter = painterResource(id = R.drawable.ic_trash_can), // Replace with your icon resource
                            contentDescription = ""
                        )
                    }
                }

                IconButton(onClick = {
                    taskInfoVisible = !taskInfoVisible
                }) {//subtasksVisible = !subtasksVisible }) {
                    Icon(
                        imageVector = if (!taskInfoVisible) Icons.Default.ExpandLess else Icons.Default.ExpandMore,//(!subtasksVisible) Icons.Default.ExpandLess else Icons.Default.ExpandMore,
                        contentDescription = if (!taskInfoVisible) "Show info" else "Hide info"//(!subtasksVisible) "Show subtasks" else "Hide subtasks"
                    )
                }
            }
        }
        if (subtask is SubtaskScaleDto && taskInfoVisible) {
            Row (modifier = Modifier.padding(start = 8.dp, end = 8.dp).background(Color.White)){
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
                    { viewModel.updateSubtaskCurrentValue(taskId, subtask, valueToAdd.toInt())}
                )
            }
        }

        if (subtask is SubtaskRepeatableDto && taskInfoVisible) {
            var message by remember { mutableStateOf("") }
            Column (modifier = Modifier.background(AppColors.White).padding(start = 8.dp, end = 8.dp)){
                //Spacer(Modifier.height(8.dp))
                Text("Дні для занять", style = additionalTypography.semiboldText)
                Spacer(Modifier.height(8.dp))
                val context = LocalContext.current
                WeekdayCheckInView(
                    markedDays = subtask.repeatDays,
                    checkedInDates = subtask.checkedInDays,
                    onCheckIn = { message, date ->
                        if (message == "Відмітка успішна!") {
                            viewModel.checkInTask(subtask.id, date)
                        }
                        Toast.makeText(context, message, Toast.LENGTH_SHORT).show()
                    }
                )
                Spacer(Modifier.height(8.dp))
                if (message.isNotBlank()) {
                    Text(text = message, color = Color.Red)
                }
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
        }

        if (taskInfoVisible) {
            Column(
                modifier = Modifier
                    .background(AppColors.Gray, RoundedCornerShape(8.dp))
                    .padding(start = 16.dp, end = 16.dp, bottom = 16.dp)
            ) {
                if (subtask is SubtaskScaleDto) {
                    Spacer(Modifier.height(16.dp))
                    Row (
                        verticalAlignment = Alignment.CenterVertically
                    ){
                        GradientProgressBar(
                            progress = (subtask.currentValue/subtask.targetValue).toFloat(), // assuming 0f to 1f
                            modifier = Modifier.weight(1f).padding(end = 16.dp), // take the remaining horizontal space
                            height = 12.dp,
                            gradientColors = listOf(Color(0xFF023047), Color(0xFF219DBB))
                        )
                        Text(
                            " ${subtask.currentValue.toInt()} / ${subtask.targetValue.toInt()}",//, ${user.levelInfo.progress}",
                            modifier = Modifier
                                //.fillMaxWidth()
                                .wrapContentWidth(Alignment.End)
                                .padding(end = 16.dp),
                            style = additionalTypography.mediumText
                        )
                    }
                    //Spacer(Modifier.height(16.dp))
                }
                if (subtask.description != null && subtask.description != "") {
                    Spacer(Modifier.height(8.dp))
                    Text(
                        subtask.description!!,
                        style = additionalTypography.regularText,
                        textAlign = TextAlign.Justify
                    )
                    //Spacer(Modifier.height(8.dp))
                }
                Spacer(Modifier.height(8.dp))
                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.SpaceBetween
                ) {
                    val createDate = ZonedDateTime.parse(createDateString).toLocalDate()
                    Text("Дата старту", style = additionalTypography.mediumText)
                    Text(createDate.format(formatter), style = additionalTypography.mediumText)
                }
            }
            if (deadlineDateString != null) {
                val deadlineDate = ZonedDateTime.parse(deadlineDateString).toLocalDate()
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
}