package com.example.dyplomproject.ui.components.task.create

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Remove
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp

@Composable
fun ShowTaskCreationFormButton(
    isFormVisible: Boolean,
    onClick: () -> Unit
) {
    Row(
        modifier = Modifier
            .padding(vertical = 24.dp)
            .clickable { onClick() },
        horizontalArrangement = Arrangement.Center
    ) {
        Icon(
            imageVector = if (isFormVisible) Icons.Default.Remove else Icons.Default.Add,
            contentDescription = if (isFormVisible) "Видалити" else "Додати",
            tint = Color(0xFFFF8700)
        )
        Spacer(Modifier.width(16.dp))
        Text(if (isFormVisible) "Згорнути вікно додавання нового завдання" else "Додати нове завдання")
    }
}