package com.example.dyplomproject.ui.features.friends

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.defaultMinSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.wrapContentWidth
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Close
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.AsyncImage
import com.example.dyplomproject.R
import com.example.dyplomproject.ui.components.ButtonStyle
import com.example.dyplomproject.ui.components.SecondaryButton
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel

@Composable
fun FriendItem(
    user: UserShortUiModel,
    onRemoveClick: (UserShortUiModel) -> Unit,
    onSendSupportRequest: () -> Unit
) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(8.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.SpaceBetween
    ) {
        IconButton(onClick = { onRemoveClick(user) }) {
            Icon(
                imageVector = Icons.Default.Close,
                contentDescription = "Удалить",
                tint = Color(0xFF003344),
                modifier = Modifier.size(24.dp)
            )
        }
        Box(
            modifier = Modifier
                .size(40.dp)
                .clip(CircleShape)
                .background(Color.LightGray)
        )
        Text(
            text = user.nickname,
            style = MaterialTheme.typography.labelLarge,
            modifier = Modifier
                .weight(1f)
                .padding(start = 8.dp),
            maxLines = 1
        )
        SecondaryButton(
            text = if (!user.isRequestSent) "Підтримати" else "Підтримано",
            onClick = { onSendSupportRequest() },
            style = if (!user.isRequestSent) ButtonStyle.Secondary else ButtonStyle.Outline(AppColors.Orange),
            modifier = Modifier.defaultMinSize(minWidth = 96.dp)
        )
    }
}

@Preview(showBackground = true)
@Composable
fun PreviewFriendItem() {
    FriendItem(
        user = UserShortUiModel(
            id = "",
            nickname = "john_doe",
            fullName = "John Doe",
            isOnline = true,
            isProUser = false,
            isRequestSent = false,
            photo = ""
        ),
        onRemoveClick = {},
        onSendSupportRequest = {}
    )
}