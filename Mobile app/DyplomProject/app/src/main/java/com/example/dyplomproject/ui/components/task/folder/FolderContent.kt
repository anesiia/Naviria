package com.example.dyplomproject.ui.components.task.folder

import android.util.Log
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
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
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.data.remote.FolderWithTasksDto
import com.example.dyplomproject.data.remote.dto.TaskDto
import com.example.dyplomproject.ui.components.task.create.ShowTaskCreationFormButton
import com.example.dyplomproject.ui.components.task.create.TaskCreationForm
import com.example.dyplomproject.ui.components.task.display.TaskSection
import com.example.dyplomproject.ui.viewmodel.TasksUiState
import com.example.dyplomproject.ui.viewmodel.TasksViewModel

@Composable
fun FolderContent(uiState: TasksUiState, viewModel: TasksViewModel) {
    val selectedFolder = uiState.selectedFolder
    if (selectedFolder == null) {
        Box(
            modifier = Modifier.padding(32.dp),
        ) {
            Text(
                text = "Жодна папка не обрана! \nЯкщо на разі жодної папки не створено, створіть папку для подальшого створення задач!",
                style = MaterialTheme.typography.labelMedium,
                modifier = Modifier.fillMaxWidth(),
                color = Color(0xFF063B52),
                textAlign = TextAlign.Justify
            )
        }
        return
    }
    uiState.foldersWithTasks.forEach {
        Log.e("FOLDER CONTENT","Checking folder ID: ${it.folderId}")
    }
    val folderWithTasks: FolderWithTasksDto? = uiState.foldersWithTasks.find { it.folderId == selectedFolder.id }//?: return
    val tasks: List<TaskDto>
    val groupedTasks: Map<String, List<TaskDto>>
    if (folderWithTasks != null) {
        tasks = folderWithTasks.tasks
        groupedTasks = tasks.groupBy { it.status }
    } else {
        tasks = emptyList()
        groupedTasks = emptyMap()
    }
    var showTaskCreationForm by remember { mutableStateOf(false) }

    Column(
        modifier = Modifier
            .verticalScroll(rememberScrollState())
            .fillMaxSize()
    ) {
        Box(modifier = Modifier.padding(horizontal = 16.dp)){
            ShowTaskCreationFormButton(
                isFormVisible = showTaskCreationForm,
                onClick = {
                    showTaskCreationForm = !showTaskCreationForm
                }
            )
        }

        if (showTaskCreationForm) {
            Spacer(Modifier.height(8.dp))
            TaskCreationForm(
                onSave = { showTaskCreationForm = false },
                onCancel = { showTaskCreationForm = false },
                uiState.categories,
                viewModel = viewModel
            )
        }
        if (tasks.isEmpty()) {
            Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                Text(
                    "Папка порожня!",
                    style = MaterialTheme.typography.titleMedium,
                )
            }

        } else {
            Spacer(Modifier.height(8.dp))
            val completedTasks = groupedTasks["Completed"].orEmpty() + groupedTasks["DeadlineMissed"].orEmpty() + groupedTasks["CompletedInTime"].orEmpty() + groupedTasks["CompletedNotInTime"].orEmpty()
            TaskSection(title = "В процесі", tasks = groupedTasks["InProgress"].orEmpty(), viewModel = viewModel)
            TaskSection(title = "Виконано", tasks = completedTasks, viewModel = viewModel)
        }
    }
}