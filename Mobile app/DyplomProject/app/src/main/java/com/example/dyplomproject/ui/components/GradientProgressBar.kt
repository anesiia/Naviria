package com.example.dyplomproject.ui.components

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp

@Composable
fun GradientProgressBar(
    progress: Float, // value between 0f and 1f
    modifier: Modifier = Modifier,
    height: Dp = 8.dp,
    cornerRadius: Dp = 50.dp, // high enough for rounded ends
    backgroundColor: Color = Color.LightGray,
    gradientColors: List<Color> = listOf(Color.Red, Color.Magenta, Color.Blue)
) {
    val brush = Brush.linearGradient(gradientColors)

    Box(
        modifier
            .fillMaxWidth()
            .height(height)
            .clip(RoundedCornerShape(cornerRadius))
            .background(backgroundColor)
    ) {
        Box(
            Modifier
                .fillMaxHeight()
                .fillMaxWidth(progress.toFloat())
                .background(brush)
        )
    }
}

@Preview(showBackground = true)
@Composable
fun GradientProgressBarPreview() {
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp)
    ) {
        Text("Progress: 70%", modifier = Modifier.padding(bottom = 8.dp))

        GradientProgressBar(
            progress = 0.7f,
            height = 12.dp,
            cornerRadius = 12.dp,
            gradientColors = listOf(Color(0xFF00C6FF), Color(0xFF0072FF)) // blue gradient
        )
    }
}