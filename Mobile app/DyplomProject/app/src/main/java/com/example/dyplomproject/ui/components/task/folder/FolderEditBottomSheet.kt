package com.example.dyplomproject.ui.components.task.folder

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.ModalBottomSheet
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.material3.rememberModalBottomSheetState
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun FolderEditBottomSheet(
    folderNameInitial: String,
    isNew: Boolean,
    onRename: (String) -> Unit,
    onDelete: (() -> Unit)? = null,
    onDismiss: () -> Unit
) {
    var folderName by remember { mutableStateOf(folderNameInitial) }
    LaunchedEffect(folderNameInitial) {
        folderName = folderNameInitial
    }
    ModalBottomSheet(
        onDismissRequest = onDismiss,
        sheetState = rememberModalBottomSheetState(skipPartiallyExpanded = true)
    ) {
        Column(Modifier.padding(16.dp)) {
            Text(if (isNew) "Створення нової папки" else "Редагування нової папки", style = MaterialTheme.typography.titleMedium)
            Spacer(Modifier.height(8.dp))

            OutlinedTextField(
                value = folderName,
                onValueChange = { folderName = it },
                label = { Text("Назва папки") },
                modifier = Modifier.fillMaxWidth()
            )

            Spacer(Modifier.height(16.dp))

            Row(
                horizontalArrangement = Arrangement.SpaceBetween,
                modifier = Modifier.fillMaxWidth()
            ) {
                if (!isNew && onDelete != null) {
                    TextButton(onClick = {
                        onDelete()
                        onDismiss()
                    }) {
                        Text("Видалити", color = Color.Red)
                    }
                }

                Spacer(Modifier.width(8.dp))

                TextButton(onClick = {
                    onRename(folderName)
                    onDismiss()
                }) {
                    Text(if (isNew) "Створити" else "Зберігти")
                }
            }
        }
    }
}