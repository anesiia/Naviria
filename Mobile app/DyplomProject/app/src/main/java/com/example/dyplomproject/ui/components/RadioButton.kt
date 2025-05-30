package com.example.dyplomproject.ui.components

import androidx.compose.foundation.Image
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.R

//object RadioButtonStyle {
//    const val GenderRadioButton = "radio_button"
//}

@Composable
fun RadioButton(selected: Boolean, text: String, onClick: () -> Unit, style: String = "task") {
    Row(
        verticalAlignment = Alignment.CenterVertically,
        modifier = Modifier
            .clickable(onClick = onClick)
            .padding(4.dp)
    ) {
        var iconRes = R.drawable.ic_radiobutton_unchecked;
        if (style == "gender") {
            iconRes = if (selected) R.drawable.ic_radiobutton_checked else R.drawable.ic_radiobutton_unchecked
        } else if (style == "task"){
            iconRes = if (selected) R.drawable.ic_task_radiobutton_checked else R.drawable.ic_task_radiobutton_unchecked
        }

        Image(
            painter = painterResource(id = iconRes),
            contentDescription = if (selected) "Selected" else "Not selected",
            modifier = Modifier.size(20.dp)
        )
        Spacer(modifier = Modifier.width(8.dp))
        Text(text)
    }
}