package com.example.dyplomproject.ui.screen

import android.util.Log
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.navigation.NavHostController
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.ui.viewinterop.AndroidView
import com.github.mikephil.charting.charts.PieChart
import com.github.mikephil.charting.charts.LineChart
import com.github.mikephil.charting.components.XAxis
import com.github.mikephil.charting.data.*
import com.github.mikephil.charting.formatter.IndexAxisValueFormatter
import com.github.mikephil.charting.utils.ColorTemplate
import android.view.ViewGroup
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.MaterialTheme
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.draw.clip
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import coil.compose.AsyncImage
import com.example.dyplomproject.data.remote.dto.CategoryPieCharSliceDto
import com.example.dyplomproject.data.remote.dto.CompletedTasksLineChartElementDto
import com.example.dyplomproject.data.remote.dto.TopUserDto
import com.example.dyplomproject.data.remote.repository.StatisticsRepository
import com.example.dyplomproject.data.remote.repository.UserRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.ButtonStyle
import com.example.dyplomproject.ui.components.SecondaryButton
import com.example.dyplomproject.ui.features.statistics.LineChartView
import com.example.dyplomproject.ui.features.statistics.PieChartView
import com.example.dyplomproject.ui.features.statistics.StatsCard
import com.example.dyplomproject.ui.features.statistics.categoryToPieEntries
import com.example.dyplomproject.ui.features.statistics.completedTasksToLineEntries
import com.example.dyplomproject.ui.viewmodel.FriendsViewModel
import com.example.dyplomproject.ui.viewmodel.StatisticsTab
import com.example.dyplomproject.ui.viewmodel.StatisticsViewModel
import com.github.mikephil.charting.components.Legend
import com.google.accompanist.swiperefresh.SwipeRefresh
import com.google.accompanist.swiperefresh.rememberSwipeRefreshState

