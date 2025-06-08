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
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.TextUnit
import androidx.compose.ui.unit.sp
import androidx.navigation.NavHostController
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
//import io.github.tehras.charts.piechart.PieChart
//import io.github.tehras.charts.piechart.data.PieChartData
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
import com.example.dyplomproject.data.remote.api.CategoryPieCharSliceDto
import com.example.dyplomproject.data.remote.api.CompletedTasksLineChartElementDto
import com.example.dyplomproject.data.remote.api.TopUserDto
import com.example.dyplomproject.data.remote.repository.StatisticsRepository
import com.example.dyplomproject.data.remote.repository.UserRepository
import com.example.dyplomproject.data.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.ButtonStyle
import com.example.dyplomproject.ui.components.SecondaryButton
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
                                //Modifier
                                .padding(horizontal = 8.dp)
                                //.fillMaxWidth()
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
                                //.padding(horizontal = 8.dp)
                                //.fillMaxWidth()
                                .wrapContentHeight()
                        )
                        StatsCard(
                            title = "З них виконано",
                            value = "${uiState.completedTasksPercentage}%",
                            modifier = Modifier.weight(1f)
                            //.padding(horizontal = 8.dp)
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

//        Column(
//            modifier = Modifier
//                .fillMaxSize()
//                .background(Color.White)
//                .verticalScroll(scrollState)
//                .padding(padding),
//            horizontalAlignment = Alignment.CenterHorizontally
//        ) {
//            Text("Статистика виконання цілей за категоріями", color = AppColors.Orange, style = additionalTypography.semiboldText, textAlign = TextAlign.Center)
//
//            PieChartView(
//                data = categoryToPieEntries(
//                    when (uiState.selectedTab) {
//                        StatisticsTab.USER -> uiState.userCategoryChart
//                        StatisticsTab.FRIENDS -> uiState.friendsCategoryChart
//                        StatisticsTab.GLOBAL -> uiState.globalCategoryChart
//                    }
//                )
//            )
//
//            Row(Modifier.padding(8.dp), horizontalArrangement = Arrangement.SpaceBetween) {
//                StatisticsTab.values().forEach { tab ->
//                    SecondaryButton(
//                        text = when (tab) {
//                            StatisticsTab.USER -> "Особиста"
//                            StatisticsTab.FRIENDS -> "Друзі"
//                            StatisticsTab.GLOBAL -> "Всі користувачі"
//                        },
//                        onClick = { viewModel.changeTab(tab) },
//                        modifier = Modifier.weight(1f),
//                        style = if (uiState.selectedTab == tab) ButtonStyle.Outline() else ButtonStyle.Primary,
//                    )
//                    Spacer(Modifier.width(2.dp))
//                }
//            }
//            HorizontalDivider()
//            Spacer(Modifier.height(8.dp))
//            Column(modifier = Modifier.fillMaxWidth().padding(horizontal = 8.dp)) {
//                Row(Modifier.fillMaxWidth()) {
//                    StatsCard(
//                        title = "Користувачів",
//                        value = uiState.usersCount.toString(),
//                        modifier = Modifier.weight(1f)
//                                //Modifier
//                                .padding(horizontal = 8.dp)
//                            //.fillMaxWidth()
//                            .wrapContentHeight()
//                    )
//                    StatsCard(
//                        title = "Ви з нами вже",
//                        value = uiState.daysSinceUserRegistered.toString(),
//                        modifier = Modifier.weight(1f)
//                            .padding(horizontal = 8.dp)
//                    )
//                }
//                Row(Modifier.fillMaxWidth()) {
//                    StatsCard(
//                        title = "Всього задач",
//                        value = uiState.tasksCount.toString(),
//                        modifier = Modifier.weight(1f)
//                            //.padding(horizontal = 8.dp)
//                            //.fillMaxWidth()
//                            .wrapContentHeight()
//                    )
//                    StatsCard(
//                        title = "З них виконано",
//                        value = "${uiState.completedTasksPercentage}%",
//                        modifier = Modifier.weight(1f)
//                            //.padding(horizontal = 8.dp)
//                    )
//
//                }
//                Row(Modifier.fillMaxWidth()) {
//                    StatsCard(
//                        title = "Днів застосунку",
//                        value = "${uiState.daysSinceAppCreated}",
//                        modifier = Modifier.weight(1f)
//                            .padding(horizontal = 100.dp)
//                    )
//
//                }
//
//            }
//
//            Spacer(Modifier.height(8.dp))
//            Text("Продуктивність по місяцям", color = AppColors.Orange, style = additionalTypography.semiboldText, textAlign = TextAlign.Center)
//
//            val (lineEntries, xLabels) = completedTasksToLineEntries(
//                when (uiState.selectedTab) {
//                    StatisticsTab.USER -> uiState.userChart
//                    StatisticsTab.FRIENDS -> uiState.friendsChart
//                    StatisticsTab.GLOBAL -> uiState.globalChart
//                }
//            )
//            LineChartView(data = lineEntries, xLabels = xLabels)
//
//            Text("Таблиця лідерів", color = AppColors.Orange, style = additionalTypography.semiboldText, textAlign = TextAlign.Center)
//            LeaderboardTable(users = uiState.leaderboard)
//        }
    }
}


