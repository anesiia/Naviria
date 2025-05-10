package com.example.dyplomproject.ui.screen

import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.background
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
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Close
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import com.example.dyplomproject.data.utils.RetrofitInstance
import com.example.dyplomproject.data.remote.UserRepository
import com.example.dyplomproject.ui.components.ButtonStyle
import com.example.dyplomproject.ui.components.SecondaryButton
import com.example.dyplomproject.ui.viewmodel.FriendsViewModel
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel

@Composable
fun FriendsScreen(
    navController: NavHostController, userId: String, padding: PaddingValues
) {
    val repository = remember { UserRepository(RetrofitInstance.api) }
    var searchInput by remember { mutableStateOf("") }
    val viewModel: FriendsViewModel = viewModel(factory = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(modelClass: Class<T>): T {
            if (modelClass.isAssignableFrom(FriendsViewModel::class.java)) {
                return FriendsViewModel(repository, userId) as T
            }
            throw IllegalArgumentException("Unknown ViewModel class")
        }
    })

//    val selectedTab = viewModel.selectedTab
//    val isLoading = viewModel.isLoading
//    val friends = viewModel.friends
//    val allUsers = viewModel.allUsers
    val uiState by viewModel.uiState.collectAsState()
    LaunchedEffect(Unit) {
        viewModel.onTabSelected(uiState.selectedTab) // Загрузка при первом старте
        viewModel.checkNewFriendRequests(userId)
    }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .background(Color(0xFFFFFFFF))
            .padding(padding)
            .padding(horizontal = 8.dp),
    ) {
        Spacer(modifier = Modifier.height(32.dp))

        Row(horizontalArrangement = Arrangement.SpaceBetween) {
            SecondaryButton(
                text = "Друзі",
                onClick = { viewModel.onTabSelected(FriendsViewModel.TabType.FRIENDS) },
                style = if (uiState.selectedTab == FriendsViewModel.TabType.FRIENDS) ButtonStyle.Primary else ButtonStyle.Outline(),
                modifier = Modifier.weight(1f)
            )

            Spacer(Modifier.width(24.dp))

            SecondaryButton(
                text = "Всі користувачі",
                onClick = { viewModel.onTabSelected(FriendsViewModel.TabType.ALL_USERS) },
                style = if (uiState.selectedTab == FriendsViewModel.TabType.ALL_USERS) ButtonStyle.Primary else ButtonStyle.Outline(),
                modifier = Modifier.weight(1f)
            )
        }
        Spacer(modifier = Modifier.height(32.dp))
        ////////search field
        Row(modifier = Modifier.fillMaxWidth()) {
            OutlinedTextField(
                value = searchInput,
                onValueChange = {
                    searchInput = it
                    viewModel.onSearchQueryChanged(it)
                },
                label = { Text("Пошук") },
                modifier = Modifier.weight(1f)
            )

            Spacer(modifier = Modifier.width(8.dp))

            Button(onClick = { viewModel.searchUsers() }) {
                Text("Search")
            }

            Spacer(modifier = Modifier.width(8.dp))

            Button(onClick = {
                searchInput = ""
                viewModel.resetSearch()
            }) {
                Text("Reset")
            }
        }
        ////////
        Column(modifier = Modifier.weight(1f)) {
            if (uiState.isLoading) {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    CircularProgressIndicator()
                }
            } else {
//                val listToShow = when (uiState.selectedTab) {
//                    FriendsViewModel.TabType.FRIENDS -> uiState.friends
//                    FriendsViewModel.TabType.ALL_USERS -> uiState.allUsers
//                }
                val baseList = when (uiState.selectedTab) {
                    FriendsViewModel.TabType.FRIENDS -> uiState.friends
                    FriendsViewModel.TabType.ALL_USERS -> uiState.allUsers
                }
                val listToShow = uiState.searchResults ?: baseList
                if (uiState.searchResults != null && listToShow.isEmpty()) {
                    Text("Не знайдено жодного користувача з поданим ніком. Той во...забув..буває", style = MaterialTheme.typography.bodyMedium)
                } else if (listToShow.isEmpty()) {
                    Text("Потрібен час, щоб знайти однодумців, тож поки тут пусто", style = MaterialTheme.typography.bodyMedium)
                } else {
                    LazyColumn {
                        items(listToShow, key = { it.id }) { user ->
                            when (uiState.selectedTab) {
                                FriendsViewModel.TabType.FRIENDS -> {
                                    FriendItem(user = user, onRemoveClick = { /* Handle remove */ }, onSendSupportRequest = {  })
                                }
                                FriendsViewModel.TabType.ALL_USERS -> {
                                    UserItem(user = user, onAddFriendClick = { viewModel.onAddFriend(user) })
                                }
                            }
                        }
                    }
                }

            }
        }

