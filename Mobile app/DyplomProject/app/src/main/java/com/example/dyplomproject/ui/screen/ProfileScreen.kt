package com.example.dyplomproject.ui.screen

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.aspectRatio
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.heightIn
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.layout.wrapContentHeight
import androidx.compose.foundation.layout.wrapContentWidth
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.SolidColor
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import coil.compose.AsyncImage
import com.example.dyplomproject.R
import com.example.dyplomproject.data.remote.dto.UserAchievement
import com.example.dyplomproject.data.remote.repository.UserRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.GradientProgressBar
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.AuthViewModel
import com.example.dyplomproject.ui.viewmodel.ProfileViewModel

@Composable
fun ProfileScreen(
    navController: NavHostController,
    authViewModel: AuthViewModel,
    userId: String,
    padding: PaddingValues,
)  {
    val repository = remember { UserRepository(RetrofitInstance.api) }
    val viewModel: ProfileViewModel = viewModel(factory = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(modelClass: Class<T>): T {
            if (modelClass.isAssignableFrom(ProfileViewModel::class.java)) {
                return ProfileViewModel(repository, userId) as T
            }
            throw IllegalArgumentException("Unknown ViewModel class")
        }
    })
    val state by viewModel.state.collectAsState()
    when {
        state.isLoading -> {
            Box(Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                CircularProgressIndicator()
            }
        }

        state.error != null -> {
            Text("Error: ${state.error}")
        }
        state.userDto != null -> {
            val user = state.userDto!!
            LazyColumn(
                modifier = Modifier
                    .fillMaxSize()
                    .background(AppColors.White)
                    .padding(padding)
                    .padding(horizontal = 16.dp),
                verticalArrangement = Arrangement.spacedBy(16.dp),
                horizontalAlignment = Alignment.Start
            ) {
                item {
                    Text(
                        user.nickname,
                        style = additionalTypography.profileTitle,
                        modifier = Modifier.fillMaxWidth().wrapContentWidth(Alignment.CenterHorizontally)
                    )
                }

                item {
                    AsyncImage(
                        model = user.photo,
                        contentDescription = "Profile photo",
                        modifier = Modifier
                            .fillMaxWidth()
                            .wrapContentWidth(Alignment.CenterHorizontally)
                            .size(120.dp)
                            .clip(CircleShape),
                        contentScale = ContentScale.Crop,
                        placeholder = painterResource(id = R.drawable.avatar),
                        error = painterResource(id = R.drawable.avatar)
                    )
                }

                item {
                    Text(
                        user.fullName,
                        style = MaterialTheme.typography.labelMedium,
                        modifier = Modifier.fillMaxWidth().wrapContentWidth(Alignment.CenterHorizontally),
                        color = Color(0xFF023047)
                    )
                }

                item {
                    Row(
                        verticalAlignment = Alignment.CenterVertically,
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(horizontal = 16.dp)
                    ) {
                        Text(
                            text = "Lvl ${user.levelInfo.level}",
                            modifier = Modifier.padding(end = 16.dp)
                        )

                        GradientProgressBar(
                            progress = user.levelInfo.progress.toFloat(), // assuming 0f to 1f
                            modifier = Modifier.weight(1f), // take the remaining horizontal space
                            height = 12.dp,
                            gradientColors = listOf(Color(0xFF023047), Color(0xFF219DBB))
                        )
                    }
                    Text("XP: ${user.levelInfo.totalXp} / ${user.levelInfo.xpForNextLevel}",//, ${user.levelInfo.progress}",
                        modifier = Modifier.fillMaxWidth().wrapContentWidth(Alignment.End).padding(end = 16.dp)
                    )
                }

                item {
                    Text(
                        user.description,
                        style = MaterialTheme.typography.labelMedium,
                        modifier = Modifier.fillMaxWidth(),
                        color = Color(0xFF063B52),
                        textAlign = TextAlign.Justify
                    )
                }

                item {
                    Text(
                        "Досягнення",
                        style = additionalTypography.profileTitle,
                        color = Color(0xFF023047),
                        modifier = Modifier.clickable { navController.navigate("achievements") }
                    )
                }

                val achievements = state.achievements
                if (!achievements.isNullOrEmpty()) {
                    items(achievements.chunked(2)) { rowItems ->
                        Row(
                            Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.spacedBy(12.dp)
                        ) {
                            rowItems.forEach { achievement ->
                                AchievementCard(
                                    achievement = achievement,
                                    modifier = Modifier.weight(1f)
                                )
                            }
                            if (rowItems.size == 1) Spacer(Modifier.weight(1f))
                        }
                    }
                } else {
                    item {
                        Text(
                            "Немає досягнень",
                            style = additionalTypography.lightText,
                            color = Color.Gray
                        )
                    }
                }

                if (user.friends.isNotEmpty()) {
                    item {
                        Text("Друзі",
                            style = additionalTypography.profileTitle,
                            color = Color(0xFF023047)
                        )
                    }
                    items(user.friends.chunked(2)) { rowItems ->
                        Row(
                            Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.spacedBy(12.dp)
                        ) {
                            rowItems.forEach { friend ->
                                ProfileFriendItem(
                                    friendNickname = friend.nickname,
                                    onProfileFriendItemClick = {},
                                    modifier = Modifier
                                        .weight(1f)
                                )
                            }
                            if (rowItems.size == 1) Spacer(Modifier.weight(1f))
                        }
                    }
                }

                item {
                    Row (
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.End
                    ){
                        TextButton(onClick = { authViewModel.logout()}) {
                            Text(
                                text = "Вийти",
                                style = additionalTypography.semiboldTextUnderlined,
                                modifier = Modifier.padding(16.dp),
                                color = AppColors.DarkBlue
                            )
                        }
                    }

                }
            }
        }
    }
}

