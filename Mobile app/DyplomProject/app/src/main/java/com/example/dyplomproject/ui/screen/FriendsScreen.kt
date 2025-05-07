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
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
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
import com.example.dyplomproject.ui.viewmodel.UserUiModel

@Composable
fun FriendsScreen(
    navController: NavHostController,
    userId: String,
    padding: PaddingValues
//    backgroundColor: Color = Color.Red,
//    text: String = "Friends screen",
//    textColor: Color = Color.White,
//    textSize: TextUnit = 24.sp
) {
    val repository = remember { UserRepository(RetrofitInstance.api) }

    val viewModel: FriendsViewModel = viewModel(
        factory = object : ViewModelProvider.Factory {
            @Suppress("UNCHECKED_CAST")
            override fun <T : ViewModel> create(modelClass: Class<T>): T {
                if (modelClass.isAssignableFrom(FriendsViewModel::class.java)) {
                    return FriendsViewModel(repository, userId) as T
                }
                throw IllegalArgumentException("Unknown ViewModel class")
            }
        }
    )

    val selectedTab = viewModel.selectedTab
    val isLoading = viewModel.isLoading
    val friends = viewModel.friends
    val allUsers = viewModel.allUsers

    LaunchedEffect(Unit) {
        viewModel.onTabSelected(selectedTab) // Загрузка при первом старте
    }

    Column(modifier = Modifier
        .fillMaxSize()
        .padding(padding)
        .padding(horizontal = 8.dp),
    ) {
        Spacer(modifier = Modifier.height(32.dp))

        Row(horizontalArrangement = Arrangement.SpaceBetween) {
//            Button(
//                onClick = { viewModel.onTabSelected(FriendsViewModel.TabType.FRIENDS) },
//                colors = ButtonDefaults.buttonColors(
//                    containerColor = if (selectedTab == FriendsViewModel.TabType.FRIENDS) Color.Blue else Color.Gray
//                )
//            ) {
//                Text("Друзі")
//            }
//
//            Button(
//                onClick = { viewModel.onTabSelected(FriendsViewModel.TabType.ALL_USERS) },
//                colors = ButtonDefaults.buttonColors(
//                    containerColor = if (selectedTab == FriendsViewModel.TabType.ALL_USERS) Color.Blue else Color.Gray
//                )
//            ) {
//                Text("Всі користувачі")
//            }
            SecondaryButton(
                text = "Друзі",
                onClick = { viewModel.onTabSelected(FriendsViewModel.TabType.FRIENDS) },
                style = if (selectedTab == FriendsViewModel.TabType.FRIENDS) ButtonStyle.Primary else ButtonStyle.Outline(),
                modifier = Modifier.weight(1f)
            )

            Spacer(Modifier.width(24.dp))

            SecondaryButton(
                text = "Всі користувачі",
                onClick = { viewModel.onTabSelected(FriendsViewModel.TabType.ALL_USERS) },
                style = if (selectedTab == FriendsViewModel.TabType.ALL_USERS) ButtonStyle.Primary else ButtonStyle.Outline(),
                modifier = Modifier.weight(1f)
            )
        }

        Spacer(modifier = Modifier.height(16.dp))

//        if (isLoading) {
//            CircularProgressIndicator()
//        } else {
//            val listToShow = when (selectedTab) {
//                FriendsViewModel.TabType.FRIENDS -> friends
//                FriendsViewModel.TabType.ALL_USERS -> allUsers
//            }
//
//            LazyColumn {
//                items(listToShow) { user ->
//                    Text(
//                        text = user.,
//                        modifier = Modifier
//                            .fillMaxWidth()
//                            .padding(8.dp)
//                    )
//                }
//            }
//        }
//        if (isLoading) {
//            CircularProgressIndicator()
//        } else {
//            when (selectedTab) {
//                FriendsViewModel.TabType.FRIENDS -> {
//                    LazyColumn {
//                        items(friends, key = { it.id }) { user ->
//                            FriendItem(
//                                user = user,
//                                onRemoveClick = { /* обработка удаления */ }
//                            )
//                        }
//                    }
//                }
//
//                FriendsViewModel.TabType.ALL_USERS -> {
//                    LazyColumn {
//                        items(allUsers, key = { it.id }) { user ->
//                            UserItem(
//                                user = user,
//                                onAddFriendClick = { /* обработка добавления */ }
//                            )
//                        }
//                    }
//                }
//            }
//        }
//        if (isLoading) {
//            CircularProgressIndicator()
//        } else {
//            when (selectedTab) {
//                FriendsViewModel.TabType.FRIENDS -> {
//                    LazyColumn {
//                        items(friends, key = { it.id }) { user ->
//                            FriendItem(user = user, onRemoveClick = { /* ... */ })
//                        }
//                    }
//                }
//                FriendsViewModel.TabType.ALL_USERS -> {
//                    LazyColumn {
//                        items(allUsers, key = { it.id }) { user ->
//                            UserItem(user = user, onAddFriendClick = { /* ... */ })
//                        }
//                    }
//                }
//            }
//        }
        if (isLoading) {
            Box(
                modifier = Modifier.fillMaxSize(),
                contentAlignment = Alignment.Center
            ) {
                CircularProgressIndicator()
            }
        } else {
            val listToShow = when (selectedTab) {
                FriendsViewModel.TabType.FRIENDS -> friends
                FriendsViewModel.TabType.ALL_USERS -> allUsers
            }

            LazyColumn {
                items(listToShow, key = { it.id }) { user ->
                    when (selectedTab) {
                        FriendsViewModel.TabType.FRIENDS -> {
                            FriendItem(user = user, onRemoveClick = { /* Handle remove */ })
                        }
                        FriendsViewModel.TabType.ALL_USERS -> {
                            UserItem(user = user, onAddFriendClick = { /* Handle add */ })
                        }
                    }
                }
            }
        }
    }
}

