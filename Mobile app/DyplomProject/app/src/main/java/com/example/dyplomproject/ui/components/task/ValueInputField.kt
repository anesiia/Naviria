package com.example.dyplomproject.ui.components.task

import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.IconButton
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.SolidColor
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.memory.MemoryCache
import com.example.dyplomproject.R
import com.example.dyplomproject.ui.components.SecondaryButton
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.TasksViewModel

@Composable
fun ValueInput(valueText: String,
               onValueTextChange: (String) -> Unit,
               onValueIncrease: () -> Unit,
               onValueDescrease: () -> Unit,
               onClick: () -> Unit
) {//viewModel: TasksViewModel) {//OnClick: () -> Unit) {
    //var valueText by remember { mutableStateOf("") }
    val isValueValid = valueText.toIntOrNull() != null

    Row(
        verticalAlignment = Alignment.CenterVertically,
        modifier = Modifier
            .padding(16.dp)
            .fillMaxWidth()
    ) {
//        TextField(
//            value = valueText,
//            onValueChange = { valueText = it },
//            label = { Text("Enter value") },
//            modifier = Modifier.weight(1f),
//            keyboardOptions = KeyboardOptions.Default.copy(keyboardType = KeyboardType.Number)
//        )
//
//        Column {
//            IconButton(onClick = {
//                val current = valueText.toIntOrNull() ?: 0
//                valueText = (current + 1).toString()
//            }) {
//                // Replace with your up arrow image
//                Image(
//                    painter = painterResource(id = R.drawable.ic_arrow_up), // <-- You need to add this to res/drawable
//                    contentDescription = "Increment"
//                )
//            }
//
//            IconButton(onClick = {
//                val current = valueText.toIntOrNull() ?: 0
//                val newValue = if (current > 0) current - 1 else 0
//                valueText = newValue.toString()
//            }) {
//                Image(
//                    painter = painterResource(id = R.drawable.ic_arrow_down), // <-- You need to add this to res/drawable
//                    contentDescription = "Increment"
//                )
//            }
//        }




//        trying to make it work
//        ValueInputField(
//            valueText = valueText,
//            onValueTextChange = { valueText = it },
//            onValueIncrease = {
//                val current = valueText.toIntOrNull() ?: 0
//                valueText = (current + 1).toString()
//            },
//            onValueDescrease = {
//                val current = valueText.toIntOrNull() ?: 0
//                val newValue = if (current > 0) current - 1 else 0
//                valueText = newValue.toString()
//            },
//            modifier = Modifier.size(150.dp, 50.dp)
//        )
        ValueInputField(
            valueText = valueText,
            onValueTextChange = onValueTextChange,
            onValueIncrease = onValueIncrease,
            onValueDescrease = onValueDescrease,
            modifier = Modifier.size(150.dp, 50.dp)
        )

        Spacer(modifier = Modifier.width(32.dp))

        SecondaryButton(
            text = "Додати",
            onClick = onClick,//OnClick,
            enabled = isValueValid
        )
    }
}


@Composable
fun ValueInputField(
    valueText: String,
    onValueTextChange: (String) -> Unit,
    onValueIncrease: () -> Unit,
    onValueDescrease: () -> Unit,
    modifier: Modifier
) {
    //var valueText by remember { mutableStateOf("") }
    //val isValueValid = valueText.toIntOrNull() != null
    Surface(
        shape = RoundedCornerShape(10.dp),
        border = BorderStroke(1.dp, SolidColor(Color(0xFF063B52))),
        modifier = modifier,
//            .padding(16.dp)
//            .fillMaxWidth(),
        contentColor = Color(0xFF063B52),
        color = Color.Transparent
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            modifier = Modifier.padding(start = 8.dp)
        ) {
            Spacer(modifier = Modifier.width(4.dp))
            Column(
                modifier = Modifier
                    .weight(1f)           // <-- Take up remaining horizontal space
                    .padding(vertical = 4.dp)
            )
            {
                BasicTextField(
                    value = valueText,
                    onValueChange = onValueTextChange,
                    textStyle = additionalTypography.hashTagText,
                    singleLine = true,
                    modifier = Modifier
                        .widthIn(min = 60.dp, max = 90.dp) // small but limited width
                        .padding(vertical = 4.dp)
                    //keyboardOptions = KeyboardOptions()
                )
                HorizontalDivider(
                    thickness = 1.dp,
                    color = Color(0xFF063B52)
                )
            }
            Spacer(modifier = Modifier.width(8.dp))
            Column(
                modifier = Modifier.wrapContentSize(),
                verticalArrangement = Arrangement.spacedBy(0.dp) 
            ){
                IconButton(
                    onClick = onValueIncrease,
                    modifier = Modifier.size(24.dp))
                {
                    Icon(
                        painter = painterResource(id = R.drawable.ic_arrow_up),
                        contentDescription = "Increment"
                    )
                }
                IconButton(
                    onClick = onValueDescrease,
                    modifier = Modifier.size(24.dp)
                ) {
                    Icon(
                        painter = painterResource(id = R.drawable.ic_arrow_down), // <-- You need to add this to res/drawable
                        contentDescription = "Increment"
                    )
                }
            }
        }
        Spacer(modifier = Modifier.width(8.dp))
    }
}

@Preview(showBackground = true)
@Composable
fun ValueInputFieldPreview() {

    MaterialTheme {
        Column {
            ValueInput("", {}, {}, {}, {})
            //ValueInputField()
        }
    }
}