//        Spacer(modifier = Modifier.weight(1f)) // This will push the button to the bottom
        Button(
            onClick = { navController.navigate("friend_requests") }, // Navigate to the new composable
            modifier = Modifier
                .align(Alignment.End)
                .padding(vertical = 16.dp),
            shape = RoundedCornerShape(10.dp),
            colors = ButtonDefaults.buttonColors(
                containerColor = if (uiState.hasNewFriendRequests) Color.Red else Color(0x142099B7),
                disabledContentColor = Color(0x142099B7)
            )
        ) {
            Text(
                /*text = if (uiState.newFriendRequest) "New Friend Request" else*/ text = "Нові запити",
                color = Color(0xFF003344),
                style = MaterialTheme.typography.labelLarge
            )
        }
//        THIS IS A CASE WITH INHERITENCE AND WHEN I CREATE ADDITIONAL DATA CLASS FOR FRIEND

//        if (uiState.isLoading) {
//            Box(
//                modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center
//            ) {
//                CircularProgressIndicator()
//            }
//        } else {
//            val listToShow = when (uiState.selectedTab) {
//                FriendsViewModel.TabType.FRIENDS -> uiState.friends // This is now List<FriendUiModel>
//                FriendsViewModel.TabType.ALL_USERS -> uiState.allUsers // This is still List<UserShortUiModel>
//            }
//
//            LazyColumn {
//                items(listToShow, key = { it.id }) { user ->
//                    when (uiState.selectedTab) {
//                        FriendsViewModel.TabType.FRIENDS -> {
//                            // Handle FriendUiModel
//                            if (user is FriendUiModel) {
//                                FriendItem(
//                                    user = user,
//                                    onRemoveClick = { /* Handle remove friend */ })
//                            }
//                        }
//
//                        FriendsViewModel.TabType.ALL_USERS -> {
//                            // Handle UserShortUiModel
//                            if (user is UserShortUiModel) {
//                                UserItem(
//                                    user = user,
//                                    onAddFriendClick = { /* Handle add friend */ })
//                            }
//                        }
//                    }
//                }
//            }
//        }
    }
}

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
            style = if (!user.isRequestSent) ButtonStyle.Secondary else ButtonStyle.Outline(Color(0xFFFF4500)),
            modifier = Modifier.weight(0.5f)
        )
    }
}

@Composable
fun UserItem(
    user: UserShortUiModel,
    onAddFriendClick: (UserShortUiModel) -> Unit
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
                //.background(if (user.isOnline) Color.Green else Color.Gray) -
                // CURRENTLY THE SERVER DON'T HANDLE THIS STATE(STATE OF USERS THAT ARE ONLINE)
                // So basically All users online
                .background(Color(0xFF005580))
        )
        Column(
            modifier = Modifier
                .weight(1f)
                .padding(start = 8.dp)
        ) {
            Text(text = user.fullName, style = MaterialTheme.typography.labelLarge)
            Text(text = "@${user.nickname}", color = Color.Gray)
        }
        SecondaryButton(
            text = if (user.isRequestSent == false) "Надіслати" else "Надіслано",
            onClick = { onAddFriendClick(user) },
            style = if (user.isRequestSent == false) ButtonStyle.Secondary else ButtonStyle.Outline(Color(0xFFFF4500)),
            modifier = Modifier.weight(0.5f)
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
            isRequestSent = false
        ),
        onRemoveClick = {},
        onSendSupportRequest = {}
    )
}

@Preview(showBackground = true)
@Composable
fun PreviewUserItem() {
//    UserItem(
//        user = UserShortUiModel(
//            id = "",
//            nickname = "john_doe",
//            fullName = "John Doe",
//            isOnline = true,
//            isProUser = false,
//            isRequestSent = false
//        ),
//        onAddFriendClick = {}
//    )
    Column (modifier = Modifier
        .fillMaxWidth()) {
        Button(
            onClick = {  }, // Navigate to the new composable
            modifier = Modifier
                .align(Alignment.CenterHorizontally)
                .padding(vertical = 16.dp),
            shape = RoundedCornerShape(10.dp),
            colors = ButtonDefaults.buttonColors(
                /*backgroundColor = if (uiState.newFriendRequest) Color.Red else Color.Blue*/ // Change color based on new request
            )
        ) {
            Text(
                /*text = if (uiState.newFriendRequest) "New Friend Request" else*/ "Add Friend",
                color = Color.White
            )
        }
    }
}