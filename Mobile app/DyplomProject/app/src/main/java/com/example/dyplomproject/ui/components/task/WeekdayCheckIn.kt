package com.example.dyplomproject.ui.components.task
import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.horizontalScroll
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.SolidColor
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.ui.theme.AppColors
import kotlinx.coroutines.delay
import java.time.DayOfWeek
import java.time.Instant
import java.time.LocalDate
import java.time.ZoneId

//@Composable
//fun WeekdayCheckIn(
//    modifier: Modifier = Modifier,
//    isEditMode: Boolean, // true: вибір днів, false: відмітка
//    markedDays: List<Int>,
//    onMarkedDaysChange: (List<Int>) -> Unit,
//    onCheckIn: (String) -> Unit
//) {
//    val days = listOf("Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд")
//    val today = remember { LocalDate.now().dayOfWeek.value % 7 } // Нд має бути 6
//    var checkedInDays by remember { mutableStateOf(setOf<Int>()) }
//
//    Row(modifier = modifier, horizontalArrangement = Arrangement.SpaceAround) {
//        days.forEachIndexed { index, day ->
//            val isMarked = index in markedDays
//            val isToday = index == today
//            val isCheckedIn = index in checkedInDays
//
//            val backgroundColor = when {
//                isToday && isMarked -> Brush.verticalGradient(
//                    colors = listOf(AppColors.Orange, AppColors.Yellow)
//                )
//                isMarked -> SolidColor(AppColors.DarkBlue)
//                else -> SolidColor(AppColors.White)
//            }
//
//            val textColor = when {
//                isToday -> Color.White
//                else -> Color.DarkGray
//            }
//
//            Box(
//                modifier = Modifier
//                    .size(48.dp)
//                    .clip(CircleShape)
//                    .background(backgroundColor)
//                    .clickable {
//                        if (isEditMode) {
//                            val newMarkedDays = if (isMarked)
//                                markedDays - index
//                            else
//                                markedDays + index
//                            onMarkedDaysChange(newMarkedDays.sorted())
//                        } else {
//                            when {
//                                !isMarked -> {} // не можна натиснути
//                                index > today -> onCheckIn("День ще не настав")
//                                index < today -> onCheckIn("Ви пропустили відмітку")
//                                else -> {
//                                    checkedInDays = checkedInDays + index
//                                    onCheckIn("Відмітка успішна!")
//                                }
//                            }
//                        }
//                    }
//                    .border(
//                        width = if (isToday) 2.dp else 1.dp,
//                        color = if (isToday) Color.Red else Color.Gray,
//                        shape = CircleShape
//                    ),
//                contentAlignment = Alignment.Center
//            ) {
//                Text(
//                    text = day,
//                    color = textColor,
//                    fontWeight = if (isToday) FontWeight.Bold else FontWeight.Normal
//                )
//            }
//        }
//    }
//}

