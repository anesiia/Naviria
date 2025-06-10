package com.example.dyplomproject.ui.screen
import android.util.Log
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.navigation.NavHostController
import com.example.dyplomproject.ui.viewmodel.Folder
import com.example.dyplomproject.ui.viewmodel.TasksViewModel
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.tooling.preview.Preview
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.dyplomproject.data.remote.repository.TaskRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.task.folder.FolderContent
import com.example.dyplomproject.ui.components.task.folder.FolderTabs
import com.example.dyplomproject.ui.components.task.folder.FolderEditBottomSheet
import com.example.dyplomproject.ui.theme.AppColors

@Composable
fun TaskScreen(navController: NavHostController,
               userId: String,
               padding: PaddingValues) {
    val repository = remember { TaskRepository(RetrofitInstance.api) }
    val viewModel: TasksViewModel = viewModel(factory = object: ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T: ViewModel> create(modelClass: Class<T>) : T {
            if (modelClass.isAssignableFrom(TasksViewModel::class.java)) {
                return TasksViewModel(repository, userId) as T
            }
            throw IllegalArgumentException("Unknown ViewModel class")
        }
    })
    val uiState by viewModel.uiState.collectAsState()
    Log.d("Composable", "Folders size: ${uiState.folders.size}, selected: ${uiState.selectedFolder?.name}")
    var showBottomSheet by remember { mutableStateOf(false) }
    var editingFolder by remember { mutableStateOf<Folder?>(null) }

    LaunchedEffect(Unit) {
        viewModel.loadUserFolders()
        viewModel.loadCategories()
        viewModel.loadTasks()
    }
    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(padding)
            .background(AppColors.White)
    ) {
        FolderTabs(
            folders = uiState.folders,
            selectedFolder = uiState.selectedFolder,
            onFolderSelected = { folder -> viewModel.selectFolder(folder) },
            onAddFolder = {
                editingFolder = null
                showBottomSheet = true
            },
            onEditFolder = { folder ->
                editingFolder = folder
                showBottomSheet = true
            },
            onDeleteFolder = { folder ->
                viewModel.deleteFolderOnServer(folder.id) { /* можна обробити після видалення */ }
            }
        )

        if (showBottomSheet) {
            FolderEditBottomSheet(
                folderNameInitial = editingFolder?.name ?: "",
                isNew = (editingFolder == null),
                onRename = { name ->
                    if (editingFolder == null) {
                        viewModel.addFolderOnServer(name) {
                            showBottomSheet = false
                        }
                    } else {
                        viewModel.updateFolderOnServer(editingFolder!!.id, name) {
                            showBottomSheet = false
                        }
                    }
                },
                onDelete = if (editingFolder != null) {
                    {
                        viewModel.deleteFolderOnServer(editingFolder!!.id) {
                            showBottomSheet = false
                        }
                    }
                } else null,
                onDismiss = {
                    showBottomSheet = false
                }
            )
        }

        if (uiState.isLoading) {
            CircularProgressIndicator()//  modifier = Modifier.align(Alignment.Center)
        }
        FolderContent(uiState, viewModel)
    }
}



@Composable
fun AddTaskButton(onClick: () -> Unit) {
    Row(
        modifier = Modifier
            .padding(vertical = 24.dp)
            .clickable { onClick() },
        horizontalArrangement = Arrangement.Center
    ) {
        Icon(
            imageVector = Icons.Default.Add,
            contentDescription = "Додати",
            tint = Color(0xFFFF8700) // This sets the icon color to orange
        )
        Spacer(Modifier.width(16.dp))
        Text("Додати завдання")//, modifier = Modifier.align(Alignment.CenterVertically))
    }
}


data class Task(
    //val id: String,
    val userId: String,
    val folderId: String,
    val description: String,
    val title: String,
    val categoryId: String,
    val tags: Map<String, String>, //"tagName" to "sport"
    val isDeadlineOn: Boolean,
    val deadline: String, //"2025-05-26T00:42:46.715Z"
    val isShownProgressOnPage: Boolean,
    val isNotificationsOn: Boolean,
    val notificationDate: String, //"2025-05-26T00:42:46.715Z"
    val priority: Int, //0
    val type: String, //"standart"
    val subtasks: List<Subtask>,
    val status: String //"InProgress" ""
)

data class Subtask(
    val title: String,
    val description: String,
    val type: String, //String
)

data class Folder(
    val id: String,
    val name: String,
    val tasks: List<Task> = emptyList()
)

@Preview(showBackground = true)
@Composable
fun FolderTabsPreview() {
//    var selectedFolder by remember { mutableStateOf(sampleFolders[0]) }
//
//    FolderTabs(
//        folders = sampleFolders,
//        selectedFolder = selectedFolder,
//        onFolderSelected = { selectedFolder = it },
//        onAddFolder = {
//            // No-op for preview
//        },
//        {},
//        {}
//    )
}