@Composable
fun FriendItem(
    user: UserUiModel,
    onRemoveClick: () -> Unit
) {
    var supported by remember { mutableStateOf(false) }

    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(8.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.SpaceBetween
    ) {
        // Удалить (иконка X)
        IconButton(onClick = onRemoveClick) {
            Icon(
                imageVector = Icons.Default.Close,
                contentDescription = "Удалить",
                tint = Color(0xFF003344),
                modifier = Modifier.size(24.dp)
            )
        }

        // Аватар
        Box(
            modifier = Modifier
                .size(40.dp)
                .clip(CircleShape)
                .background(Color.LightGray)
        )

        // Никнейм
        Text(
            text = user.nickname,
            fontWeight = FontWeight.Bold,
            modifier = Modifier
                .weight(1f)
                .padding(start = 8.dp),
            maxLines = 1
        )

        // Кнопка "Підтримати"
        val backgroundColor = if (supported) Color.White else Color(0xFFFF9800)
        val contentColor = if (supported) Color(0xFFFF9800) else Color.White
        val border = if (supported) BorderStroke(1.dp, Color(0xFFFF9800)) else null

        Button(
            onClick = { supported = !supported },
//            colors = ButtonDefaults.buttonColors(
//                backgroundColor = backgroundColor,
//                contentColor = contentColor
//            ),
            border = border,
            shape = RoundedCornerShape(12.dp),
            modifier = Modifier.height(36.dp)
        ) {
            Text("Підтримати")
        }
    }
}

@Composable
fun UserItem(
    user: UserUiModel,
    onAddFriendClick: () -> Unit
) {
    var added by remember { mutableStateOf(false) }

    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(8.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.SpaceBetween
    ) {
        // Аватар
        Box(
            modifier = Modifier
                .size(40.dp)
                .clip(CircleShape)
                .background(if (user.isOnline) Color.Green else Color.Gray)
        )

        // Информация
        Column(
            modifier = Modifier
                .weight(1f)
                .padding(start = 8.dp)
        ) {
            Text(text = user.fullName, fontWeight = FontWeight.Bold)
            Text(text = "@${user.nickname}", color = Color.Gray)
        }

        // Кнопка "Добавить"
        val backgroundColor = if (added) Color.White else Color(0xFF4CAF50)
        val contentColor = if (added) Color(0xFF4CAF50) else Color.White
        val border = if (added) BorderStroke(1.dp, Color(0xFF4CAF50)) else null

        Button(
            onClick = {
                added = !added
                onAddFriendClick()
            },
//            colors = ButtonDefaults.buttonColors(
//                backgroundColor = backgroundColor,
//                contentColor = contentColor
//            ),
            border = border,
            shape = RoundedCornerShape(12.dp),
            modifier = Modifier.height(36.dp)
        ) {
            Text(if (added) "Додано" else "Додати")
        }
    }
}