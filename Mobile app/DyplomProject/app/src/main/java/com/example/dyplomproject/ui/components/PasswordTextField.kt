package com.example.dyplomproject.ui.components
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Visibility
import androidx.compose.material.icons.filled.VisibilityOff
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.tooling.preview.Preview

@Composable
fun PasswordTextField(
    value: String,
    onValueChange: (String) -> Unit,
    isPasswordVisible: Boolean,
    onVisibilityChange: () -> Unit
) {
    OutlinedTextField(
        value = value,
        onValueChange = onValueChange,
        label = { Text("Пароль") },
        singleLine = true,
        visualTransformation = if (isPasswordVisible) VisualTransformation.None else PasswordVisualTransformation(),
        trailingIcon = {
            val image = if (isPasswordVisible)
                Icons.Filled.Visibility
            else Icons.Filled.VisibilityOff

            IconButton(onClick = onVisibilityChange) {
                Icon(imageVector = image, contentDescription = "Toggle Password Visibility")
            }
        },
//        colors = TextFieldDefaults.outlinedTextFieldColors(
//            focusedBorderColor = Color.Blue,
//            unfocusedBorderColor = Color.Gray,
//            cursorColor = Color.Red,
//            focusedLabelColor = Color.Magenta
//        )
    )

}
@Preview(showBackground = true)
@Composable
fun PasswordTextFieldPreview() {
    Surface() {
        PasswordTextField(
            value = "",
            onValueChange = {},
            isPasswordVisible = true,
            onVisibilityChange = {}
        )
    }
}