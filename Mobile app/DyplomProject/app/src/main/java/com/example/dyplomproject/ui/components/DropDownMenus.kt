package com.example.dyplomproject.ui.components

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.Icon
import androidx.compose.material3.MenuDefaults
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.R
import com.example.dyplomproject.data.remote.Category
import com.example.dyplomproject.ui.components.task.create.Priority
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography

@Composable
fun DropdownMenuCategory(
    options: List<Category>,
    selected: Category,
    onSelected: (Category) -> Unit
) {
    var expanded by remember { mutableStateOf(false) }

    Box {
        TextButton(
            onClick = { expanded = true },
            modifier = Modifier
                //.padding(8.dp)
                .background(AppColors.Gray, RoundedCornerShape(8.dp))
                .padding(0.dp)
        ) {
            Text(selected.name, style = additionalTypography.exampleText)
            Spacer(Modifier.width(8.dp))
            Icon(painterResource(id = R.drawable.ic_arrow_drop_down) , contentDescription = null)
        }

        DropdownMenu(expanded = expanded, onDismissRequest = { expanded = false }) {
            options.forEach { option ->
                DropdownMenuItem(
                    text = { Text(option.name) },
                    onClick = {
                        onSelected(option)
                        expanded = false
                    },
//                    colors = MenuDefaults.itemColors(
//                    )
                )
            }
        }
    }
}

@Composable
fun DropdownMenuFiltrationCategory(
    options: List<Category>,
    selected: Category,
    onSelected: (Category) -> Unit
) {
    var expanded by remember { mutableStateOf(false) }

    Box {
        TextButton(
            onClick = { expanded = true },
            modifier = Modifier
                //.padding(8.dp)
                .background(Color.Transparent, RoundedCornerShape(8.dp))
                .padding(0.dp)
        ) {
            Text(selected.name, style = additionalTypography.mediumTextUnderlined, color = AppColors.Orange)
            Spacer(Modifier.width(8.dp))
            Icon(painterResource(id = R.drawable.ic_arrow_drop_down) , contentDescription = null)
        }

        DropdownMenu(expanded = expanded, onDismissRequest = { expanded = false }) {
            options.forEach { option ->
                DropdownMenuItem(
                    text = { Text(option.name) },
                    onClick = {
                        onSelected(option)
                        expanded = false
                    },
                    colors = MenuDefaults.itemColors(
                        textColor = AppColors.Orange,
                        leadingIconColor = AppColors.Orange,
                        trailingIconColor = AppColors.Orange,
                        disabledTextColor = AppColors.Orange
                    )
                )
            }
        }
    }
}

@Composable
fun DropdownMenuPriority(
    options: List<Priority>,
    selected: Priority,
    onSelected: (Priority) -> Unit
) {
    var expanded by remember { mutableStateOf(false) }

    Box {
        TextButton(
            onClick = { expanded = true },
            modifier = Modifier
                //.padding(8.dp)
                .background(AppColors.Gray, RoundedCornerShape(8.dp))
                .padding(0.dp)
        ) {
            Text(selected.name, style = additionalTypography.exampleText)
            Spacer(Modifier.width(8.dp))
            Icon(painterResource(id = R.drawable.ic_arrow_drop_down) , contentDescription = null)
        }

        DropdownMenu(expanded = expanded, onDismissRequest = { expanded = false }) {
            options.forEach { option ->
                DropdownMenuItem(text = { Text(option.name) }, onClick = {
                    onSelected(option)
                    expanded = false
                })
            }
        }
    }
}

@Composable
fun DropdownMenuWithOptions(
    options: List<String>,
    selected: String,
    onSelected: (String) -> Unit
) {
    var expanded by remember { mutableStateOf(false) }

    Box {
        TextButton(onClick = { expanded = true }) {
            Text(selected)
            Icon(painterResource(id = R.drawable.ic_arrow_drop_down) , contentDescription = null)
        }

        DropdownMenu(expanded = expanded, onDismissRequest = { expanded = false }) {
            options.forEach { option ->
                DropdownMenuItem(text = { Text(option) }, onClick = {
                    onSelected(option)
                    expanded = false
                })
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun DropdownMenuCategoryFriendsScreenPreview() {
    var selectedCategory by remember { mutableStateOf(Category("", "Категорія")) }
    DropdownMenuFiltrationCategory(
        listOf(Category("", "Категорія")),
        selectedCategory,
        { selectedCategory = it }
    )
}