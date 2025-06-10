package com.example.dyplomproject.ui.screen

import android.graphics.drawable.Icon
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import com.example.dyplomproject.data.remote.repository.AssistantChatRepository
import com.example.dyplomproject.data.remote.repository.UserRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.viewmodel.AssistantChatViewModel
import com.example.dyplomproject.ui.viewmodel.FriendRequestsViewModel
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.Send
import androidx.compose.material.icons.filled.Search
import androidx.compose.material.icons.filled.Send
import androidx.compose.material3.Button
import androidx.compose.material3.Checkbox
import androidx.compose.material3.CheckboxDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.TextField
import androidx.compose.runtime.setValue

import com.example.dyplomproject.ui.components.RoundedTextFieldWithIcon
import com.example.dyplomproject.ui.theme.AppColors
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.magnifier
import androidx.compose.runtime.*
import androidx.compose.ui.modifier.modifierLocalConsumer

@Composable
fun AssistantChatScreen (
    navController: NavHostController,
    userId: String,
    padding: PaddingValues
){
    val viewModel: AssistantChatViewModel =
        viewModel(factory = object : ViewModelProvider.Factory {
            override fun <T : ViewModel> create(modelClass: Class<T>): T {
                return AssistantChatViewModel(AssistantChatRepository(RetrofitInstance.api), userId) as T
            }
        })
    val messages by viewModel::messages
    val userInput by viewModel::userInput
    var isTaskRequest by viewModel::isTaskRequest
    var isChecked by remember { mutableStateOf(false) }
    var isEnabled by remember { mutableStateOf(true) }
    Column(
        modifier = Modifier
            .fillMaxSize()
            .background(AppColors.White)
            .padding(padding)
            .padding(horizontal = 8.dp),
    ) {
        LazyColumn(
            modifier = Modifier
                .weight(1f)
                .padding(8.dp),
            reverseLayout = true
        ) {
            items(messages.reversed()) { message ->
                val isUser = message.role == "user"
//                val modifier = if (isUser) Modifier.padding(start = 48.dp)
//                else Modifier.padding(end = 48.dp)
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                            //background(AppColors.MilkBlue, RoundedCornerShape(8.dp))
                        .padding(vertical = 8.dp)
                        //.then(modifier)
                        ,
                    horizontalArrangement = if (isUser) Arrangement.End else Arrangement.Start
                ) {
//                    val modifier = if (isUser)
//                        Modifier.border(1.dp, color = AppColors.DarkBlue, RoundedCornerShape(8.dp))
//                    else Modifier.border(1.dp, color = AppColors.Orange, RoundedCornerShape(8.dp))
                    Text(

                        text = message.message,
                        modifier = Modifier
                            .background(if (isUser) AppColors.MilkBlue else AppColors.MilkBlue, RoundedCornerShape(8.dp))
                            //.clip(RoundedCornerShape(8.dp))
                            //.then(modifier)
                            .padding(8.dp)
                            ,
                            //.clip(RoundedCornerShape(8.dp)),
                        color = AppColors.DarkBlue
                    )
                }
            }

        }
//        val icon = Icon(
//            painter = painterResource(id = R.drawable.ic_send_message),
//            contentDescription = "",
//            tint = AppColors.DarkBlue,
//        )
        RoundedTextFieldWithIcon(
            value = userInput,
            onValueChange = { viewModel.userInput = it },
            placeholder = "Введіть повідомлення",
            icon = Icons.AutoMirrored.Filled.Send,
            onIconClick = { viewModel.sendMessage() }
        )
        Spacer(Modifier.height(8.dp))
        Row {

            Checkbox(
                checked = isChecked,
                onCheckedChange = { checked ->
                    if (checked) {
                        isChecked = true
                        isTaskRequest = true
                        //isEnabled = false // Disable further changes
                    } else {
                        isChecked = false
                        isTaskRequest = false
                    }
                    // If unchecked is attempted, ignore it
                },
                enabled = isEnabled,
                modifier = Modifier
                    .size(24.dp),
                //.align(Alignment.CenterVertically),
                colors = CheckboxDefaults.colors(
                    checkedColor = AppColors.DarkBlue,
                    uncheckedColor = AppColors.Orange,
                    checkmarkColor = AppColors.Yellow
                ))
                Spacer(Modifier.width(2.dp))
                Text("Створити задачу на основі запиту?")
        }
        Spacer(Modifier.height(8.dp))
//        Row(modifier = Modifier.padding(8.dp)) {
//            TextField(
//                value = userInput,
//                onValueChange = { viewModel.userInput = it },
//                modifier = Modifier.weight(1f)
//            )
//            Button(onClick = { viewModel.sendMessage() }) {
//                Text("Send")
//            }
//        }
    }
}