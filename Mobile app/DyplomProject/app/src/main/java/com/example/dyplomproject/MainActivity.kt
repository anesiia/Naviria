package com.example.dyplomproject

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.statusBarsPadding
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.Icon
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue

import androidx.lifecycle.ViewModel
import androidx.navigation.NavController
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navigation
import com.example.dyplomproject.ui.screen.LoginScreen
import com.example.dyplomproject.ui.theme.DyplomProjectTheme
import com.example.dyplomproject.ui.viewmodel.AuthViewModel
import androidx.compose.material.Text
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.NavigationBar
import androidx.compose.material3.NavigationBarItem
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavGraph.Companion.findStartDestination
import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.repository.AuthRepository
import com.example.dyplomproject.data.remote.repository.NotificationRepository
import com.example.dyplomproject.utils.DataStoreManager
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.screen.AchievementsScreen
import com.example.dyplomproject.ui.screen.AssistantChatScreen
import com.example.dyplomproject.ui.screen.FriendRequestsScreen
import com.example.dyplomproject.ui.screen.FriendsScreen
import com.example.dyplomproject.ui.screen.NotificationScreen
import com.example.dyplomproject.ui.screen.ProfileScreen
import com.example.dyplomproject.ui.screen.RegistrationScreen
import com.example.dyplomproject.ui.screen.SettingsScreen
import com.example.dyplomproject.ui.screen.StatisticsScreen
import com.example.dyplomproject.ui.screen.TaskScreen
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.viewmodel.LoginViewModel
import com.example.dyplomproject.ui.viewmodel.NotificationViewModel
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class MainActivity : ComponentActivity() {
    private lateinit var authViewModel: AuthViewModel
    private lateinit var loginViewModel: LoginViewModel
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val apiService = Retrofit.Builder()
            //.baseUrl("http://10.0.2.2:5186/") //Mariam's URL emulator
            //.baseUrl("http://192.168.56.1:5186/")//Mariam's URL Physical device
            //.baseUrl("http://192.168.1.9:5186/") //Lisa's URL
            .baseUrl("http://10.0.2.2:5186/") //Lisa's URL
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(ApiService::class.java)

        val authRepository = AuthRepository(apiService)
        val dataStoreManager = DataStoreManager(applicationContext)

        authViewModel = ViewModelProvider(this, AuthViewModelFactory(dataStoreManager))
            .get(AuthViewModel::class.java)
            //authViewModel.logout()
        loginViewModel = ViewModelProvider(this, LoginViewModelFactory(authRepository))
            .get(LoginViewModel::class.java)
        enableEdgeToEdge()
        setContent {
            DyplomProjectTheme {
                MyApp(authViewModel, loginViewModel)
            }
        }
    }
}

class AuthViewModelFactory(private val dataStoreManager: DataStoreManager) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(AuthViewModel::class.java)) {
            return AuthViewModel(dataStoreManager) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}

class LoginViewModelFactory(private val authRepository: AuthRepository) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(LoginViewModel::class.java)) {
            return LoginViewModel(authRepository) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}

@Composable
fun MainScaffold(
    navController: NavHostController,
    notificationViewModel: NotificationViewModel? = null,
    content: @Composable (PaddingValues) -> Unit
) {
    val bottomNavItems = listOf(
        BottomNavItem("statistics", "Статистика", R.drawable.ic_statatistics),
        BottomNavItem("tasks", "Задачі", R.drawable.ic_tasks),
        BottomNavItem("friends", "Друзі", R.drawable.ic_friends),
        BottomNavItem("assistant_chat", "Помічник", R.drawable.ic_chat_assistant)
    )

    Scaffold(topBar = {
        notificationViewModel?.let {
            NaviriaTopAppBar(navController, it)
        }
    }, bottomBar = {
        NaviriaBottomNavigationMenu(
            navController = navController, items = bottomNavItems
        )
    }) { innerPadding ->
        content(innerPadding)
    }
}

// Data model for navigation items
data class BottomNavItem(
    val route: String,
    val label: String,
    val iconResId: Int
)

@Composable
fun NaviriaBottomNavigationMenu(
    navController: NavHostController,
    items: List<BottomNavItem>,
    modifier: Modifier = Modifier
) {
    Box(
        modifier = modifier
            .fillMaxWidth()
            .background(
                Brush.linearGradient(
                    colors = listOf(AppColors.Orange, AppColors.Yellow)
                )
            )
    ) {
        NavigationBar(
            containerColor = Color.Transparent, //Make NavigationBar background transparent so gradient shows through
            contentColor = AppColors.White,
            modifier = Modifier.fillMaxWidth()
        ) {
            val currentRoute = navController.currentDestination?.route
            items.forEach { item ->
                NavigationBarItem(
                    selected = currentRoute == item.route,
                    onClick = {
                        if (currentRoute != item.route) {
                            navController.popBackStack(navController.graph.findStartDestination().id, false)
                            navController.navigate(item.route) {
                                launchSingleTop = true
                                restoreState = true
                            }
                        }
                    },
                    icon = {
                        Icon(
                            painter = painterResource(id = item.iconResId),
                            contentDescription = item.label,
                            tint = AppColors.White
                        )
                    },
//                    label = {
//                        Text(
//                            text = item.label,
//                            maxLines = 1,
//                            color = AppColors.White,
//                            style = MaterialTheme.typography.labelSmall
//                        )
//                    }
                )
            }
        }
    }
}