@Composable
fun AchievementCard(
    achievement: UserAchievement,
    modifier: Modifier = Modifier,
    showButton: Boolean = false,
    onGetPointsClick: (() -> Unit)? = null
) {
    if (showButton) modifier.heightIn(min = 200.dp, max = 250.dp) else modifier.heightIn(min = 200.dp)
    Card(
        modifier = modifier
            .shadow(6.dp, RoundedCornerShape(16.dp))
            .clip(RoundedCornerShape(16.dp))
            .fillMaxWidth()
            .then(
                if (showButton)
                    Modifier.heightIn(min = 200.dp, max = 250.dp)
                else
                    Modifier.heightIn(min = 50.dp)
            )
    ) {
        Box(
            modifier = Modifier.fillMaxSize()
                .background(
                    if (achievement.isRare) {
                        Brush.verticalGradient(
                            colors = listOf(Color(0xFFFFD600), Color(0xFFFF8C00))
                        )
                    } else {
                        SolidColor(Color.White)
                    }
                )
                .padding(12.dp),
            contentAlignment = Alignment.TopStart
        ) {
            Column (modifier = Modifier.fillMaxSize()){
                Text(
                    achievement.name,
                    style = additionalTypography.profileTitle,
                    fontWeight = FontWeight.Bold,
                )
                Spacer(modifier = Modifier.height(8.dp))
                Text(
                    text = achievement.description,
                    style = MaterialTheme.typography.labelMedium,
                    color = Color(0xFF333333),
                    textAlign = TextAlign.Justify
                )
                Spacer(modifier = Modifier.weight(1f))
                if (showButton) {
                    val isReceived = achievement.isPointsReceived == true
                    Button(
                        onClick = { if (!isReceived) onGetPointsClick?.invoke() },
                        enabled = !isReceived,
                        modifier = Modifier.fillMaxWidth()
                    ) {
                        Text(text = if (isReceived) "Отримано" else "Отримати")
                    }
                }
            }
        }
    }
}

@Composable
fun ProfileFriendItem(
    friendNickname: String,
    onProfileFriendItemClick: () -> Unit,
    modifier: Modifier = Modifier
) {
    Card(
        onClick = onProfileFriendItemClick,
        modifier = modifier,
        shape = RoundedCornerShape(8.dp),
        elevation = CardDefaults.cardElevation(defaultElevation = 0.dp),
        colors = CardDefaults.cardColors(containerColor = Color.Transparent)
    ) {
        Row(
            modifier = Modifier
                .padding(8.dp)
                .wrapContentHeight()
                .fillMaxWidth(),
            verticalAlignment = Alignment.CenterVertically
        ) {
            Box(
                modifier = Modifier
                    .size(40.dp)
                    .clip(CircleShape)
                    .background(Color.LightGray)
            )
            Spacer(modifier = Modifier.width(8.dp))
            Text(
                text = friendNickname,
                style = MaterialTheme.typography.labelLarge,
                maxLines = 1,
                overflow = TextOverflow.Ellipsis
            )
        }
    }
}

@Preview(showBackground = true)
@Composable
fun ProfileScreenPreview() {
    Column(
    ){
        Card(
            Modifier
                .weight(1f)
                .aspectRatio(1f)
        ) {
            Box(contentAlignment = Alignment.Center) {
                Text("lala")
            }
        }
    }
}