val ukrMonthNames = listOf("", "Січень", "Лютий", "Березень", "Квітень", "Травень", "Червень", "Липень", "Серпень", "Вересень", "Жовтень", "Листопад", "Грудень")

fun completedTasksToLineEntries(data: List<CompletedTasksLineChartElementDto>): Pair<List<Entry>, List<String>> {
    val sorted = data.sortedWith(compareBy({ it.year }, { it.month }))
    val entries = sorted.mapIndexed { index, item -> Entry(index.toFloat(), item.completedCount.toFloat()) }
    val labels = sorted.map { ukrMonthNames[it.month] }
    return entries to labels
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
fun categoryToPieEntries(data: List<CategoryPieCharSliceDto>): List<PieEntry> {
    return data.map { PieEntry(it.value.toFloat(), it.categoryName) }
}

@Composable
fun PieChartView(data: List<PieEntry>) {
    AndroidView(
        factory = { context ->
            PieChart(context).apply {
                layoutParams = ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.MATCH_PARENT,
                    600
                )
                setPieChartProperties(this, data)
            }
        },
        update = { chart ->
            setPieChartProperties(chart, data)
        }
    )
}

private fun setPieChartProperties(chart: PieChart, data: List<PieEntry>) {
    val customColors = listOf(
        0xFFFF0000.toInt(),
        0xFF00FF00.toInt(),
        0xFF0000FF.toInt(),
        0xFFFFFF00.toInt(),
        0xFF00FFFF.toInt(),
        0xFFFF00FF.toInt(),
        0xFFD3D3D3.toInt(),
        0xFF444444.toInt(),
        0xFFFF5722.toInt(),
        0xFF00BCD4.toInt(),
        0xFF8BC34A.toInt(),
        0xFFE91E63.toInt()
    )

    val dataSet = PieDataSet(data, "").apply {
        colors = customColors
        valueTextSize = 16f
        sliceSpace = 2f
        setDrawValues(true)
    }

    chart.data = PieData(dataSet)
    chart.setUsePercentValues(true)
    chart.setDrawEntryLabels(false)
    chart.description.isEnabled = false
    chart.legend.apply {
        isEnabled = true
        textSize = 16f
        form = Legend.LegendForm.CIRCLE
        verticalAlignment = Legend.LegendVerticalAlignment.CENTER
        horizontalAlignment = Legend.LegendHorizontalAlignment.RIGHT
        orientation = Legend.LegendOrientation.VERTICAL
        setDrawInside(false)
        xEntrySpace = 10f
        yEntrySpace = 10f
    }
    chart.invalidate()
}

@Composable
fun LineChartView(data: List<Entry>, xLabels: List<String>) {
    AndroidView(
        factory = { context ->
            LineChart(context).apply {
                layoutParams = ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.MATCH_PARENT,
                    600
                )

                setChartProperties(this, data, xLabels)
            }
        },
        update = { chart ->
            setChartProperties(chart, data, xLabels)
        }
    )
}

