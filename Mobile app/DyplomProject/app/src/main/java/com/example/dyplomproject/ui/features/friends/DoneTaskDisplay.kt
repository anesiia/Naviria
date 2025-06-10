package com.example.dyplomproject.ui.features.friends

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Checkbox
import androidx.compose.material3.CheckboxDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextDecoration
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.data.remote.dto.TaskDto
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.viewmodel.TasksViewModel

@Composable
fun DoneTaskDisplay(task: TaskDto, viewModel: TasksViewModel) {
    //before modifaction
//        Column(
//            modifier = Modifier
//                .fillMaxWidth()
//                .padding(8.dp)
//                .background(AppColors.Gray, RoundedCornerShape(8.dp))
//                .border(0.dp, AppColors.Orange, RoundedCornerShape(12.dp))
//                .padding(8.dp)
//            //.padding(end = 8.dp)
//        )
//        val borderModifier = if (taskInfoVisible) {
//            Modifier.border(1.dp, AppColors.Orange, RoundedCornerShape(8.dp))
//        } else {
//            Modifier.border(0.dp, Color.Companion.Transparent, RoundedCornerShape(8.dp))
//        }
    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(8.dp)
            .background(AppColors.White, RoundedCornerShape(8.dp))//, RoundedCornerShape(8.dp))
            .padding(8.dp)
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth(),
            verticalAlignment = Alignment.CenterVertically
        ) {
            Checkbox(
                checked = true,
                onCheckedChange = { },
                modifier = Modifier
                    .size(24.dp)
                    .align(Alignment.CenterVertically),
                colors = CheckboxDefaults.colors(
                    checkedColor = AppColors.Orange,
                    //uncheckedColor = AppColors.Orange,
                    checkmarkColor = AppColors.White
                )
            )
            Text(
                text = task.title,
                style = MaterialTheme.typography.bodyLarge,
                maxLines = 2,
                textDecoration = TextDecoration.LineThrough,
                overflow = TextOverflow.Ellipsis,
                modifier = Modifier
                    .weight(1f)
                    .padding(start = 8.dp, end = 8.dp)
            )
        }
    }
}