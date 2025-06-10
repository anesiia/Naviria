package com.example.dyplomproject.ui.screen

import android.widget.Toast
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import com.example.dyplomproject.R
import com.example.dyplomproject.data.remote.repository.UserRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.UnderlinedText
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.viewmodel.FriendRequestsViewModel
import com.example.dyplomproject.ui.viewmodel.IncomingRequestUiModel
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel

@Composable
fun FriendRequestsScreen(
    navController: NavHostController,
    userId: String,
    padding: PaddingValues
) {
    val viewModel: FriendRequestsViewModel =
        viewModel(factory = object : ViewModelProvider.Factory {
            override fun <T : ViewModel> create(modelClass: Class<T>): T {
                return FriendRequestsViewModel(UserRepository(RetrofitInstance.api), userId) as T
            }
        })
    val requests by viewModel.requests.collectAsState()
    val message by viewModel.messageFlow.collectAsState(initial = "")
    val context = LocalContext.current
    LaunchedEffect(Unit) {
        viewModel.loadRequests()
        if (message.isNotEmpty()) {
            Toast.makeText(context, message, Toast.LENGTH_SHORT).show()
        }
    }
    LaunchedEffect(message) {
        if (message.isNotEmpty()) {
            Toast.makeText(context, message, Toast.LENGTH_SHORT).show()
        }
    }
    Column(
        modifier = Modifier
            .fillMaxSize()
            .background(AppColors.White)
            .padding(padding)
            .padding(horizontal = 24.dp),
    ) {
        Spacer(Modifier.height(16.dp))
        UnderlinedText(
            text = "Мої запити у друзі",
            useGradient = true
        )
        Column(
            modifier = Modifier.weight(1f)
        ) {
            if (requests.isEmpty()) {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    Text(
                        text = "Немає запитів у друзі,\nне хвилюйся, вони ще попереду!",
                        style = MaterialTheme.typography.bodyLarge,
                        //color = Color.Gray
                    )
                }
            } else {
                LazyColumn {
                    items(requests) { request ->
                        RequestItem(
                            request,
                            onApproveButtonClick = { viewModel.acceptFriendRequest(request.requestId) },
                            onDeclineButtonClick = { viewModel.declineFriendRequest(request.requestId) }
                        )
                    }
                }
            }
        }
//        Column(
//            modifier = Modifier
//                .weight(1f)
//                //.padding(padding)
//        ) {
////            LazyColumn {
////                items(requests) { request ->
////                    RequestItem(
////                        request,
////                        onApproveButtonClick = { /* Handle approve */ },
////                        onDeclineButtonClick = { viewModel.declineFriendRequest(request.requestId) }
////                    )
////                }
////            }
////            Spacer(modifier = Modifier.weight(1f))
//        }
        Row(
            modifier = Modifier
                .padding(vertical = 24.dp)
                .clickable { navController.popBackStack() }
        ) {
            IconButton(onClick = { navController.popBackStack()/*navController.navigate("friends")*/ }) {
                Icon(
                    painter = painterResource(id = R.drawable.ic_arrow_back), // Replace with your icon resource
                    contentDescription = "Стрілка назад"
                )
            }

            Spacer(Modifier.width(16.dp))
            Text("Назад", modifier = Modifier.align(Alignment.CenterVertically))
        }
    }
}

@Composable
fun RequestItem(
    request: IncomingRequestUiModel,
    onApproveButtonClick: () -> Unit,
    onDeclineButtonClick: (String) -> Unit
) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(8.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.SpaceBetween
    ) {

        Box(
            modifier = Modifier
                .size(40.dp)
                .clip(CircleShape)
                .background(Color.LightGray)
        )
        Text(
            text = request.sender.nickname,//user.nickname,
            fontWeight = FontWeight.Bold,
            modifier = Modifier
                .weight(1f)
                .padding(start = 8.dp),
            maxLines = 1
        )
        IconButton(onClick = { onDeclineButtonClick(request.requestId) }) {
            Image(
                painter = painterResource(id = R.drawable.ic_decline_request), // replace with your drawable name
                contentDescription = "Видалити",
                modifier = Modifier.size(35.dp)
            )
        }
        IconButton(onClick = { onApproveButtonClick() }) {
            Image(
                painter = painterResource(id = R.drawable.ic_approve_request), // replace with your drawable name
                contentDescription = "Додати",
                modifier = Modifier.size(35.dp)
            )
        }
//        SecondaryButton(
//            text = if (!user.isRequestSent) "Підтримати" else "Підтримано",
//            onClick = {  },
//            style = if (!user.isRequestSent) ButtonStyle.Secondary else ButtonStyle.Outline(Color(0xFFFF4500)),
//            modifier = Modifier.weight(0.5f)
//        )
    }
}

@Preview(showBackground = true)
@Composable
fun RequestItemPreview() {
    val request = IncomingRequestUiModel(
        requestId = "",
        fromUserId = "",
        toUserId = "",
        status = "",
        sender = UserShortUiModel(
            id = "",
            nickname = "john_doe",
            fullName = "John Doe",
            isOnline = true,
            isProUser = false,
            isRequestSent = false,
            photo = ""
        )
    )
    Column {
        RequestItem(
            request,
            {},
            {})
    }
}


//    Column(modifier = Modifier.fillMaxSize().padding(padding)) {
//        Text("Запити у друзі")
//        LazyColumn {
//            items(requests) { user ->
//                Text(user.fullName) // Replace with FriendRequestItem()
//            }
//        }
//
//        IconButton(onClick = { navController.popBackStack() }) {
//            Icon(painter = painterResource(R.drawable.ic_arrow_back), contentDescription = "Назад")
//        }
//    }
//}

//@Preview
//@Composable
//fun FriendRequestsScreenPreview() {
//    //FriendRequestsScreen()
//}