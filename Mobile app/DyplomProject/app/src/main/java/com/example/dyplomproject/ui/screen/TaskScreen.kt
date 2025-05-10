package com.example.dyplomproject.ui.screen
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.TextUnit
import androidx.compose.ui.unit.sp
import androidx.navigation.NavHostController

@Composable
fun TaskScreen(
    navController: NavHostController,
    backgroundColor: Color = Color.Green,
    text: String = "Task screen",
    textColor: Color = Color.White,
    textSize: TextUnit = 24.sp,
    padding: PaddingValues
) {
    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(color = backgroundColor)
            .padding(padding),
        contentAlignment = Alignment.Center
    ) {
        Text(
            text = text,
            color = textColor,
            fontSize = textSize
        )
    }
}