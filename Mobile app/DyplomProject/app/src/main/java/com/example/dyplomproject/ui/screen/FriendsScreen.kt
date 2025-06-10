package com.example.dyplomproject.ui.screen

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.defaultMinSize
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.layout.wrapContentWidth
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.lazy.rememberLazyListState
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
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.data.remote.repository.UserRepository
import com.example.dyplomproject.ui.components.ButtonStyle
import com.example.dyplomproject.ui.components.DropdownMenuCategory
import com.example.dyplomproject.ui.components.DropdownMenuFiltrationCategory
import com.example.dyplomproject.ui.components.RoundedTextFieldWithIcon
import com.example.dyplomproject.ui.components.SecondaryButton
import com.example.dyplomproject.ui.features.friends.FriendItem
import com.example.dyplomproject.ui.features.friends.FriendListContent
import com.example.dyplomproject.ui.features.friends.FriendRequestsButton
import com.example.dyplomproject.ui.features.friends.UserItem
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.viewmodel.FriendsViewModel
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel
import com.google.accompanist.swiperefresh.SwipeRefresh
import com.google.accompanist.swiperefresh.rememberSwipeRefreshState

@Composable
fun FriendsScreen(
    navController: NavHostController,
    userId: String,
    padding: PaddingValues
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

    val uiState by viewModel.uiState.collectAsState()
    LaunchedEffect(Unit) {
        viewModel.onTabSelected(uiState.selectedTab) // Загрузка при первом старте
        viewModel.checkNewFriendRequests(userId)
        viewModel.loadCategories()
    }
    val context = LocalContext.current
    val message by viewModel.messageFlow.collectAsState(initial = "")
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
            .padding(horizontal = 8.dp),
    ) {
        Spacer(modifier = Modifier.height(32.dp))

        Row(horizontalArrangement = Arrangement.SpaceBetween) {
            SecondaryButton(
                text = "Друзі",
                onClick = {
                    searchInput = ""
                    viewModel.onTabSelected(FriendsViewModel.TabType.FRIENDS)
                },
                style = if (uiState.selectedTab == FriendsViewModel.TabType.FRIENDS) ButtonStyle.Primary else ButtonStyle.Outline(),
                modifier = Modifier.weight(1f)
            )

            Spacer(Modifier.width(24.dp))

            SecondaryButton(
                text = "Всі користувачі",
                onClick = {
                    searchInput = ""
                    viewModel.onTabSelected(FriendsViewModel.TabType.ALL_USERS)
                },
                style = if (uiState.selectedTab == FriendsViewModel.TabType.ALL_USERS) ButtonStyle.Primary else ButtonStyle.Outline(),
                modifier = Modifier.weight(1f)
            )
        }
        Spacer(modifier = Modifier.height(32.dp))
        RoundedTextFieldWithIcon(
            value = searchInput,
            onValueChange = {
                searchInput = it
                viewModel.onSearchQueryChanged(it)
            },
            placeholder = "Знайти нових друзів",
            onIconClick = { viewModel.searchUsers() }
        )

        Spacer(modifier = Modifier.width(16.dp))

        Row(horizontalArrangement = Arrangement.End) {
            Spacer(Modifier.width(8.dp))
            DropdownMenuFiltrationCategory(
                options = uiState.categories,
                selected = uiState.selectedCategory,//category,
                onSelected = {
                    viewModel.onSelectedCategoryChanged(it)
                }
            )
        }
        ////////
        val swipeRefreshState = rememberSwipeRefreshState(isRefreshing = uiState.isLoading)
        val listState = rememberLazyListState()

        SwipeRefresh(
            state = swipeRefreshState,
            onRefresh = {
                searchInput = ""
                viewModel.resetSearch()
                //viewModel.onTabSelected(uiState.selectedTab)
                viewModel.onRefresh()
            },
            modifier = Modifier.weight(1f) // Take remaining vertical space
        ) {
            FriendListContent(
                uiState = uiState,
                listState = listState,
                onAddFriendClick = { viewModel.sendFriendRequest(it) },
                onRemoveClick = { viewModel.removeUserFromFriends(it) },
                onSendSupportRequest = { viewModel.sendFriendSupport(it) }
            )
        }
//            Column(modifier = Modifier.weight(1f)) {
//                if (uiState.isLoading) {
//                    Box(
//                        modifier = Modifier.fillMaxSize(),
//                        contentAlignment = Alignment.Center
//                    ) {
//                        CircularProgressIndicator()
//                    }
//                } else {
////                val listToShow = when (uiState.selectedTab) {
////                    FriendsViewModel.TabType.FRIENDS -> uiState.friends
////                    FriendsViewModel.TabType.ALL_USERS -> uiState.allUsers
////                }
//                    val baseList = when (uiState.selectedTab) {
//                        FriendsViewModel.TabType.FRIENDS -> uiState.friends
//                        FriendsViewModel.TabType.ALL_USERS -> uiState.allUsers
//                    }
//                    val listToShow = uiState.searchResults ?: baseList
//                    if (uiState.searchResults != null && listToShow.isEmpty()) {
//                        Text("Не знайдено жодного користувача з поданим ніком. Той во...забув..буває", style = MaterialTheme.typography.bodyMedium)
//                    } else if (listToShow.isEmpty()) {
//                        Text("Потрібен час, щоб знайти однодумців, тож поки тут пусто", style = MaterialTheme.typography.bodyMedium)
//                    } else {
//                        LazyColumn (state = listState){
//                            items(listToShow, key = { it.id }) { user ->
//                                when (uiState.selectedTab) {
//                                    FriendsViewModel.TabType.FRIENDS -> {
//                                        FriendItem(user = user, onRemoveClick = { /* Handle remove */ }, onSendSupportRequest = {  })
//                                    }
//                                    FriendsViewModel.TabType.ALL_USERS -> {
//                                        UserItem(user = user, onAddFriendClick = { viewModel.sendFriendRequest(user) })
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        Spacer(modifier = Modifier.weight(1f)) // This will push the button to the bottom
        FriendRequestsButton(
            hasNewRequests = uiState.hasNewFriendRequests,
            onClick = { navController.navigate("friend_requests") },
            modifier = Modifier
                .fillMaxWidth()
                .wrapContentWidth(Alignment.End)
        )

//        Button(
//            onClick = { navController.navigate("friend_requests") }, // Navigate to the new composable
//            modifier = Modifier
//                .align(Alignment.End)
//                .padding(vertical = 16.dp),
//            shape = RoundedCornerShape(10.dp),
//            colors = ButtonDefaults.buttonColors(
//                containerColor = if (uiState.hasNewFriendRequests) Color.Red else Color(0x142099B7),
//                disabledContentColor = Color(0x142099B7)
//            )
//        ) {
//            Text(
//                /*text = if (uiState.newFriendRequest) "New Friend Request" else*/ text = "Нові запити",
//                color = Color(0xFF003344),
//                style = MaterialTheme.typography.labelLarge
//            )
//        }
    }
}