@Composable
fun StatsCard(title: String, value: String, modifier: Modifier = Modifier) {
    Card(
        modifier = modifier
            .padding(8.dp)
            .wrapContentHeight()
            .fillMaxWidth(), // Each card takes 50% of row width
        elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
        shape = RoundedCornerShape(16.dp),
        colors = CardDefaults.cardColors(containerColor = AppColors.MilkBlue)
    ) {
        Column(
            modifier = Modifier
                .padding(16.dp)
                .align(Alignment.CenterHorizontally),
            verticalArrangement = Arrangement.spacedBy(8.dp),
            //verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {

            Text(title, style = MaterialTheme.typography.bodyMedium, color = AppColors.DarkBlue,  maxLines = 1)
            Text(value, style = MaterialTheme.typography.headlineMedium, fontWeight = FontWeight.Bold, color = AppColors.DarkBlue)
        }
    }
}

private fun setChartProperties(chart: LineChart, data: List<Entry>, xLabels: List<String>) {
    val dataSet = LineDataSet(data, "Продуктивність").apply {
        color = ColorTemplate.rgb("#5E35B1")
        valueTextSize = 16f
        setDrawFilled(true)
        fillColor = ColorTemplate.rgb("#D1C4E9")
        setDrawCircles(true)
        circleRadius = 8f
        lineWidth = 2f
    }

    chart.data = LineData(dataSet)

    chart.xAxis.apply {
        valueFormatter = IndexAxisValueFormatter(xLabels)
        granularity = 1f
        position = XAxis.XAxisPosition.BOTTOM
        setDrawGridLines(false)
    }

    chart.axisLeft.apply {
        axisMinimum = 0f
        setDrawGridLines(true)
    }

    chart.axisRight.isEnabled = false

    chart.description.isEnabled = false
    chart.legend.isEnabled = false

    // ✅ Force update
    chart.notifyDataSetChanged()
    chart.invalidate()
}
@Composable
fun PieChartView() {
    AndroidView(factory = { context ->
        PieChart(context).apply {
            layoutParams = ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MATCH_PARENT,
                600
            )

            val entries = listOf(
                PieEntry(63f, ""),
                PieEntry(26f, ""),
                PieEntry(5f, ""),
                PieEntry(5f, ""),
                //PieEntry(5f)
            )

            val dataSet = PieDataSet(entries, "").apply {
                colors = listOf(
                    ColorTemplate.rgb("#00B2FF"),
                    ColorTemplate.rgb("#FFA726"),
                    ColorTemplate.rgb("#66BB6A"),
                    ColorTemplate.rgb("#BA68C8"),
                    ColorTemplate.rgb("#B0BEC5")
                )
                valueTextSize = 14f
            }

            data = PieData(dataSet)
            description.isEnabled = false
            legend.isEnabled = true
            setUsePercentValues(true)
            invalidate()
        }
    })
}

@Composable
fun LineChartView() {
    AndroidView(factory = { context ->
        LineChart(context).apply {
            layoutParams = ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MATCH_PARENT,
                600
            )

            val entries = listOf(
                Entry(0f, 55f),
                Entry(1f, 65f),
                Entry(2f, 75f),
                Entry(3f, 80f),
                Entry(4f, 70f),
                Entry(5f, 38f)
            )

            val dataSet = LineDataSet(entries, "Продуктивність").apply {
                color = ColorTemplate.rgb("#5E35B1")
                valueTextSize = 12f
                setDrawFilled(true)
                fillColor = ColorTemplate.rgb("#D1C4E9")
                setDrawCircles(true)
                circleRadius = 4f
                lineWidth = 2f
            }

            data = LineData(dataSet)
            xAxis.valueFormatter = IndexAxisValueFormatter(
                listOf("Грудень", "Січень", "Лютий", "Березень", "Квітень", "Травень")
            )
            xAxis.granularity = 1f
            xAxis.position = XAxis.XAxisPosition.BOTTOM

            axisRight.isEnabled = false
            description.isEnabled = false
            legend.isEnabled = false

            invalidate()
        }
    })
}

@Preview
@Composable
fun StatisticsScreenPreview(){
    Column {
        PieChartView()

        LineChartView()
    }

}