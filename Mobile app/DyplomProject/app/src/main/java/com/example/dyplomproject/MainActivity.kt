package com.example.dyplomproject

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.statusBarsPadding
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
import androidx.compose.material.BottomNavigation
import androidx.compose.material.BottomNavigationItem
import androidx.compose.material.Text
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModelProvider
import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.AuthRepository
import com.example.dyplomproject.data.utils.DataStoreManager
import com.example.dyplomproject.ui.screen.FriendsScreen
import com.example.dyplomproject.ui.screen.RegistrationScreen
import com.example.dyplomproject.ui.screen.StatisticsScreen
import com.example.dyplomproject.ui.screen.TaskScreen
import com.example.dyplomproject.ui.viewmodel.LoginViewModel
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.security.SecureRandom
import java.security.cert.X509Certificate
import javax.net.ssl.SSLContext
import javax.net.ssl.TrustManager
import javax.net.ssl.X509TrustManager


class MainActivity : ComponentActivity() {
    private lateinit var authViewModel: AuthViewModel
    private lateinit var loginViewModel: LoginViewModel
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val trustAllCerts = arrayOf<TrustManager>(
            object : X509TrustManager {
                override fun checkClientTrusted(chain: Array<out X509Certificate>?, authType: String?) {}
                override fun checkServerTrusted(chain: Array<out X509Certificate>?, authType: String?) {}
                override fun getAcceptedIssuers(): Array<X509Certificate> = arrayOf()
            }
        )

        val sslContext = SSLContext.getInstance("SSL")
        sslContext.init(null, trustAllCerts, SecureRandom())

        val okHttpClient = OkHttpClient.Builder()
            .sslSocketFactory(sslContext.socketFactory, trustAllCerts[0] as X509TrustManager)
            .hostnameVerifier { _, _ -> true } // ⚠️ Don't use in production!
            .build()

        val retrofit = Retrofit.Builder()
            .baseUrl("https://10.0.2.2:7172/")//.baseUrl("https://192.168.1.7:7172/")
            .client(okHttpClient)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
        val apiService = retrofit.create(ApiService::class.java)


//        val apiService = Retrofit.Builder()
//            .baseUrl("http://192.168.1.6:5186") // Replace with your actual URL
//            .addConverterFactory(GsonConverterFactory.create())
//            .build()
//            .create(ApiService::class.java)

        val authRepository = AuthRepository(apiService)
        val dataStoreManager = DataStoreManager(applicationContext)


        // Create ViewModel using ViewModelFactory
        authViewModel = ViewModelProvider(this, AuthViewModelFactory(dataStoreManager))
            .get(AuthViewModel::class.java)
        authViewModel.logout()
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
    content: @Composable (PaddingValues) -> Unit
) {
    Scaffold(
        topBar = { CustomTopAppBar(navController) },
        bottomBar = {
            BottomNavigation (
                backgroundColor = Color(0xFFFF8F01),
                contentColor = Color.White
            ){
                BottomNavigationItem(
                    selected = navController.currentDestination?.route == "statistics",
                    onClick = { navController.navigate("statistics") },
//                    icon = { Icon(Icons.Default.StackedLineChart, contentDescription = null) },
                    {
                        Icon(
                            painter = painterResource(id = R.drawable.ic_statatistics),
                            contentDescription = "statistics"
                        )
                    },
                    label = { /*Text("statistics")*/ }
                )
                BottomNavigationItem(
                    selected = navController.currentDestination?.route == "tasks",
                    onClick = { navController.navigate("tasks") },
//                    icon = { Icon(Icons.Default.Task, contentDescription = null) },
                    {
                        Icon(
                            painter = painterResource(id = R.drawable.ic_tasks),
                            contentDescription = "Friends"
                        )
                    },
                    label = { /*Text("tasks")*/ }
                )
                BottomNavigationItem(
                    selected = navController.currentDestination?.route == "friends",
                    onClick = { navController.navigate("friends") },
                    {
                        Icon(
                            painter = painterResource(id = R.drawable.ic_friends),
                            contentDescription = "Friends"
                        )
                    },
                    label = { /*Text("friends")*/ }
                )
            }
        }
    ) { innerPadding ->
        content(innerPadding)
    }

}

