package com.example.dyplomproject.ui.theme

import androidx.compose.material3.Typography
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.sp
import androidx.compose.ui.text.font.Font
import androidx.compose.ui.text.style.TextDecoration
import com.example.dyplomproject.R

val MonsterratAlternatesFamily = FontFamily(
    Font(R.font.montserrat_alternates_regular, FontWeight.Normal),
    Font(R.font.montserrat_alternates_bold, FontWeight.Bold),
    Font(R.font.montserrat_alternates_semi_bold, FontWeight.SemiBold),
    Font(R.font.montserrat_alternates_black, FontWeight.Black),
    Font(R.font.montserrat_alternates_medium, FontWeight.Medium),
    Font(R.font.montserrat_alternates_italic, FontWeight.Light)
)
// Set of Material typography styles to start with
val Typography = Typography(
    bodyLarge = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Medium,
        fontSize = 16.sp,
    ),

    bodyMedium = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Medium,
        fontSize = 12.sp,
    ),

    bodySmall = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Medium,
        fontSize = 8.sp,
    ),

    titleLarge = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Black,
        fontSize = 16.sp,
    ),

    labelSmall = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Normal,
        fontSize = 12.sp,
    ),

    labelMedium = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Normal,
        fontSize = 16.sp,
    ),

    labelLarge = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.SemiBold,
        fontSize = 16.sp,
    )
)

data class CustomTypography(
    val exampleText: TextStyle,
    val profileTitle: TextStyle,
    val hashTagSign: TextStyle,
    val hashTagText: TextStyle,
    val semiboldText: TextStyle,
    val regularText: TextStyle,
    val mediumText: TextStyle,
    val lightText: TextStyle,
    val mediumTextUnderlined: TextStyle,
    val semiboldTextUnderlined: TextStyle,
    val boldText: TextStyle
)
val additionalTypography = CustomTypography(
    exampleText = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Light,
        fontSize = 16.sp
    ),

    profileTitle = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Bold,
        fontSize = 16.sp
    ),

    hashTagSign = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Normal,
        fontSize = 24.sp
    ),

    hashTagText = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Normal,
        fontSize = 16.sp
    ),

    semiboldText = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.SemiBold,
        fontSize = 16.sp
    ),

    regularText = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Normal,
        fontSize = 16.sp
    ),

    mediumText = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Medium,
        fontSize = 16.sp
    ),

    lightText = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Light,
        fontSize = 16.sp
    ),

    mediumTextUnderlined = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Medium,
        textDecoration = TextDecoration.Underline,
        fontSize = 16.sp,
    ),

    semiboldTextUnderlined = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.SemiBold,
        textDecoration = TextDecoration.Underline,
        fontSize = 16.sp,
    ),

    boldText = TextStyle(
        fontFamily = MonsterratAlternatesFamily,
        fontWeight = FontWeight.Bold,
        fontSize = 20.sp
    ),

)