//where?
@Composable
fun NaviriaTopAppBar(
    navController: NavController,
    notificationViewModel: NotificationViewModel
) {
    val hasUnread by notificationViewModel.hasUnread
    LaunchedEffect(Unit) {
        notificationViewModel.loadNotifications()
    }
    Box(
        modifier = Modifier
            .fillMaxWidth()
            .height(90.dp)
            .statusBarsPadding()
            .background(
                Brush.linearGradient(
                    colors = listOf(AppColors.Yellow, AppColors.Orange)
                )
            )
    ) {
        Row(
            modifier = Modifier
                .fillMaxSize()
                .padding(horizontal = 16.dp),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Text(
                text = "naviria",
                style = MaterialTheme.typography.titleLarge,
                color = Color.White
            )
            Row {
                IconButton(onClick = { navController.navigate("settings") }) {
                    Icon(
                        painter = painterResource(id = R.drawable.ic_settings),
                        contentDescription = "Settings",
                        tint = Color.White,
                    )
                }
                IconButton(onClick = { navController.navigate("notifications") }) {
                    Icon(
                        painter = painterResource(
                            id = if (hasUnread) R.drawable.ic_bell_active else R.drawable.ic_bell_inactive
                        ),
                        contentDescription = "Notifications",
                        tint = Color.White
                    )
                }
                Image(
                    painter = painterResource(id = R.drawable.avatar),
                    contentDescription = "profile photo",
                    modifier = Modifier
                        .size(40.dp)
                        .clip(CircleShape)
                        .clickable {
                            navController.navigate("profile")
                        },
                    contentScale = ContentScale.Crop
                )
//                IconButton(onClick = { navController.navigate("profile") }) {
//                    Icon(
//                        painter = painterResource(id = R.drawable.ic_avatar),
//                        contentDescription = "Profile",
//                        tint = Color.White
//                    )
//                }
            }
        }
    }
}

class NotificationViewModelFactory(
    private val repository: NotificationRepository,
    private val userId: String
) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        return NotificationViewModel(repository, userId) as T
    }
}

@Composable
fun RequireUserId(
    userId: String?,
    padding: PaddingValues = PaddingValues(0.dp),
    loadingText: String = "Завантаження...",
    content: @Composable (String) -> Unit
) {
    if (userId != null) {
        content(userId)
    } else {
        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(padding),
            contentAlignment = Alignment.Center
        ) {
            Text(loadingText)
        }
    }
}

@Composable
fun MyApp(authViewModel: AuthViewModel, loginViewModel: LoginViewModel) {
    val navController = rememberNavController()

    val isAuthenticated by authViewModel.isAuthenticated.collectAsState()
    val userId by authViewModel.userId.collectAsState()
    val notificationViewModel: NotificationViewModel? = userId?.let {
        val repo = NotificationRepository(RetrofitInstance.api)
        viewModel(factory = NotificationViewModelFactory(repo, it))
    }

    NavHost(
        navController = navController,
        startDestination = if (isAuthenticated && userId != null) "main" else "login"
    ) {
        composable("login") { LoginScreen(navController, loginViewModel, authViewModel) }
        composable("register") { RegistrationScreen(navController, authViewModel) }

        navigation(startDestination = "friends", route = "main") {
            composable("friends") {
                MainScaffold(navController, notificationViewModel) { padding ->
                    RequireUserId(userId, padding) { id ->
                        FriendsScreen(navController, id, padding)
                    }
                }
            }
            composable("statistics") {
                MainScaffold(navController, notificationViewModel) { padding ->
                    RequireUserId(userId, padding) { id ->
                        StatisticsScreen(navController, id, padding)
                    }
                }
            }

            composable("tasks") {
                MainScaffold(navController, notificationViewModel) { padding ->
                    RequireUserId(userId, padding) { id ->
                        TaskScreen(navController, id, padding)
                    }
                }
            }

            composable("assistant_chat") {
                MainScaffold(navController, notificationViewModel) { padding ->
                    RequireUserId(userId, padding) { id ->
                        AssistantChatScreen(navController, id, padding)
                    }
                }
            }

            navigation(route = "secondary", startDestination = "profile") {
                composable("profile") {
                    MainScaffold(navController, notificationViewModel) { padding ->
                        RequireUserId(userId, padding) { id ->
                            ProfileScreen(navController,authViewModel, id, padding)
                        }
                    }
                }
                composable("friend_requests") {
                    MainScaffold(navController, notificationViewModel) { padding ->
                        RequireUserId(userId, padding) { id ->
                            FriendRequestsScreen(navController, id, padding)
                        }
                    }
                }
                composable("notifications") {
                    MainScaffold(navController, notificationViewModel) { padding ->
                        RequireUserId(userId, padding) { id ->
                            NotificationScreen(navController, id, padding)
                        }
                    }
                }
                composable("achievements") {
                    MainScaffold(navController, notificationViewModel) { padding ->
                        RequireUserId(userId, padding) { id ->
                            AchievementsScreen(navController, id, padding)
                        }
                    }
                }

                composable("settings") {
                    MainScaffold(navController, notificationViewModel) { padding ->
                        RequireUserId(userId, padding) { id ->
                            SettingsScreen(navController, id, padding)
                        }
                    }
                }
            }
        }
    }
}
sealed class Screen(val route: String) {
    object Login : Screen("login")
    object Register : Screen("register")
    object Main : Screen("main")
    object Profile : Screen("profile")
    object Notifications : Screen("notifications")
    // etc...
}