@Composable
fun CustomTopAppBar(navController: NavController) {
    Box(
        modifier = Modifier
            .fillMaxWidth()
            .height(90.dp)
            .statusBarsPadding() // Prevent overlap with system bar
    ) {
        Image(
            painter = painterResource(id = R.drawable.topbar_nav_bg),
            contentDescription = null,
            contentScale = ContentScale.Crop,
            modifier = Modifier.matchParentSize()
        )

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
                color = Color.White // Title text in white
            )

            Row {
                IconButton(onClick = { navController.navigate("notifications") }) {
                    Icon(
                        painter = painterResource(id = R.drawable.ic_bell_active),
                        contentDescription = "Notifications",
                        tint = Color.White // Icon in white
                    )
                }
                IconButton(onClick = { navController.navigate("profile") }) {
                    Icon(
                        painter = painterResource(id = R.drawable.ic_avatar),
                        contentDescription = "Profile",
                        tint = Color.White // Icon in white
                    )
                }
            }
        }
    }
}
//@Composable
//fun MyApp(authViewModel: AuthViewModel, loginViewModel: LoginViewModel) {
//    val navController = rememberNavController()
////    val isAuthenticated by authViewModel.isAuthenticated.collectAsState()
////
////    Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
////        Text("Loading... isAuthenticated = $isAuthenticated")
////    }
////    NavHost(
////        navController = navController,
////        startDestination = if (isAuthenticated) "main" else "auth"
////    ) {
////        // Auth Graph (Login/Register)
////        navigation(startDestination = "login", route = "auth") {
////            composable("login") {
////                LoginScreen(navController, loginViewModel, authViewModel)
////            }
//////            composable("register") {
//////                RegisterScreen(navController)
//////            }
////        }
////
////        // Main Graph (Home/Profile)
////        navigation(startDestination = "friends", route = "main") {
////            composable("statistics") {
////                MainScaffold(navController) { StatisticsScreen(navController) }
////            }
////            composable("friends") {
////                MainScaffold(navController) { FriendsScreen(navController) }
////            }
////            composable("tasks") {
////                MainScaffold(navController) { TaskScreen(navController) }
////            }
////            // Add more main screen composables...
////        }
////    }
//    val isAuthenticated by authViewModel.isAuthenticated.collectAsState()
//    val userId by authViewModel.userId.collectAsState()
//
//    // Check if user is authenticated or not
//    if (!isAuthenticated || userId == null) {
//        // If the user is not authenticated or the userId is null, show the login screen
//        NavHost(
//            navController = navController,
//            startDestination = "login"
//        ) {
//            composable("login") {
//                LoginScreen(navController, loginViewModel, authViewModel) // Your login screen
//            }
//            // You can also add register screen here, if needed
//        }
//    } else {
//        // Once user is authenticated and userId is available, proceed to the main screen
//        NavHost(
//            navController = navController,
//            startDestination = "main"
//        ) {
////            // Main Graph (Home/Profile)
////            navigation(startDestination = "friends", route = "main") {
////                composable("friends") {
////                    FriendsScreen(navController, userId!!) // Pass userId to FriendsScreen
////                }
////                composable("statistics") {
////                    StatisticsScreen(navController) // Pass userId to StatisticsScreen
////                }
////                // More composables...
////            }
//            navigation(startDestination = "friends", route = "main") {
//                composable("statistics") {
//                    MainScaffold(navController) { StatisticsScreen(navController) }
//                }
//                composable("friends") {
//                    MainScaffold(navController) { FriendsScreen(navController, userId!!) }
//                }
//                composable("tasks") {
//                    MainScaffold(navController) { TaskScreen(navController) }
//                }
//        }
//        }
//    }
//}
@Composable
fun MyApp(authViewModel: AuthViewModel, loginViewModel: LoginViewModel) {
    val navController = rememberNavController()

    val isAuthenticated by authViewModel.isAuthenticated.collectAsState()
    val userId by authViewModel.userId.collectAsState()

    NavHost(
        navController = navController,
        startDestination = if (isAuthenticated && userId != null) "main" else "login"
    ) {
        // Auth graph
        composable("login") {
            LoginScreen(navController, loginViewModel, authViewModel)
        }

        composable("register") {
            RegistrationScreen(navController, authViewModel)
        }
        // Main graph
        navigation(startDestination = "friends", route = "main") {
//            composable("friends") {
//                MainScaffold(navController) {
//                    FriendsScreen(navController, userId!!)
//                }
//            }
            composable("friends") {
//                MainScaffold(navController) {
//                    userId?.let {
//                        FriendsScreen(navController, it)
//                    } ?: run {
//                        // Show a loading screen or navigate back to login if this is unexpected
//                        Text("Loading user info...")
//                    }
//                }
                MainScaffold(navController) { padding ->
                    userId?.let {
                        FriendsScreen(navController, it, padding)
                        //} ?: Text("Loading user info...")
                        } ?:Box(
                            modifier = Modifier
                                .fillMaxSize()
                                .padding(padding),
                            contentAlignment = Alignment.Center
                        ) {
                            Text("Loading user info...")
                        }
                }
            }
            composable("statistics") {
                MainScaffold(navController) {
                    StatisticsScreen(navController)
                }
            }
            composable("tasks") {
                MainScaffold(navController) { padding ->
                    TaskScreen(navController, padding = padding)
                }
            }
        }
    }
}
//
////@Composable
////fun AppNavigator() {
////    val navController = rememberNavController()
////
////    NavHost(
////        navController = navController,
////        startDestination = Routes.SPLASH
////    ) {
////        composable(Routes.SPLASH) {
////            SplashScreen(navController)
////        }
////
////        composable(Routes.LOGIN) {
////            LoginScreen(navController)
////        }
////
////        composable(Routes.REGISTER) {
////            RegisterScreen(navController)
////        }
////
////        composable(Routes.HOME) {
////            MainLayout(navController)
////        }
////
////        // optionally add other standalone screens
////    }
////}
//
//object Routes {
//    const val SPLASH = "splash_screen"
//    const val LOGIN = "login_screen"
//    const val HOME = "home_screen"
//    const val REGISTER = "register_screen"
//    const val PROFILE_SCREEN = "profile_screen"
//    const val SETTINGS_SCREEN = "settings_screen"
//    const val DETAIL_SCREEN = "detail_screen"
//}
//
//@Composable
//fun MyApp() {
//    val navController = rememberNavController()
//    val authViewModel: AuthViewModel = hiltViewModel()
//    val isAuthenticated by authViewModel.isAuthenticated.collectAsState()
//
//    LaunchedEffect(Unit) {
//        authViewModel.checkAuthentication()
//    }
//
//    when (isAuthenticated) {
//        null -> LoadingScreen()
//        true -> MainApp(navController)
//        false -> AuthNavHost(navController)
//    }
//}
//
////@HiltViewModel
////class AuthViewModel @Inject constructor(
////    private val tokenManager: TokenManager
////) : ViewModel() {
////
////    private val _isAuthenticated = MutableStateFlow<Boolean?>(null)
////    val isAuthenticated: StateFlow<Boolean?> = _isAuthenticated
////
////    fun checkAuthentication() {
////        viewModelScope.launch {
////            _isAuthenticated.value = !tokenManager.getToken().isNullOrBlank()
////        }
////    }
////
////    fun login(token: String) {
////        viewModelScope.launch {
////            tokenManager.saveToken(token)
////            _isAuthenticated.value = true
////        }
////    }
////}
//class AuthViewModel(application: Application) : AndroidViewModel(application) {
//
//    private val userPrefs = UserPreferences(application.applicationContext)
//
//    private val _isLoggedIn = MutableStateFlow(false)
//    val isLoggedIn: StateFlow<Boolean> = _isLoggedIn
//
//    init {
//        viewModelScope.launch {
//            val token = userPrefs.token.first()
//            _isLoggedIn.value = !token.isNullOrEmpty()
//        }
//    }
//
//    fun login(token: String) {
//        viewModelScope.launch {
//            userPrefs.saveToken(token)
//            _isLoggedIn.value = true
//        }
//    }
//
//    fun logout() {
//        viewModelScope.launch {
//            userPrefs.clearToken()
//            _isLoggedIn.value = false
//        }
//    }
//}
//
//@Singleton
//class TokenManager @Inject constructor(@ApplicationContext context: Context) {
//
//    private val Context.dataStore by preferencesDataStore("user_prefs")
//    private val dataStore = context.dataStore
//
//    companion object {
//        val TOKEN_KEY = stringPreferencesKey("auth_token")
//    }
//
//    suspend fun saveToken(token: String) {
//        dataStore.edit { it[TOKEN_KEY] = token }
//    }
//
//    suspend fun getToken(): String? {
//        return dataStore.data.map { it[TOKEN_KEY] }.firstOrNull()
//    }
//
//    suspend fun clearToken() {
//        dataStore.edit { it.remove(TOKEN_KEY) }
//    }
//}
//
//@Composable
//fun BottomBar(navController: NavController) {
//    val items = listOf(
//        BottomNavItem("Home", "home", Icons.Default.Home),
//        BottomNavItem("Profile", "profile", Icons.Default.Person),
//        BottomNavItem("Settings", "settings", Icons.Default.Settings)
//    )
//
//    BottomNavigation {
//        val currentDestination = navController.currentBackStackEntryAsState().value?.destination?.route
//
//        items.forEach { item ->
//            BottomNavigationItem(
//                icon = { Icon(item.icon, contentDescription = item.label) },
//                label = { Text(item.label) },
//                selected = currentDestination == item.route,
//                onClick = {
//                    if (currentDestination != item.route) {
//                        navController.navigate(item.route) {
//                            popUpTo("home") { inclusive = false }
//                            launchSingleTop = true
//                        }
//                    }
//                }
//            )
//        }
//    }
//}
//
//data class BottomNavItem(val label: String, val route: String, val icon: ImageVector)