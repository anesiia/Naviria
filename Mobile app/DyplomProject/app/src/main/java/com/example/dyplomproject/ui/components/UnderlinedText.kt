package com.example.dyplomproject.ui.components
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.material3.MaterialTheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.SolidColor
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.TextUnit
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.material3.Text

@Composable
fun UnderlinedText(
    text: String,
    fontSize: TextUnit = 24.sp,
    fontWeight: FontWeight = FontWeight.SemiBold,
    underlineColor: Color = Color(0xFF005580),
    underlineThickness: Dp = 2.dp,
    underlineHorizontalPadding: Dp = 8.dp,
    spacingBetweenTextAndLine: Dp = 8.dp,
    useGradient: Boolean = false,
    gradientColors: List<Color> = listOf(Color(0xFFFF9800), Color(0xFFFFC107)),
    modifier: Modifier = Modifier
) {
    Column(
        modifier = modifier,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Text(
            text = text,
            fontSize = fontSize,
            fontWeight = fontWeight,
            modifier = Modifier.padding(bottom = spacingBetweenTextAndLine)
        )
        Box(
            modifier = Modifier
                .fillMaxWidth()
                .padding(horizontal = underlineHorizontalPadding)
                .height(underlineThickness)
                .background(
                    brush = if (useGradient) {
                        Brush.linearGradient(gradientColors)
                    } else {
                        SolidColor(underlineColor)
                    }
                )
        )
    }
}

@Preview(showBackground = true)
@Composable
fun UnderlinedTextPreview() {
    MaterialTheme {
        Column {
            Text( text = "lol")

            UnderlinedText(
                text = "Sample Text",
                useGradient = true // Try setting this to false to see solid underline
            )
            Text( text = "lol")
        }

    }
}