package com.example.dyplomproject.ui.components
import androidx.compose.animation.core.animateDpAsState
import androidx.compose.animation.core.tween
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.MaterialTheme
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.util.fastCbrt
import com.example.dyplomproject.ui.theme.AppColors

@Composable
fun SwitchButton(
    isOn: Boolean,
    onToggle: (Boolean) -> Unit
) {
    //var isOn by remember { mutableStateOf(false) }

    // Animate the position of the knob
    val knobOffset by animateDpAsState(
        targetValue = if (isOn) 34.dp else 2.dp,
        animationSpec = tween(durationMillis = 300), label = ""
    )

    Box(
        modifier = Modifier
            .width(60.dp)
            .height(28.dp)
            .clip(CircleShape)
            .background(
                brush = if (isOn)
                    Brush.horizontalGradient(listOf(AppColors.Orange, AppColors.Yellow))
                else
                    Brush.horizontalGradient(listOf(AppColors.Blue, AppColors.DarkBlue))
            )
            .clickable { onToggle(!isOn) },
        contentAlignment = Alignment.CenterStart
    ) {
        Box(
            modifier = Modifier
                .offset(x = knobOffset)
                .size(24.dp)
                .clip(CircleShape)
                .background(Color.White)
        )
    }
}

@Preview(showBackground = true)
@Composable
fun CustomSwitchPreview() {
    MaterialTheme {
        var switchState by remember { mutableStateOf(false) }
        Box(modifier = Modifier.padding(16.dp)) {
            SwitchButton(
                isOn = switchState,
                onToggle = { switchState = it }
            )
        }
    }
}