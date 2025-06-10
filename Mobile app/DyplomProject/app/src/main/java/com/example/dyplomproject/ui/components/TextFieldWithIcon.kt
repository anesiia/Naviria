package com.example.dyplomproject.ui.components

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Search
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography

@Composable
fun RoundedTextFieldWithIcon(
    value: String,
    onValueChange: (String) -> Unit,
    placeholder: String = "Enter text",
    icon: ImageVector = Icons.Default.Search,
    onIconClick: () -> Unit = {}
) {
    TextField(
        value = value,
        onValueChange = onValueChange,
        placeholder = { Text(placeholder, style = additionalTypography.lightText) },
        modifier = Modifier
            .fillMaxWidth()
            .padding(16.dp)
            .shadow(
                elevation = 8.dp,
                shape = RoundedCornerShape(20.dp),
                ambientColor = Color.Black.copy(alpha = 0.25f),
                spotColor = Color.Black.copy(alpha = 1f)
            ),
        shape = RoundedCornerShape(20.dp), // Eagle style
        trailingIcon = {
            IconButton(onClick = onIconClick) {
                Icon(imageVector = icon, contentDescription = "Trailing icon")
            }
        },
        colors = TextFieldDefaults.textFieldColors(
            backgroundColor = AppColors.White,
            focusedIndicatorColor = Color.Transparent,
            unfocusedIndicatorColor = Color.Transparent,
            textColor = Color.Black
        ),
        textStyle = additionalTypography.semiboldText
    )
}

@Preview(showBackground = true)
@Composable
fun RoundedTextFieldWithIconPreview() {
    var text by remember { mutableStateOf("") }

    RoundedTextFieldWithIcon(
        value = text,
        onValueChange = { text = it },
        onIconClick = { text = "" }
    )
}