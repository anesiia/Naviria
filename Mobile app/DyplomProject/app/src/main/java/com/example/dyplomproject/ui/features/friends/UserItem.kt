package com.example.dyplomproject.ui.features.friends
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.AsyncImage
import com.example.dyplomproject.ui.components.ButtonStyle
import com.example.dyplomproject.ui.components.SecondaryButton
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel

@Composable
fun UserItem(
    user: UserShortUiModel,
    onAddFriendClick: (UserShortUiModel) -> Unit,
    isAlreadyFriend: Boolean
) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(8.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.SpaceBetween
    ) {
        AsyncImage(
            model = user.photo,
            contentDescription = null,
            modifier = Modifier.size(40.dp).clip(CircleShape)
        )
//        Box(
//            modifier = Modifier
//                .size(40.dp)
//                .clip(CircleShape)
//                //.background(if (user.isOnline) Color.Green else Color.Gray) -
//                // CURRENTLY THE SERVER DON'T HANDLE THIS STATE(STATE OF USERS THAT ARE ONLINE)
//                // So basically All users online
//                .background(Color(0xFF005580))
//        )
        Column(
            modifier = Modifier
                .weight(1f)
                .padding(start = 8.dp)
        ) {
            Text(text = user.fullName, style = MaterialTheme.typography.labelLarge)
            Text(text = "@${user.nickname}", color = Color.Gray)
        }
        if (isAlreadyFriend) {
            Text("Вже у друзях", color = Color.Gray)
        } else {
            SecondaryButton(
                text = if (user.isRequestSent) "Надіслано" else "Надіслати",
                onClick = { onAddFriendClick(user) },
                style = if (user.isRequestSent) ButtonStyle.Outline( AppColors.Orange) else ButtonStyle.Secondary,
                modifier = Modifier.weight(0.5f)
            )
        }
    }
}


@Preview(showBackground = true)
@Composable
fun PreviewUserItem() {
    Column (modifier = Modifier
        .fillMaxWidth()
        .background(AppColors.Gray)
    ) {
//        Button(
//            onClick = {  }, // Navigate to the new composable
//            modifier = Modifier
//                .align(Alignment.CenterHorizontally)
//                .padding(vertical = 16.dp),
//            shape = RoundedCornerShape(10.dp),
//            colors = ButtonDefaults.buttonColors(
//                /*backgroundColor = if (uiState.newFriendRequest) Color.Red else Color.Blue*/ // Change color based on new request
//            )
//        ) {
//            Text(
//                /*text = if (uiState.newFriendRequest) "New Friend Request" else*/ "Add Friend",
//                color = Color.White
//            )
//        }
        UserItem(
            user = UserShortUiModel(
                id = "",
                nickname = "john_doe",
                fullName = "John Doe",
                isOnline = true,
                isProUser = false,
                isRequestSent = false,
                photo = ""
            ),
            onAddFriendClick = {},
            false
        )
    }
}