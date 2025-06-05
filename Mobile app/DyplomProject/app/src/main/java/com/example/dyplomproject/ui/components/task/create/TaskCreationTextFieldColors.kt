package com.example.dyplomproject.ui.components.task.create

import androidx.compose.material3.TextFieldColors
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.Color

@Composable
fun TaskCreationTextFieldColors(): TextFieldColors {
    return TextFieldDefaults.colors(
        disabledTextColor = Color(0xFF7A8B99),
        focusedContainerColor = Color.Transparent,
        unfocusedContainerColor = Color.Transparent,
        focusedPlaceholderColor = Color(0x99023047),
        unfocusedPlaceholderColor = Color(0x99023047),
        focusedTextColor = Color(0xFF023047),
        unfocusedTextColor = Color(0x99023047),
        focusedIndicatorColor = Color(0xFFFF9800),
        unfocusedIndicatorColor = Color(0x33023047)
    )
}