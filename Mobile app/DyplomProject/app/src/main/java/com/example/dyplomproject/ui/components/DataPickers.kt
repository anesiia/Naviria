package com.example.dyplomproject.ui.components
import android.app.DatePickerDialog
import android.app.TimePickerDialog
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.wrapContentSize
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.ui.theme.additionalTypography
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.LocalTime
import java.time.ZoneId
import java.time.format.DateTimeFormatter
import java.util.Calendar

data class DateTimeState(
    val date: LocalDate?,
    val time: LocalTime?
) {
    fun toLocalDateTime(): LocalDateTime? =
        if (date != null && time != null) LocalDateTime.of(date, time) else null
}

fun DateTimeState.toIsoUtcString(): String? { //toIsoUtcString
    return when {
        date != null && time != null -> {
            val localDateTime = LocalDateTime.of(date, time)
            localDateTime
                .atZone(ZoneId.systemDefault()) // перетворює до зони пристрою
                .toInstant()                    // в UTC
                .toString()                     // ISO 8601
        }
        date != null -> {
            date
                .atStartOfDay(ZoneId.systemDefault())
                .toInstant()
                .toString()
        }
        else -> null
    }
}

@Composable
fun DateField(
    selectedDate: LocalDate?,
    onDateSelected: (LocalDate) -> Unit
) {
    val context = LocalContext.current
    val formatter = DateTimeFormatter.ofPattern("dd/MM/yyyy")
    val displayText = selectedDate?.format(formatter) ?: ""

    TextField(
        value = displayText,
        onValueChange = {},
        modifier = Modifier
            //.wrapContentSize()
            .clip(RoundedCornerShape(10.dp))
            .clickable {
                val calendar = Calendar.getInstance()
                selectedDate?.let {
                    calendar.set(it.year, it.monthValue - 1, it.dayOfMonth)
                }

                DatePickerDialog(
                    context,
                    { _, year, month, dayOfMonth ->
                        onDateSelected(LocalDate.of(year, month + 1, dayOfMonth))
                    },
                    calendar.get(Calendar.YEAR),
                    calendar.get(Calendar.MONTH),
                    calendar.get(Calendar.DAY_OF_MONTH)
                ).apply {
                    datePicker.minDate = System.currentTimeMillis()
                }.show()
            }.wrapContentSize(),
        placeholder = { Text("дд/мм/гггг", style = additionalTypography.exampleText) },
        readOnly = true,
        enabled = false,
        colors = TextFieldDefaults.colors(
            disabledTextColor = Color(0xFF023047),
            disabledLabelColor = Color(0xFFF0F3F6),
            disabledContainerColor = Color(0xFFF0F3F6),
            //focusedContainerColor = Color(0xFFF0F3F6),
            //unfocusedContainerColor = Color(0xFFF0F3F6),// background
            focusedPlaceholderColor = Color(0x99023047),
            unfocusedPlaceholderColor = Color(0x99023047), // placeholder before typing
            focusedTextColor = Color(0xFF023047), //
            unfocusedTextColor = Color(0xFF023047),//0x99023047
            focusedIndicatorColor = Color(0xFFFF9800), // line of the textFieled
            unfocusedIndicatorColor = Color(0x33023047)
        )
    )
}

@Composable
fun TimeField(
    selectedTime: LocalTime?,
    onTimeSelected: (LocalTime) -> Unit
) {
    val context = LocalContext.current
    val displayText = selectedTime?.format(DateTimeFormatter.ofPattern("HH:mm")) ?: ""

    TextField(
        value = displayText,
        onValueChange = {},
        modifier = Modifier
            .wrapContentSize()
            .clip(RoundedCornerShape(10.dp))
            .clickable {
                val now = Calendar.getInstance()
                TimePickerDialog(
                    context,
                    { _, hourOfDay, minute ->
                        onTimeSelected(LocalTime.of(hourOfDay, minute))
                    },
                    now.get(Calendar.HOUR_OF_DAY),
                    now.get(Calendar.MINUTE),
                    true
                ).show()
            },
        placeholder = { Text("чч:мм", style = additionalTypography.exampleText) },
        readOnly = true,
        enabled = false,
        colors = TextFieldDefaults.colors(
            disabledTextColor = Color(0xFF023047),
            disabledLabelColor = Color(0xFFF0F3F6),
            disabledContainerColor = Color(0xFFF0F3F6),
            //focusedContainerColor = Color(0xFFF0F3F6),
            //unfocusedContainerColor = Color(0xFFF0F3F6),// background
            focusedPlaceholderColor = Color(0x99023047),
            unfocusedPlaceholderColor = Color(0x99023047), // placeholder before typing
            focusedTextColor = Color(0xFF023047), //
            unfocusedTextColor = Color(0xFF023047),//0x99023047
            focusedIndicatorColor = Color(0xFFFF9800), // line of the textFieled
            unfocusedIndicatorColor = Color(0x33023047)
        )
    )
}

@Preview(showBackground = true)
@Composable
fun PreviewSeparatedDateTimeFields() {
    val dateTimeState = remember {
        mutableStateOf(DateTimeState(date = null, time = null))
    }

    val dateTime = dateTimeState.value.toLocalDateTime()

    Column {
        DateField(
            selectedDate = dateTimeState.value.date,
            onDateSelected = {
                dateTimeState.value = dateTimeState.value.copy(date = it)
            }
        )
        TimeField(
            selectedTime = dateTimeState.value.time,
            onTimeSelected = {
                dateTimeState.value = dateTimeState.value.copy(time = it)
            }
        )

        if (dateTime != null) {
            Text("Selected DateTime: ${dateTime.format(DateTimeFormatter.ofPattern("dd/MM/yyyy HH:mm"))}")
        }
    }
}