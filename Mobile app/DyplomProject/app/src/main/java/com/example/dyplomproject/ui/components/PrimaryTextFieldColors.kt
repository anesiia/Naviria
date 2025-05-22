package com.example.dyplomproject.ui.components

import androidx.compose.material3.TextFieldColors
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.Color

@Composable
fun primaryTextFieldColors(): TextFieldColors {
    return TextFieldDefaults.colors(
        disabledTextColor = Color(0xFF023047),
        disabledLabelColor = Color(0xFFF0F3F6),
        focusedContainerColor = Color(0xFFF0F3F6),
        unfocusedContainerColor = Color(0xFFF0F3F6),// background
        focusedPlaceholderColor = Color(0x99023047),
        unfocusedPlaceholderColor = Color(0x99023047), // placeholder before typing
        focusedTextColor = Color(0xFF023047), //
        unfocusedTextColor = Color(0xFF023047),//0x99023047
        focusedIndicatorColor = Color(0xFFFF9800), // line of the textFieled
        unfocusedIndicatorColor = Color(0x33023047)
    )
}