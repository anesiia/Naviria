package com.example.dyplomproject.ui.components.task

import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Check
import androidx.compose.material.icons.filled.Close
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.SolidColor
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.ui.theme.additionalTypography
import com.google.accompanist.flowlayout.FlowMainAxisAlignment
import com.google.accompanist.flowlayout.FlowRow
//val tags = remember { mutableStateListOf<String>() }
//val tags = remember { mutableStateListOf("Kotlin", "Kotlin","Kotliasdasdn","Kotlin","Kotdsdalin","Koddtlin","Kotlddin") }
@Composable
fun TagInputComponent(
    tags: List<String>,
    onTagsChange: (List<String>) -> Unit
) {
    var newTagText by remember { mutableStateOf("") }
    var editingTagIndex by remember { mutableStateOf(-1) }
    var editingTagText by remember { mutableStateOf("") }

    FlowRow(
        modifier = Modifier.fillMaxWidth(),
        mainAxisSpacing = 8.dp, crossAxisSpacing = 8.dp, mainAxisAlignment = FlowMainAxisAlignment.Start
    ) {
        tags.forEachIndexed { index, tag ->
            if (editingTagIndex == index) {
                TagModification(
                    text = editingTagText,
                    onTextChange = { editingTagText = it },
                    onSaveButtonClick = {
                        val updatedTags = tags.toMutableList()
                        updatedTags[index] = editingTagText
                        onTagsChange(updatedTags)
                        editingTagIndex = -1
                        editingTagText = ""
                    },
                    modifier = Modifier.width(IntrinsicSize.Min)
                )
            } else {
                TagDisplay(
                    text = tag,
                    onClick = {
                        editingTagIndex = index
                        editingTagText = tag
                    },
                    onDelete = {
                        val updatedTags = tags.toMutableList()
                        updatedTags.removeAt(index)
                        onTagsChange(updatedTags)
                    }
                )
            }
        }
        TagCreationTextField(
            text = newTagText,
            onTextChange = { newTagText = it },
            onClick = {
                if (newTagText.isNotBlank()) {
                    if (newTagText.isNotBlank()) {
                        onTagsChange(tags + newTagText.trim())
                        newTagText = ""
                    }
                }
            },
            modifier = Modifier.width(IntrinsicSize.Min)
        )
    }
}

@Composable
fun TagCreationTextField(
    text: String,
    onTextChange: (String) -> Unit,
    onClick: () -> Unit,
    modifier: Modifier
) {
    Surface(
        shape = RoundedCornerShape(10.dp),
        border = BorderStroke(1.dp, SolidColor(Color(0xFF063B52))),
        modifier = modifier,
        contentColor = Color(0xFF063B52),
        color = Color.Transparent
            //.height(40.dp)
            //.fillMaxWidth(0.8f) // Adjust width as needed
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            modifier = Modifier.padding(start = 8.dp)
        ) {
            Text(text = "#", style = additionalTypography.hashTagSign, color = Color(0xFF063B52))

            Spacer(modifier = Modifier.width(4.dp))
            Column (
                modifier = Modifier
                    .weight(1f)           // <-- Take up remaining horizontal space
                    .padding(vertical = 4.dp)
            )
            {
                BasicTextField(
                    value = text,
                    onValueChange = onTextChange,
                    textStyle = additionalTypography.hashTagText,
                    singleLine = true,
                    modifier = Modifier
                        .widthIn(min = 60.dp, max = 90.dp) // small but limited width
                        .padding(vertical = 4.dp)
                )
                HorizontalDivider(
                    //modifier = Modifier.fillMaxWidth(),
                    thickness = 1.dp,
                    color = Color(0xFF063B52)
                )
            }
            Spacer(modifier = Modifier.width(8.dp))
            IconButton(
                onClick = { onClick() },
                modifier = Modifier.size(48.dp)
            ) {
                Icon(
                    imageVector = Icons.Default.Add,
                    contentDescription = "Add"
                )
            }
        }
    }
}

@Composable
fun TagModification(
    text: String,
    onTextChange: (String) -> Unit,
    onSaveButtonClick: () -> Unit,
    modifier: Modifier
) {
    Surface(
        shape = RoundedCornerShape(10.dp),
        border = BorderStroke(1.dp, SolidColor(Color(0xFF063B52))),
        modifier = modifier,
        color = Color(0xFFF0F0F0),
        contentColor = Color(0xFF063B52)
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            modifier = Modifier.padding(start = 8.dp)
        ) {
            //Text(text = "#", style = additionalTypography.hashTagSign, color = Color(0xFF063B52))

            Spacer(modifier = Modifier.width(4.dp))
            Column (
                modifier = Modifier
                    .weight(1f)
                    .padding(vertical = 4.dp)
            )
            {
                BasicTextField(
                    value = text,
                    onValueChange = onTextChange,
                    textStyle = additionalTypography.hashTagText,
                    singleLine = true,
                    modifier = Modifier
                        .widthIn(min = 60.dp, max = 90.dp)
                        .padding(vertical = 4.dp)
                )
                HorizontalDivider(
                    thickness = 1.dp,
                    color = Color(0xFF063B52)
                )
            }
            Spacer(modifier = Modifier.width(8.dp))
            IconButton(
                onClick = onSaveButtonClick,
                modifier = Modifier.size(48.dp)
            ) {
                Icon(Icons.Default.Check, contentDescription = "Зберегти")
            }
        }
    }
}

@Composable
fun TagDisplay(text: String, onClick: () -> Unit, onDelete: () -> Unit) {
    Surface(
        shape = RoundedCornerShape(10.dp),
        color = Color(0xFFF0F0F0),//MaterialTheme.colorScheme.primary.copy(alpha = 0.2f),
        contentColor = Color(0xFF063B52),
        modifier = Modifier
            .clickable { onClick() }
            //.padding(0.dp)
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            modifier = Modifier.padding(horizontal = 8.dp, vertical = 10.dp)
        ) {
            IconButton(
                onClick = onDelete,
                modifier = Modifier.size(28.dp)
            ) {
                Icon(Icons.Default.Close, contentDescription = "Видалити", modifier = Modifier.size(16.dp))
            }
            Spacer(modifier = Modifier.width(16.dp))
            Text("#", style = additionalTypography.hashTagText)
            Spacer(modifier = Modifier.width(2.dp))
            Text(text, style = additionalTypography.hashTagText)
        }
    }
}

@Preview(showBackground = true)
@Composable
fun TagInputComponentPreview() {
    MaterialTheme {
        Surface(modifier = Modifier.fillMaxSize()) {
            //TagInputComponent()
        }
    }
}