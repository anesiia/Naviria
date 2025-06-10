package com.example.dyplomproject.ui.features.friends

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.LazyListState
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.ui.viewmodel.FriendsUiState
import com.example.dyplomproject.ui.viewmodel.FriendsViewModel
import androidx.compose.foundation.lazy.items
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel

@Composable
fun FriendListContent(
    uiState: FriendsUiState,
    listState: LazyListState,
    onAddFriendClick: (UserShortUiModel) -> Unit,
    onRemoveClick: (UserShortUiModel) -> Unit,
    onSendSupportRequest: (UserShortUiModel) -> Unit
) {
    val baseList = when (uiState.selectedTab) {
        FriendsViewModel.TabType.FRIENDS -> uiState.friends
        FriendsViewModel.TabType.ALL_USERS -> uiState.allUsers
    }
    val listToShow = uiState.searchResults ?: baseList

    LazyColumn(state = listState, modifier = Modifier.fillMaxSize()) {
        when {
            uiState.isLoading -> {
                item {
                    Box(
                        modifier = Modifier
                            .fillParentMaxSize()
                            .padding(16.dp),
                        contentAlignment = Alignment.Center
                    ) {
                        CircularProgressIndicator()
                    }
                }
            }

            uiState.searchResults != null && uiState.selectedCategory.id.isNotEmpty() && listToShow.isEmpty() -> {
                item {
                    Text(
                        "Не знайдено жодного користувача за пошуком \"${uiState.searchQuery}\" з категорією \"${uiState.selectedCategory.name}\"",
                        style = additionalTypography.mediumText,
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(16.dp)
                    )
                }
            }

            uiState.searchResults != null && listToShow.isEmpty() -> {
                item {
                    Text(
                        "Не знайдено жодного користувача з за пошуком \"${uiState.searchQuery}\"",
                        style = additionalTypography.mediumText,
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(16.dp)
                    )
                }
            }

            listToShow.isEmpty() -> {
                item {
                    Text(
                        "Потрібен час, щоб знайти однодумців, тож поки тут пусто.",
                        style = additionalTypography.semiboldText,
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(16.dp)
                    )
                }
            }

            else -> {
                items(listToShow, key = { it.id }) { user ->
                    when (uiState.selectedTab) {
                        FriendsViewModel.TabType.FRIENDS -> {
                            FriendItem(
                                user = user,
                                onRemoveClick = { onRemoveClick(user) },
                                onSendSupportRequest = { onSendSupportRequest(user) }
                            )
                        }
                        FriendsViewModel.TabType.ALL_USERS -> {
                            UserItem(
                                user = user,
                                onAddFriendClick = onAddFriendClick,//{ onAddFriendClick(user) },
                                isAlreadyFriend = uiState.friends.any { it.id == user.id }
                            )
                        }
                    }
                }
            }
        }
    }
}