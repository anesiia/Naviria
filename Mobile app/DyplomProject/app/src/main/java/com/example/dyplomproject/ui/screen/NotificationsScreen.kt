package com.example.dyplomproject.ui.screen

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
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
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.dyplomproject.data.remote.repository.NotificationRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.viewmodel.NotificationViewModel
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.Surface
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.navigation.NavHostController
import com.example.dyplomproject.R
import com.example.dyplomproject.data.remote.UserNotification
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import com.google.accompanist.swiperefresh.SwipeRefresh
import com.google.accompanist.swiperefresh.rememberSwipeRefreshState

@Composable
fun NotificationScreen(
    navController: NavHostController,
    userId: String,
    padding: PaddingValues
) {

    // Example repository (you’d create it properly)
//    val repository = remember {
//        NotificationRepository(MyApi.create()) // Or however you build your API
//    }
//
//    val viewModel = remember {
//        ViewModelProvider(
//            context as ViewModelStoreOwner,
//            NotificationViewModelFactory(repository)
//        )[NotificationViewModel::class.java]
//    }
    val repository = remember { NotificationRepository(RetrofitInstance.api) }

    val viewModel: NotificationViewModel = viewModel(factory = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(modelClass: Class<T>): T {
            if (modelClass.isAssignableFrom(NotificationViewModel::class.java)) {
                return NotificationViewModel(repository, userId) as T
            }
            throw IllegalArgumentException("Unknown ViewModel class")
        }
    })

    val notifications by viewModel.notifications
    var isRefreshing by remember { mutableStateOf(false) }

    // Separate notifications
    val newNotifications = notifications.filter { it.isNew }
    val oldNotifications = notifications.filterNot { it.isNew }

    LaunchedEffect(Unit) {
        viewModel.loadNotifications()
    }

    SwipeRefresh(
        state = rememberSwipeRefreshState(isRefreshing),
        onRefresh = {
            isRefreshing = true
            viewModel.loadNotifications {
                isRefreshing = false
            }
        },
        modifier = Modifier
            .fillMaxSize()
            .padding(padding)
    ) {
        Column(modifier = Modifier.fillMaxSize().background(AppColors.White)) {

            if (newNotifications.isEmpty() && oldNotifications.isEmpty()) {
                // Show a centered message when there are no notifications
                Box(
                    modifier = Modifier
                        .fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    Text(
                        text = "На разі повідомлення відсутні!",
                        style = additionalTypography.regularText.copy(color = AppColors.Blue)
                    )
                }
            } else {
                LazyColumn(modifier = Modifier.fillMaxSize().padding(horizontal = 16.dp) , horizontalAlignment = Alignment.CenterHorizontally) {
                    item {
                        Spacer(Modifier.height(8.dp))
                        Text(
                            "Повідомлення",
                            modifier = Modifier.padding(vertical = 16.dp),
                            color = AppColors.DarkBlue,
                            style = additionalTypography.boldText
                        )
                        Spacer(Modifier.height(8.dp))
                    }
                    if (newNotifications.isNotEmpty()) {
                        item {
                            Surface(
                                tonalElevation = 4.dp, // for Material3
                                shadowElevation = 4.dp, // for Material2
                                shape = RoundedCornerShape(8.dp), // optional
                                modifier = Modifier
                                    .fillMaxWidth()
                                    .padding(8.dp) // optional outer spacing
                            ) {
                                Column(
                                    modifier = Modifier
                                        //.fillMaxWidth()
                                        .background(AppColors.NotificationBlueLayout, RoundedCornerShape(8.dp))
                                        .clickable { viewModel.markAllAsRead() }
                                        .padding(16.dp)
                                ) {
                                    Spacer(Modifier.height(8.dp))
                                    Text(
                                        "Нові",
                                        style = additionalTypography.semiboldText,
                                        modifier = Modifier.padding(bottom = 4.dp)
                                    )
                                    HorizontalDivider(color = AppColors.DarkBlue)
                                    newNotifications.forEach { notif ->
                                        Spacer(Modifier.height(8.dp))
                                        NotificationItem(notification = notif)
                                    }
                                }
                            }
                            }

                    }

                    if (oldNotifications.isNotEmpty()) {
                        item {
                            Surface(
                                tonalElevation = 4.dp, // for Material3
                                shadowElevation = 4.dp, // for Material2
                                shape = RoundedCornerShape(8.dp), // optional
                                modifier = Modifier
                                    .fillMaxWidth()
                                    .padding(8.dp) // optional outer spacing
                            ) {
                                Column(
                                    modifier = Modifier
                                        //.fillMaxWidth()
                                        .background(AppColors.White, RoundedCornerShape(8.dp))
                                        .padding(16.dp)
                                ) {
                                    Text(
                                        "Прочитані",
                                        modifier = Modifier.padding(8.dp),
                                        color = AppColors.DarkBlue,
                                        style = additionalTypography.semiboldText
                                    )
                                    HorizontalDivider(color = AppColors.DarkBlue)
                                    oldNotifications.asReversed().forEach { notif ->
                                        Spacer(Modifier.height(8.dp))
                                        NotificationItem(notification = notif)
                                    }
                                }
                            }
                            }

//                        items(oldNotifications) { notif ->
//                            NotificationItem(notification = notif)
//                        }
                    }
                }
            }
        }
    }
}

@Composable
fun NotificationItem(
    notification: UserNotification,
    //backgroundColor: Color,
    //iconRes: Int,
    //onClick: () -> Unit
) {
    Row(
        modifier = Modifier
            .padding(vertical = 16.dp)
            .fillMaxWidth(),
        //verticalAlignment = Alignment.CenterVertically
    ) {
//        Icon(
//            imageVector = icon,
//            contentDescription = "Notification Icon",
//            modifier = Modifier
//                .size(24.dp)
//                .padding(end = 8.dp),
//            tint = Color.Black
//        )
        Icon(
            painter = painterResource(id = R.drawable.ic_acclamation_point),
            modifier = Modifier.size(20.dp),
            tint = AppColors.DarkBlue,
            contentDescription = "Знак оклику"
        )
        Spacer(Modifier.width(8.dp))
        Text(text = notification.text, color = AppColors.DarkBlue)
    }
}
