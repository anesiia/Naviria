package com.example.dyplomproject.ui.components.task.folder

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.combinedClickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyRow
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.ui.viewmodel.Folder

@OptIn(ExperimentalMaterial3Api::class, ExperimentalFoundationApi::class)
@Composable
fun FolderTabs(
    folders: List<Folder>,
    selectedFolder: Folder?,
    onFolderSelected: (Folder) -> Unit,
    onAddFolder: () -> Unit,
    onEditFolder: (Folder) -> Unit,
    onDeleteFolder: (Folder) -> Unit
) {
    LazyRow(
        modifier = Modifier.fillMaxWidth(),
        contentPadding = PaddingValues(horizontal = 8.dp, vertical = 8.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        item {
            IconButton(onClick = onAddFolder) {
                Icon(Icons.Default.Add, contentDescription = "Add Folder")
            }
        }

        items(folders, key = { it.id }) { folder ->
            val isSelected = folder == selectedFolder
            val background = if (isSelected) Color.White else Color(0xFFF0F0F0)
            val borderColor = if (isSelected) Color.LightGray else Color.Transparent

            Box(
                modifier = Modifier
                    .padding(horizontal = 4.dp)
                    .clip(RoundedCornerShape(topStart = 12.dp, topEnd = 12.dp))
                    .background(background)
                    .border(1.dp, borderColor, RoundedCornerShape(topStart = 12.dp, topEnd = 12.dp))
                    .combinedClickable(
                        onClick = { onFolderSelected(folder) },
                        onLongClick = {
                            // Could open a dialog or dropdown to edit/delete
                            onEditFolder(folder)
                        }
                    )
                    .padding(horizontal = 16.dp, vertical = 10.dp)
            ) {
                Text(folder.name)
            }
        }
    }
}