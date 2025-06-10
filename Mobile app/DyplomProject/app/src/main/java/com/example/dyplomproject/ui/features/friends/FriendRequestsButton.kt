package com.example.dyplomproject.ui.features.friends

import androidx.compose.foundation.layout.Box
import androidx.compose.material3.Button
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.ui.Alignment
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import com.example.dyplomproject.ui.theme.AppColors

@Composable
fun FriendRequestsButton(
    hasNewRequests: Boolean,
    onClick: () -> Unit,
    modifier: Modifier
) {
    Box(
        contentAlignment = Alignment.TopEnd // Align the red dot at the top end
    ) {
        Button(
            onClick = onClick,
            modifier = modifier
                .padding(vertical = 16.dp),
            shape = RoundedCornerShape(10.dp),
            colors = ButtonDefaults.buttonColors(
                containerColor = Color(0x142099B7),
                disabledContentColor = Color(0x142099B7)
            )
        ) {
            Text(
                text = "Нові запити",
                color = Color(0xFF003344),
                style = MaterialTheme.typography.labelLarge
            )
        }

        // Red dot badge
        if (hasNewRequests) {
            Box(
                modifier = Modifier
                    .size(10.dp)
                    .offset(x = (-4).dp, y = 14.dp) // Adjust the offset for proper positioning
                    .background(AppColors.DarkBlue, shape = CircleShape)
            )
        }
    }
}

@Preview(showBackground = true)
@Composable
fun FriendRequestsButtonPreview() {
    Column (modifier = Modifier.fillMaxSize()){
        FriendRequestsButton(
            true,
            {},
            modifier = Modifier
                .fillMaxWidth()
                .wrapContentWidth(Alignment.End)
        )
    }
}
