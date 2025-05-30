package com.example.dyplomproject.ui.screen

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
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
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.layout.wrapContentHeight
import androidx.compose.foundation.layout.wrapContentWidth
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
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
import com.example.dyplomproject.data.remote.UserAchievement
import com.example.dyplomproject.data.remote.repository.UserRepository
import com.example.dyplomproject.data.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.GradientProgressBar
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.ProfileViewModel


@Composable
fun ProfileScreen(
    navController: NavHostController,
    userId: String,
    padding: PaddingValues
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

        state.user != null -> {
            val user = state.user!!

            LazyColumn(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(padding)
                    .padding(horizontal = 16.dp),
                verticalArrangement = Arrangement.spacedBy(16.dp),
                horizontalAlignment = Alignment.Start
            ) {
                item {
                    if (user.photo.isNotEmpty()) {
                        AsyncImage(
                            model = user.photo,
                            contentDescription = "Profile photo",
                            modifier = Modifier
                                .size(120.dp)
                                .clip(CircleShape),
                            contentScale = ContentScale.Crop
                        )
                    }
                }

//                item {
//                    Text("Full Name: ${user.fullName}")
//                    Text("Nickname: ${user.nickname}")
//                    Text("Email: ${user.email}")
//                    Text("Gender: ${user.gender.uppercase()}")
//                    Text("Birthdate: ${user.birthDate.take(10)}")
//                    Text("Description: ${user.description.ifBlank { "N/A" }}")
//                    Text("Online: ${if (user.isOnline) "🟢" else "⚪"}")
//                    Text("Pro User: ${user.isProUser}")
//                }
                item {
                    Text(
                        user.nickname,
                        style = additionalTypography.profileTitle,
                        modifier = Modifier.fillMaxWidth().wrapContentWidth(Alignment.CenterHorizontally)
                    )
                }

//                item {
//                    Text("Lvl: ${user.levelInfo.level}")
//                    Text("XP: ${user.levelInfo.totalXp}/${user.levelInfo.xpForNextLevel}, ${user.levelInfo.progress}")
//                    GradientProgressBar(
//                        progress = (user.levelInfo.progress).toFloat(),
//                        height = 12.dp,
//                        gradientColors = listOf(Color.Green, Color.Yellow)
//                    )
//                }

                item {
                    Image(
                        painter = painterResource(id = R.drawable.avatar_circle),
                        contentDescription = "avatar_picture",
                        modifier = Modifier.fillMaxWidth().wrapContentWidth(Alignment.CenterHorizontally).padding(vertical = 0.dp).size(120.dp)//.size(200.dp)
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
                    Text("XP: ${user.levelInfo.totalXp}/${user.levelInfo.xpForNextLevel}",//, ${user.levelInfo.progress}",
                        modifier = Modifier.fillMaxWidth().wrapContentWidth(Alignment.End).padding(end = 16.dp)
                    )
                }

                item {
                    Text(
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce vel  ornare neque. Pellentesque venenatis et lectus pretium varius. Nulla  gravida ante at mauris lacinia rutrum.Nulla  gravida ante at mauris lacinia rutrum.",
                        style = MaterialTheme.typography.labelMedium,
                        modifier = Modifier.fillMaxWidth(),
                        color = Color(0xFF063B52),
                        textAlign = TextAlign.Justify
                    )
                }

                item {
                    Text("Особистий прогрес",
                        style = additionalTypography.profileTitle,
                        color = Color(0xFF023047)
                    )
                }

                state.achievements?.let { achievements ->
                    item {
                        Text(
                            "Досягнення",
                            style = additionalTypography.profileTitle,
                            color = Color(0xFF023047)
                        )
                    }
                    items(achievements.chunked(2)) { rowItems ->
                        Row(
                            Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.spacedBy(12.dp)
                        ) {
                            rowItems.forEach { achievement ->
                                AchievementCard(
                                    achievement = achievement,
                                    modifier = Modifier.weight(1f)  // pass modifier here
                                )
                            }
                            if (rowItems.size == 1) Spacer(Modifier.weight(1f))
                        }
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
//                                Card(
//                                    Modifier
//                                        .weight(1f)
//                                        .aspectRatio(1f)
//                                ) {
//                                    Box(contentAlignment = Alignment.Center) {
//                                        Text(friend.nickname)
//                                    }
//                                }
//                                ProfileFriendItem(friend.nickname, {})
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
            }
        }
    }
}

@Composable
fun AchievementCard(
    achievement: UserAchievement,
    modifier: Modifier = Modifier
) {
    Card(
        modifier = modifier
            .shadow(6.dp, RoundedCornerShape(16.dp))
            .clip(RoundedCornerShape(16.dp))
    ) {
        Box(
            modifier = Modifier
                .background(
                    if (achievement.isRare) {
                        Brush.verticalGradient(
                            colors = listOf(Color(0xFFFFD600), Color(0xFFFF8C00))
                        )
                    } else {
                        SolidColor(Color.White)  // SolidColor wraps a single color as a Brush
                    }
                )
                .padding(12.dp),
            contentAlignment = Alignment.TopStart
        ) {
            Column {
                Text(
                    achievement.name,
                    style = additionalTypography.profileTitle,
                    fontWeight = FontWeight.Bold,
                )
                Spacer(modifier = Modifier.height(8.dp))
                Text(
                    text = achievement.description,
                    style = MaterialTheme.typography.labelMedium,
                    color = Color(0xFF333333)
                )
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