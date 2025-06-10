package com.example.dyplomproject.ui.features.statistics

import android.view.ViewGroup
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.viewinterop.AndroidView
import com.example.dyplomproject.data.remote.dto.CompletedTasksLineChartElementDto
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import com.github.mikephil.charting.charts.LineChart
import com.github.mikephil.charting.components.XAxis
import com.github.mikephil.charting.data.Entry
import com.github.mikephil.charting.data.LineData
import com.github.mikephil.charting.data.LineDataSet
import com.github.mikephil.charting.formatter.IndexAxisValueFormatter
import com.github.mikephil.charting.utils.ColorTemplate

val ukrMonthNames = listOf("", "Січень", "Лютий", "Березень", "Квітень", "Травень", "Червень", "Липень", "Серпень", "Вересень", "Жовтень", "Листопад", "Грудень")

fun completedTasksToLineEntries(data: List<CompletedTasksLineChartElementDto>): Pair<List<Entry>, List<String>> {
    val sorted = data.sortedWith(compareBy({ it.year }, { it.month }))
    val entries = sorted.mapIndexed { index, item -> Entry(index.toFloat(), item.completedCount.toFloat()) }
    val labels = sorted.map { ukrMonthNames[it.month] }
    return entries to labels
}

@Composable
fun LineChartView(data: List<Entry>, xLabels: List<String>) {
    if (data.isEmpty()) {
        Box(
            modifier = Modifier
                .fillMaxSize().padding(16.dp),
            contentAlignment = Alignment.Center
        ) {
            Text(
                text = "На разі cтатистика відсутня! \nПочни діяти та сторювати завдання!",
                style = additionalTypography.regularText.copy(color = AppColors.Orange)
            )
        }
    } else {
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
    chart.notifyDataSetChanged()
    chart.invalidate()
}