@Composable
fun WeekdayEditSelector(
    modifier: Modifier = Modifier,
    markedDays: List<Int>,
    onMarkedDaysChange: (List<Int>) -> Unit
) {
    val days = listOf("Нд", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб")
    Row(
        modifier = modifier
            .horizontalScroll(rememberScrollState()),
        horizontalArrangement = Arrangement.spacedBy(4.dp, Alignment.CenterHorizontally)
    ) {
        days.forEachIndexed { index, day ->
            val isMarked = index in markedDays
            Box(
                modifier = Modifier
                    .size(48.dp)
                    .clip(CircleShape)
                    .background(if (isMarked) AppColors.DarkBlue else AppColors.White)
                    .clickable {
                        val newMarkedDays = if (isMarked)
                            markedDays - index
                        else
                            markedDays + index
                        onMarkedDaysChange(newMarkedDays.sorted())
                    }
                    .border(1.dp, Color.Gray, CircleShape),
                contentAlignment = Alignment.Center
            ) {
                Text(
                    text = day,
                    color = if (isMarked) Color.White else Color.DarkGray
                )
            }
        }
    }
}

@Composable
fun WeekdayEditSelectorFromNames(
    modifier: Modifier = Modifier,
    markedDayNames: List<String>,
    onMarkedDayNamesChange: (List<String>) -> Unit
) {
    val englishToIndex = listOf("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday")
    val indexToEnglish = englishToIndex.withIndex().associate { it.index to it.value }

    val markedIndices = markedDayNames.mapNotNull { englishToIndex.indexOf(it).takeIf { it != -1 } }

    WeekdayEditSelector(
        modifier = modifier,
        markedDays = markedIndices,
        onMarkedDaysChange = { newMarkedIndices ->
            val newMarkedNames = newMarkedIndices.mapNotNull { indexToEnglish[it] }
            onMarkedDayNamesChange(newMarkedNames)
        }
    )
}
//
//@Composable
//fun WeekdayCheckInView(
//    modifier: Modifier = Modifier,
//    markedDays: List<String>, // Changed from List<Int>
//    onCheckIn: (String) -> Unit
//) {
//    val days = listOf("Нд","Пн", "Вт", "Ср", "Чт", "Пт", "Сб")
//    val dayNames = listOf("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday")
//
//    val today = remember { LocalDate.now().dayOfWeek.value % 7 }
//    var checkedInDays by remember { mutableStateOf(setOf<Int>()) }
//
//    Row(
//        modifier = modifier
//            .horizontalScroll(rememberScrollState()),
//        horizontalArrangement = Arrangement.spacedBy(4.dp, Alignment.CenterHorizontally)
//    ) {
//        days.forEachIndexed { index, day ->
//            val englishDayName = dayNames[index]
//            val isMarked = englishDayName in markedDays
//            val isToday = index == today
//            val isCheckedIn = index in checkedInDays
//
//            val backgroundBrush = when {
//                isToday && isMarked -> Brush.verticalGradient(
//                    colors = listOf(AppColors.Orange, AppColors.Yellow)
//                )
//                isMarked -> SolidColor(AppColors.DarkBlue)
//                else -> SolidColor(AppColors.Gray)
//            }
//            val textColor = if (isToday) Color.White else Color.DarkGray
//
//            Box(
//                modifier = Modifier
//                    .size(48.dp)
//                    .clip(CircleShape)
//                    .background(brush = backgroundBrush)
//                    .clickable {
//                        when {
//                            !isMarked -> {}
//                            index > today -> onCheckIn("День ще не настав")
//                            index < today -> onCheckIn("Ви пропустили відмітку")
//                            else -> {
//                                checkedInDays = checkedInDays + index
//                                onCheckIn("Відмітка успішна!")
//                            }
//                        }
//                    }
//                    .border(
//                        width = if (isToday) 2.dp else 1.dp,
//                        color = if (isToday) Color.Red else Color.Transparent,
//                        shape = CircleShape
//                    ),
//                contentAlignment = Alignment.Center
//            ) {
//                Text(
//                    text = day,
//                    color = textColor,
//                    fontWeight = if (isToday) FontWeight.Bold else FontWeight.Normal
//                )
//            }
//        }
//    }
//}

@Composable
fun WeekdayCheckInView(
    modifier: Modifier = Modifier,
    markedDays: List<String>,
    checkedInDates: List<String>,
    onCheckIn: (String, String) -> Unit
) {
    val days = listOf("Нд", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб")
    val dayNames = listOf("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday")
    val todayDate = remember { LocalDate.now() }
    val today = todayDate.dayOfWeek.value % 7

    val weekStart = todayDate.minusDays(today.toLong())
    val weekEnd = weekStart.plusDays(6)
    val checkedInLocalDates = checkedInDates.mapNotNull {
        runCatching { Instant.parse(it).atZone(ZoneId.systemDefault()).toLocalDate() }.getOrNull()
    }.filter { it in weekStart..weekEnd }
    val checkedInDayIndices = checkedInLocalDates.map { it.dayOfWeek.value % 7 }.toSet()
    Row(
        modifier = modifier.horizontalScroll(rememberScrollState()),
        horizontalArrangement = Arrangement.spacedBy(4.dp, Alignment.CenterHorizontally)
    ) {
        days.forEachIndexed { index, day ->
            val englishDayName = dayNames[index]
            val isMarked = englishDayName in markedDays
            val isToday = index == today
            val alreadyCheckedInToday = checkedInLocalDates.contains(todayDate)
            val isCheckedIn = index in checkedInDayIndices
            val backgroundBrush = when {
                isCheckedIn -> Brush.verticalGradient(
                    colors = listOf(AppColors.Yellow, AppColors.Orange)
                )
                isMarked -> SolidColor(AppColors.DarkBlue)
                else -> SolidColor(AppColors.Gray)
            }

            val textColor = if (isMarked) Color.White else Color.DarkGray
            Box(
                modifier = Modifier
                    .size(48.dp)
                    .clip(CircleShape)
                    .background(brush = backgroundBrush)
                    .clickable(enabled = isMarked){
                        when {
                            !isMarked -> {}
                            index > today -> onCheckIn("День ще не настав", "")
                            index < today -> {
                                if (isCheckedIn) {
                                    onCheckIn("Ви успішно відмітились в день занять! Так тримати!", "")
                                } else {
                                    onCheckIn("На жаль, ви пропустили відмітку, aле не фоксуйтесь на помилках минулого!", "")
                                }

                            }
                            else -> {
                                if (!alreadyCheckedInToday) {
                                    onCheckIn("Відмітка успішна!", Instant.now().toString())
                                } else {
                                    onCheckIn("Відмітка сьогодні вже зроблена!Відпочивай!", "")
                                }
                            }
                        }
                    }
                    .border(
                        width = if (isToday) 2.dp else 1.dp,
                        color = if (isToday) Color.Red else Color.Transparent,
                        shape = CircleShape
                    ),
                contentAlignment = Alignment.Center
            ) {
                Text(
                    text = day,
                    color = textColor,
                    fontWeight = if (isToday) FontWeight.Bold else FontWeight.Normal
                )
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun WeekdayCheckInPreview() {
    var isEditMode by remember { mutableStateOf(false) }
    var markedDays by remember { mutableStateOf(listOf(0, 2, 4)) }
    var markedDaysNames by remember { mutableStateOf(listOf("Sunday")) }
    var message by remember { mutableStateOf("") }
    val checkedInDates by remember { mutableStateOf(listOf("")) }

    LaunchedEffect(message) {
        if (message.isNotBlank()) {
            delay(3000)
            message = ""
        }
    }

    Column(
        Modifier
            .fillMaxSize()
            .padding(32.dp)
    ) {
        if (isEditMode) {
            WeekdayEditSelector(
                markedDays = markedDays,
                onMarkedDaysChange = { markedDays = it }
            )
        } else {
            //val markedDays by viewModel.markedDays.collectAsState()
            //val checkedInDates by viewModel.checkedInDates.collectAsState()

            WeekdayCheckInView(
                markedDays = markedDaysNames,
                checkedInDates = checkedInDates,
                onCheckIn = { message, date ->
                    if (message == "Відмітка успішна!") {
                        //viewModel.checkInToday() // <- send request
                    }
                    //Toast.makeText(LocalContext.current, message, Toast.LENGTH_SHORT).show()
                }
            )

        }

        Spacer(Modifier.height(16.dp))

        if (message.isNotBlank()) {
            Text(text = message, color = Color.Red)
        }

        Spacer(Modifier.height(16.dp))

        Button(onClick = { isEditMode = !isEditMode }) {
            Text(text = if (isEditMode) "Перейти до відмітки" else "Редагувати дні")
        }
    }
}