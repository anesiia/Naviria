package com.example.dyplomproject.ui.components

import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.ui.theme.additionalTypography

@Composable
fun LabeledTextField(
    label: String,
    value: String,
    onValueChange: (String) -> Unit,
    placeholder: String = "",
    isPassword: Boolean = false,
    fieldKey: String,
    fieldErrors: Map<String, String>
) {
    Text(
        color = Color(0xFF1B2B3A),
        text = label,
        style = MaterialTheme.typography.bodyLarge
    )
    Spacer(modifier = Modifier.height(4.dp))

    TextField(
        value = value,
        onValueChange = onValueChange,
        modifier = Modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(10.dp)),
        placeholder = { Text(placeholder, style = additionalTypography.exampleText) },
        colors = primaryTextFieldColors(),
        isError = fieldErrors.containsKey(fieldKey),
        visualTransformation = if (isPassword) PasswordVisualTransformation() else VisualTransformation.None,
        singleLine = true
    )

    fieldErrors[fieldKey]?.let { error ->
        Text(
            text = error,
            color = Color.Red,
            style = MaterialTheme.typography.bodyLarge
        )
    }

    Spacer(modifier = Modifier.height(16.dp))
}