@Composable
fun StatisticsScreen(
    navController: NavHostController,
    userId: String,
    padding: PaddingValues
) {
    val repository = remember { StatisticsRepository(RetrofitInstance.api) }
    val viewModel: StatisticsViewModel = viewModel(factory = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(modelClass: Class<T>): T {
            if (modelClass.isAssignableFrom(StatisticsViewModel::class.java)) {
                return StatisticsViewModel(repository, userId) as T
            }
            throw IllegalArgumentException("Unknown ViewModel class")
        }
    })
    val uiState by viewModel.uiState.collectAsState()
    val scrollState = rememberScrollState()
    SwipeRefresh(state = rememberSwipeRefreshState(uiState.isLoading), onRefresh = { viewModel.loadAllStatistics() }) {
        LazyColumn(
            modifier = Modifier
                .fillMaxSize()
                .background(Color.White)
                .padding(padding),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            item {
                Spacer(Modifier.height(8.dp))
                Text("Статистика виконання цілей за категоріями", color = AppColors.Orange, style = additionalTypography.semiboldText, textAlign = TextAlign.Center)
                Text("В розділі \"Особиста\" ти можеш переглянути розподіл виконаних цілей за категоріями задач — це допоможе краще зрозуміти, " +
                        "у яких сферах ти найактивніший, а де ще є простір для зростання. Це чудова можливість оцінити свій " +
                        "прогрес, знайти новий стимул і впевнено рухатись вперед. Пам’ятай: кожен завершений крок — це вже успіх! " +
                        "В двох наступних роздліх можна передивитись розподіл серед твоїх друзів та всіх користувачів застосунку",
                    color = AppColors.DarkBlue,
                    style = additionalTypography.regularText,
                    textAlign = TextAlign.Justify,
                    modifier = Modifier.padding(16.dp))

                PieChartView(
                    data = categoryToPieEntries(
                        when (uiState.selectedTab) {
                            StatisticsTab.USER -> uiState.userCategoryChart
                            StatisticsTab.FRIENDS -> uiState.friendsCategoryChart
                            StatisticsTab.GLOBAL -> uiState.globalCategoryChart
                        }
                    )
                )

                Row(Modifier.padding(8.dp), horizontalArrangement = Arrangement.SpaceBetween) {
                    StatisticsTab.values().forEach { tab ->
                        SecondaryButton(
                            text = when (tab) {
                                StatisticsTab.USER -> "Особиста"
                                StatisticsTab.FRIENDS -> "Друзі"
                                StatisticsTab.GLOBAL -> "Всі користувачі"
                            },
                            onClick = { viewModel.changeTab(tab) },
                            modifier = Modifier.weight(1f),
                            style = if (uiState.selectedTab == tab) ButtonStyle.Outline() else ButtonStyle.Primary,
                        )
                        Spacer(Modifier.width(2.dp))
                    }
                }
                Spacer(Modifier.height(8.dp))
            }

            item {
                Column(modifier = Modifier.fillMaxWidth().padding(horizontal = 8.dp)) {
                    HorizontalDivider()
                    Row(Modifier.fillMaxWidth()) {
                        StatsCard(
                            title = "Користувачів",
                            value = uiState.usersCount.toString(),
                            modifier = Modifier.weight(1f)
                                .padding(horizontal = 8.dp)
                                .wrapContentHeight()
                        )
                        StatsCard(
                            title = "Ви з нами вже",
                            value = uiState.daysSinceUserRegistered.toString(),
                            modifier = Modifier.weight(1f)
                                .padding(horizontal = 8.dp)
                        )
                    }
                    Row(Modifier.fillMaxWidth()) {
                        StatsCard(
                            title = "Всього задач",
                            value = uiState.tasksCount.toString(),
                            modifier = Modifier.weight(1f)
                                .wrapContentHeight()
                        )
                        StatsCard(
                            title = "З них виконано",
                            value = "${uiState.completedTasksPercentage}%",
                            modifier = Modifier.weight(1f)
                        )

                    }
                    Row(Modifier.fillMaxWidth()) {
                        StatsCard(
                            title = "Днів застосунку",
                            value = "${uiState.daysSinceAppCreated}",
                            modifier = Modifier.weight(1f)
                                .padding(horizontal = 100.dp)
                        )
                    }
                    HorizontalDivider()
                }
            }

            item {
                Spacer(Modifier.height(8.dp))
                Text("Продуктивність по місяцям", color = AppColors.Orange, style = additionalTypography.semiboldText, textAlign = TextAlign.Center)
                Text("Переглянь свою статистику виконання цілей та надихайся прогресом! У розділі \"Особиста\" ти бачиш власний " +
                        "шлях — це твій розвиток, твої перемоги і виклики, які ти вже подолав. У категорії \"Друзі\" " +
                        "можна порівняти результати з тими, хто поруч — змагайтесь, підтримуйте одне одного " +
                        "та досягайте більшого разом. А в розділі \"Всі користувачі\" відкривається глобальна картина — " +
                        "тут ти бачиш себе серед усіх, хто також працює над своїми цілями.",
                    color = AppColors.DarkBlue,
                    style = additionalTypography.regularText,
                    textAlign = TextAlign.Justify,
                    modifier = Modifier.padding(16.dp))
                val (lineEntries, xLabels) = completedTasksToLineEntries(
                    when (uiState.selectedTab) {
                        StatisticsTab.USER -> uiState.userChart
                        StatisticsTab.FRIENDS -> uiState.friendsChart
                        StatisticsTab.GLOBAL -> uiState.globalChart
                    }
                )
                LineChartView(data = lineEntries, xLabels = xLabels)
            }

            item {
                Spacer(Modifier.height(16.dp))
                Text("Таблиця лідерів", color = AppColors.Orange, style = additionalTypography.semiboldText, textAlign = TextAlign.Center)
                Text("Нижче представлена таблиця лідерів нашого застосунку! " +
                        "Ще не бачите свого нікнейму? Не засмучуйтесь — кожен лідер колись починав з першого кроку. " +
                        "Продовжуйте активно користуватись додатком, вдосконалюйте свої навички, і вже зовсім скоро " +
                        "ваше ім’я може опинитися серед кращих. У вас все обов’язково вийде!",
                    color = AppColors.DarkBlue,
                    style = additionalTypography.regularText,
                    textAlign = TextAlign.Justify,
                    modifier = Modifier.padding(16.dp))
                Spacer(Modifier.height(4.dp))
                LeaderboardTable(users = uiState.leaderboard)
                Spacer(Modifier.height(16.dp))
            }
        }
    }
}


@Composable
fun LeaderboardTable(users: List<TopUserDto>) {
    Column(modifier = Modifier.fillMaxWidth().padding(8.dp)) {
        users.forEachIndexed { index, user ->
            Row(
                verticalAlignment = Alignment.CenterVertically,
                modifier = Modifier.fillMaxWidth().padding(vertical = 4.dp)
            ) {
                Text("${index + 1}", modifier = Modifier.width(30.dp))
                AsyncImage(
                    model = user.photo,
                    contentDescription = null,
                    modifier = Modifier.size(40.dp).clip(CircleShape)
                )
                Spacer(modifier = Modifier.width(8.dp))
                Column(modifier = Modifier.weight(1f)) {
                    Text(user.fullName, fontWeight = FontWeight.Bold)
                    Text("@${user.nickname}", style = MaterialTheme.typography.bodySmall)
                }
                Text("${user.points} pts", modifier = Modifier.width(70.dp), textAlign = TextAlign.End)
            }
            HorizontalDivider()
        }
    }
}

