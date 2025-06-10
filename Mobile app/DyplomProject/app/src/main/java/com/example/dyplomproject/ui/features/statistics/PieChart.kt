package com.example.dyplomproject.ui.features.statistics

import androidx.compose.runtime.Composable
import androidx.compose.ui.viewinterop.AndroidView
import com.github.mikephil.charting.charts.PieChart
import com.github.mikephil.charting.data.*
import android.view.ViewGroup
import com.example.dyplomproject.data.remote.dto.CategoryPieCharSliceDto
import com.github.mikephil.charting.components.Legend

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
