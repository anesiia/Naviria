package com.example.dyplomproject.ui.components.task.display

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ExpandLess
import androidx.compose.material.icons.filled.ExpandMore
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.data.remote.dto.TaskDto
import com.example.dyplomproject.ui.features.friends.DoneTaskDisplay
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.TasksViewModel

@Composable
fun TaskSection(title: String, tasks: List<TaskDto>, viewModel: TasksViewModel) {
    var expanded by remember { mutableStateOf(true) }

    Column(modifier = Modifier.fillMaxWidth().padding(horizontal = 8.dp)) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .clickable { expanded = !expanded }
                .padding(vertical = 16.dp, horizontal = 16.dp),
            verticalAlignment = Alignment.CenterVertically
        ) {
            Icon(
                imageVector = if (expanded) Icons.Default.ExpandLess else Icons.Default.ExpandMore,
                contentDescription = null
            )
            Spacer(Modifier.width(8.dp))
            Text(title, style = additionalTypography.semiboldText)
        }

        if (expanded) {
            tasks.forEach { task ->
//                if (task is TaskWithSubtasksDto) {
//                    TaskWithSubtasksDisplay(task, viewModel)
//                } else {
//                    TaskDisplay(task, viewModel)
//                }
                if(task.status == "InProgress") {
                    TaskDisplay(task, viewModel)
                } else {
                    DoneTaskDisplay(task, viewModel)
                }
            }
        }
    }
}

