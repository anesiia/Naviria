package com.example.dyplomproject.ui.components

import androidx.compose.foundation.layout.Box
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Notifications
import androidx.compose.material.icons.outlined.Notifications
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import com.example.dyplomproject.ui.viewmodel.NotificationViewModel

@Composable
fun NotificationBell(viewModel: NotificationViewModel, onClick: () -> Unit) {
    val hasUnread by viewModel.hasUnread

    Box {
        IconButton(onClick = onClick) {
            Icon(
                imageVector = if (hasUnread) Icons.Filled.Notifications else Icons.Outlined.Notifications,
                contentDescription = "Notifications"
            )
        }
    }
}