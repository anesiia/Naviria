package com.example.dyplomproject.ui.theme

import androidx.compose.material3.Typography
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.sp

// Set of Material typography styles to start with
import androidx.compose.ui.text.font.Font
import com.example.dyplomproject.R

val MonsterratAlternatesFamily = FontFamily(
    Font(R.font.montserrat_alternates_regular, FontWeight.Normal),
    Font(R.font.montserrat_alternates_bold, FontWeight.Bold),
    Font(R.font.montserrat_alternates_semi_bold, FontWeight.SemiBold),
    Font(R.font.montserrat_alternates_black, FontWeight.Black)
)
// Set of Material typography styles to start with
val Typography = Typography(
    bodyLarge = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Normal,
        fontSize = 16.sp,
    ),

    titleLarge = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Black,
        fontSize = 24.sp,
    ),

    labelSmall = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Normal,
        fontSize = 14.sp,
    ),

    labelLarge = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.SemiBold,
        fontSize = 16.sp,
    )
)