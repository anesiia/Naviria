package com.example.dyplomproject.ui.components

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.interaction.MutableInteractionSource
import androidx.compose.foundation.interaction.collectIsPressedAsState
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.alpha
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import androidx.compose.material.ripple.rememberRipple
import androidx.compose.ui.graphics.SolidColor
import androidx.compose.ui.text.style.TextOverflow
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography

@Composable
fun SecondaryButton(
    text: String,
    onClick: () -> Unit,
    modifier: Modifier = Modifier.fillMaxWidth(),
    style: ButtonStyle = ButtonStyle.Primary,
    enabled: Boolean = true
) {
    val shape = RoundedCornerShape(style.cornerRadius)

    // Visual feedback for disabled state
    val backgroundColor = if (enabled) {
        style.gradient ?: SolidColor(style.backgroundColor)
    } else {
        SolidColor(style.backgroundColor.copy(alpha = 0.4f))
    }

    val contentColor = if (enabled) {
        style.contentColor
    } else {
        style.contentColor.copy(alpha = 0.4f)
    }

    val borderColor = if (enabled) {
        style.borderColor
    } else {
        style.borderColor.copy(alpha = 0.3f)
    }

    Box(
        modifier = modifier
            .clip(shape)
            .background(brush = backgroundColor, shape = shape)
            .border(
                width = if (style.borderColor != Color.Transparent) 1.dp else 0.dp,
                color = borderColor,
                shape = shape
            )
            .let {
                if (enabled) it.clickable(onClick = onClick) else it
            }
    ) {
        Text(
            text = text,
            color = contentColor,
            style = additionalTypography.regularText,
            maxLines = 1,
            overflow = TextOverflow.Ellipsis,
            modifier = Modifier
                .align(Alignment.Center)
                .padding(8.dp)
        )
    }
}
//    val shape = RoundedCornerShape(style.cornerRadius)
//
//    Box(
//        modifier = modifier
//            .clip(shape)
//            .background(
//                brush = style.gradient ?: SolidColor(style.backgroundColor),
//                shape = shape
//            )
//            .border(
//                width = if (style.borderColor != Color.Transparent) 1.dp else 0.dp,
//                color = style.borderColor,
//                shape = shape
//            )
//            .clickable(onClick = onClick)
//    ) {
//        Text(
//            text = text,
//            color = style.contentColor,
//            style = MaterialTheme.typography.labelSmall,
//            maxLines = 1, // Ensure single line
//            overflow = TextOverflow.Ellipsis, // Show ellipsis if text overflows
//            modifier = Modifier
//                .align(Alignment.Center)
//                .padding(12.dp)
//        )
//    }


sealed class ButtonStyle(
    val gradient: Brush? = null,
    val backgroundColor: Color = Color.Transparent,
    val contentColor: Color,
    val borderColor: Color = Color.Transparent,
    val cornerRadius: Dp
) {
    object Primary : ButtonStyle(
        gradient = Brush.linearGradient(listOf(Color(0xFF023047), Color(0xFF219DBB))),
        contentColor = Color.White,
        cornerRadius = 8.dp
    )

    object Secondary : ButtonStyle(
        gradient = Brush.linearGradient(listOf(AppColors.Orange, AppColors.Yellow)),
        contentColor = Color.White,
        cornerRadius = 8.dp
    )

    object Tertiary : ButtonStyle(
        gradient = Brush.linearGradient(listOf(Color(0xFF00FF00), Color(0xFF008000))),
        contentColor = Color.White,
        cornerRadius = 8.dp
    )

    // New solid white background style with colored border and text
    class Outline(val color: Color = Color(0xFF023047)) : ButtonStyle(
        gradient = null,
        backgroundColor = Color.White,
        contentColor = color,
        borderColor = color,
        cornerRadius = 10.dp
    )
}

@Preview(showBackground = true)
@Composable
fun SecondaryButtonPreview() {
    Row (
        modifier = Modifier.fillMaxWidth(),
        horizontalArrangement = Arrangement.SpaceBetween,
        verticalAlignment = Alignment.CenterVertically
    ) {
        SecondaryButton(
            text = "Option one",
            onClick = {},
            modifier = Modifier.weight(1f)

        )

        Spacer(Modifier.width(8.dp))
        SecondaryButton(
            text = "Otion two",
            onClick = {},
            modifier = Modifier.weight(1f),
        )
    }
    Spacer(Modifier.height(40.dp))
    Column(verticalArrangement = Arrangement.spacedBy(8.dp), modifier = Modifier.padding(16.dp)) {
        SecondaryButton(text = "Primary", onClick = {}, style = ButtonStyle.Primary)
        SecondaryButton(text = "Secondary", onClick = {}, style = ButtonStyle.Secondary)
        SecondaryButton(text = "Tertiary", onClick = {}, style = ButtonStyle.Tertiary)
        SecondaryButton(text = "Outline Blue", onClick = {}, style = ButtonStyle.Outline(Color(0xFF0072FF)))
        SecondaryButton(text = "Outline Red", onClick = {}, style = ButtonStyle.Outline(Color(0xFFFF0000)))
    }
}