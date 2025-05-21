package com.example.dyplomproject.ui.components

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.R

@Composable
fun PrimaryButton(
    imageRes: Int,
    text: String,
    onClick: () -> Unit,
    modifier: Modifier = Modifier.fillMaxWidth(),
    contentColor: Color = Color.White,
    cornerRadius: Dp = 10.dp
) {
    Box(
        modifier = modifier
            .clip(RoundedCornerShape(cornerRadius))
            .clickable(onClick = onClick)
            .background(Color.Transparent) // just in case
    ) {
        Image(
            painter = painterResource(id = imageRes),
            contentDescription = null,
            modifier = Modifier.matchParentSize(),
            contentScale = ContentScale.Crop
        )

        Box(
            modifier = Modifier
                .matchParentSize()
                .background(Color.Black.copy(alpha = 0.3f)) // Optional overlay for contrast
        )

        Text(
            text = text,
            color = contentColor,
            style = MaterialTheme.typography.labelLarge,
            modifier = Modifier
                .align(Alignment.Center)
                .padding(12.dp)
        )
    }
}

@Preview(
    showBackground = true,
    name = "Image Button Preview"
)
@Composable
fun DefaultButtonPreview() {
    PrimaryButton(
        imageRes = R.drawable.default_button_bg,
        text = "Увійти",
        onClick = {}
    )
}