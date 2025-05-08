package com.example.dyplomproject.ui.screen

import android.app.DatePickerDialog
import android.content.Context
import android.util.Log
import androidx.compose.runtime.Composable
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.focusable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.Divider
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.RadioButtonChecked
import androidx.compose.material.icons.filled.RadioButtonUnchecked
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.material3.TextFieldColors
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.material3.Typography
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.focus.FocusRequester
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.dyplomproject.R
import androidx.navigation.NavHostController
import com.example.dyplomproject.AuthViewModelFactory
import com.example.dyplomproject.data.remote.AuthRepository
import com.example.dyplomproject.data.remote.UserRepository
import com.example.dyplomproject.data.utils.DataStoreManager
import com.example.dyplomproject.data.utils.RetrofitInstance
import com.example.dyplomproject.ui.components.GenderRadioButton
import com.example.dyplomproject.ui.components.PrimaryButton
import com.example.dyplomproject.ui.components.UnderlinedText
import com.example.dyplomproject.ui.theme.DyplomProjectTheme
import com.example.dyplomproject.ui.theme.addtionalTypography
import com.example.dyplomproject.ui.viewmodel.AuthViewModel
import com.example.dyplomproject.ui.viewmodel.FriendsViewModel
import com.example.dyplomproject.ui.viewmodel.RegistrationViewModel
import java.time.LocalDate
import java.time.format.DateTimeFormatter
import java.util.Calendar

@Composable
fun primaryTextFieldColors(): TextFieldColors {
    return TextFieldDefaults.colors(
        disabledTextColor = Color(0xFF023047),
        disabledLabelColor = Color(0xFFF0F3F6),
        focusedContainerColor = Color(0xFFF0F3F6),
        unfocusedContainerColor = Color(0xFFF0F3F6),// background
        focusedPlaceholderColor = Color(0x99023047),
        unfocusedPlaceholderColor = Color(0x99023047), // placeholder before typing
        focusedTextColor = Color(0xFF023047), //
        unfocusedTextColor = Color(0xFF023047),//0x99023047
        focusedIndicatorColor = Color(0xFFFF9800), // line of the textFieled
        unfocusedIndicatorColor = Color(0x33023047)
    )
}

@Composable
fun RegistrationScreen(
    navController: NavHostController,
    authViewModel: AuthViewModel,
) {
//    var email by remember { mutableStateOf("") }
//    var password by remember { mutableStateOf("") }
//    var confirmPassword by remember { mutableStateOf("") }
//    var error by remember { mutableStateOf<String?>(null) }
    val repository = remember { AuthRepository(RetrofitInstance.api) }
    val registrationViewModel: RegistrationViewModel = viewModel(
        factory = object : ViewModelProvider.Factory {
            @Suppress("UNCHECKED_CAST")
            override fun <T : ViewModel> create(modelClass: Class<T>): T {
                if (modelClass.isAssignableFrom(RegistrationViewModel::class.java)) {
                    return RegistrationViewModel(repository) as T
                }
                throw IllegalArgumentException("Unknown ViewModel class")
            }
        }
    )
    val uiState by registrationViewModel.uiState.collectAsState()
    LaunchedEffect(authViewModel.isAuthenticated.collectAsState().value) {
        if (authViewModel.isAuthenticated.value) {
            navController.navigate("main") {
                popUpTo("auth") { inclusive = true }  // Clear auth screens from backstack
            }
        }
    }


    Box(
        modifier = Modifier.fillMaxSize()
    ) {
        Image(
            painter = painterResource(id = R.drawable.registeration_background),
            contentDescription = null,
            contentScale = ContentScale.Crop,
            modifier = Modifier.fillMaxSize()
        )
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(24.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.Start
        ) {

            UnderlinedText(
                text = "Реєстрація", useGradient = false
            )
            Spacer(modifier = Modifier.height(32.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Ім’я та прізвище",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))

            TextField(
                value = uiState.fullName,
                onValueChange = { registrationViewModel.updateField("fullName", it) },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("Микита Шевченко", style = addtionalTypography.exampleText)
                },
                colors = primaryTextFieldColors()
            )
            Spacer(modifier = Modifier.height(16.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Дата народження",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))

            BirthDatePickerField(selectedDate = uiState.birthDate, onDateSelected = {
                Log.d("RegistrationScreen", "Selected Date: $it")
                registrationViewModel.updateBirthDate(it)
            })
            Spacer(modifier = Modifier.height(16.dp))

            Text("Стать", style = MaterialTheme.typography.bodyLarge)
            Spacer(modifier = Modifier.height(4.dp))

            Row(verticalAlignment = Alignment.CenterVertically) {
                GenderRadioButton(selected = uiState.gender == "f",
                    text = "Жінка",
                    onClick = { registrationViewModel.updateField("gender", "f") })
                Spacer(modifier = Modifier.width(64.dp))
                GenderRadioButton(selected = uiState.gender == "m",
                    text = "Чоловік",
                    onClick = { registrationViewModel.updateField("gender", "m") })
            }
            Spacer(modifier = Modifier.height(16.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Пошта",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))
            TextField(
                value = uiState.email,
                onValueChange = { registrationViewModel.updateField("email", it) },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("example@gmail.com", style = addtionalTypography.exampleText)
                },
                colors = primaryTextFieldColors()
            )
            Spacer(modifier = Modifier.height(16.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Пароль",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))

            TextField(
                value = uiState.password,
                onValueChange = { registrationViewModel.updateField("password", it) },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("*******", style = addtionalTypography.exampleText)
                },
                colors = primaryTextFieldColors(),
                visualTransformation = PasswordVisualTransformation()
            )

            Text(
                color = Color(0xFF1B2B3A),
                text = "Підтвердіть пароль",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))
            TextField(
                value = uiState.confirmPassword,
                onValueChange = { registrationViewModel.updateField("confirmPassword", it) },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("*******", style = addtionalTypography.exampleText)
                },
                colors = primaryTextFieldColors(),
                visualTransformation = PasswordVisualTransformation()
            )
            Spacer(modifier = Modifier.height(16.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Придумай нікнейм",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))

            TextField(
                value = uiState.nickname,
                onValueChange = { registrationViewModel.updateField("nickname", it) },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("Нікнейм", style = addtionalTypography.exampleText)
                },
                colors = primaryTextFieldColors()
            )
            Spacer(modifier = Modifier.height(16.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Придумай смс для Себе майбутнього!",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))

            TextField(
                value = uiState.futureMessage,
                onValueChange = { registrationViewModel.updateField("futureMessage", it) },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("Текст смс", style = addtionalTypography.exampleText)
                },
                colors = primaryTextFieldColors()
            )
            Spacer(modifier = Modifier.height(16.dp))

            PrimaryButton(
                imageRes = R.drawable.default_button_bg,
                text = "Зареєструватися",
                onClick = {registrationViewModel.register(authViewModel)}
//                onClick = {
//                    if (password != confirmPassword) {
//                        error = "Passwords do not match"
//                    } else {
//                        // Call register logic here, then navigate
//                        // For example:
//                        // authViewModel.registerUser(email, password)
//                        navController.navigate("login") {
//                            popUpTo("register") { inclusive = true }
//                        }
//                    }
//                }
            )

            Spacer(modifier = Modifier.height(8.dp))
            Text(
                text = "Вже маєте акаунт? Увійти!",
                modifier = Modifier.align(Alignment.CenterHorizontally).clickable {
                    navController.navigate("login") {
                        popUpTo("register") { inclusive = true }
                    }
                }
            )

//            if (uiState.isLoading) {
//                CircularProgressIndicator()
//            }

            if (uiState.error != null) {
                Text(
                    text = uiState.error ?: "",
                    color = Color.Red,
                    style = MaterialTheme.typography.bodyLarge,
                    modifier = Modifier.align(Alignment.CenterHorizontally)
                )
            }
        }
    }
}



@Preview(showBackground = true)
@Composable
fun RegistrationScreenPreview() {
    var email by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var confirmPassword by remember { mutableStateOf("") }
    var error by remember { mutableStateOf<String?>(null) }
    var test by remember { mutableStateOf("") }
    val selectedGender = remember { mutableStateOf("Жінка") }
    val dob = remember { mutableStateOf("") }

    val textFieldColor = TextFieldDefaults.colors(
        disabledTextColor = Color(0xFF7A8B99),
        focusedContainerColor = Color(0xFFF0F3F6),
        unfocusedContainerColor = Color(0xFFF0F3F6),// your desired background
        focusedPlaceholderColor = Color(0x99023047), // 60% opacity
        unfocusedPlaceholderColor = Color(0x99023047), // placeholder before typing
        focusedTextColor = Color(0xFF023047), // 100% when typing
        unfocusedTextColor = Color(0x99023047),
        focusedIndicatorColor = Color(0xFFFF9800),       // or Color.Transparent to hide
        unfocusedIndicatorColor = Color(0x33023047)
    )
    Box(
        modifier = Modifier.fillMaxSize()
    ) {
        Image(
            painter = painterResource(id = R.drawable.registeration_background),
            contentDescription = null,
            contentScale = ContentScale.Crop,
            modifier = Modifier.fillMaxSize()
        )
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(24.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.Start
        ) {
            UnderlinedText(
                text = "Реєстрація", useGradient = false
            )
            Spacer(modifier = Modifier.height(32.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Ім’я та прізвище",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))
            TextField(
                value = test,
                onValueChange = { test = it },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("Микита Шевченко", style = addtionalTypography.exampleText)
                },
                colors = textFieldColor
            )
            Spacer(modifier = Modifier.height(16.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Дата народження",
                style = MaterialTheme.typography.bodyLarge
            )


            Text("Стать", style = MaterialTheme.typography.bodyLarge)
            Spacer(modifier = Modifier.height(4.dp))
            Row(verticalAlignment = Alignment.CenterVertically) {
                GenderRadioButton(selected = selectedGender.value == "Жінка",
                    text = "Жінка",
                    onClick = { selectedGender.value = "Жінка" })
                Spacer(modifier = Modifier.width(64.dp))
                GenderRadioButton(selected = selectedGender.value == "Чоловік",
                    text = "Чоловік",
                    onClick = { selectedGender.value = "Чоловік" })
            }
            Spacer(modifier = Modifier.height(16.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Пошта",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))
            TextField(
                value = email,
                onValueChange = { email = it },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("example@gmail.com", style = addtionalTypography.exampleText)
                },
                colors = textFieldColor
            )
            Spacer(modifier = Modifier.height(16.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Пароль",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))
            TextField(
                value = password,
                onValueChange = { password = it },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("*******", style = addtionalTypography.exampleText)
                },
                colors = textFieldColor,
                visualTransformation = PasswordVisualTransformation()

            )
            Spacer(modifier = Modifier.height(16.dp))

            Text(
                color = Color(0xFF1B2B3A),
                text = "Підтвердіть пароль",
                style = MaterialTheme.typography.bodyLarge
            )
            Spacer(modifier = Modifier.height(4.dp))
            TextField(
                value = confirmPassword,
                onValueChange = { confirmPassword = it },
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(
                        RoundedCornerShape(10.dp)
                    ),
                placeholder = {
                    Text("*******", style = addtionalTypography.exampleText)
                },
                colors = textFieldColor,
                visualTransformation = PasswordVisualTransformation()
            )
            Spacer(modifier = Modifier.height(16.dp))

            PrimaryButton(imageRes = R.drawable.default_button_bg,
                text = "Зареєструватися",
                onClick = { })

            Spacer(modifier = Modifier.height(8.dp))

            Text(
                text = "Вже маєте акаунт? Увійти!",
                modifier = Modifier.align(Alignment.CenterHorizontally)
            )

        }
    }
}

@Preview(showBackground = true)
@Composable
fun RegistrationScreenRunningPreview() {
    DyplomProjectTheme {
        RegistrationScreenPreview()
    }
}

fun showDatePicker(
    context: Context, initialDate: LocalDate?, onDateSelected: (LocalDate) -> Unit
) {
    Log.d("showDatePicker", "Initializing DatePickerDialog...")
    val calendar = Calendar.getInstance()
    initialDate?.let {
        calendar.set(it.year, it.monthValue - 1, it.dayOfMonth)
    }

    val datePicker = DatePickerDialog(
        context,
        { _, year, month, day ->
            onDateSelected(LocalDate.of(year, month + 1, day))
        },
        calendar.get(Calendar.YEAR),
        calendar.get(Calendar.MONTH),
        calendar.get(Calendar.DAY_OF_MONTH)
    )
    Log.d("showDatePicker", "Showing DatePickerDialog...")
    datePicker.show()
    datePicker.datePicker.maxDate = System.currentTimeMillis()
    datePicker.show()
}


@Composable
fun BirthDatePickerField(
    selectedDate: LocalDate?, onDateSelected: (LocalDate) -> Unit
) {
    val context = LocalContext.current
    val formatter = DateTimeFormatter.ofPattern("dd/MM/yyyy")
    val displayDate = selectedDate?.format(formatter) ?: ""

    TextField(value = displayDate, onValueChange = {}, // readOnly
        modifier = Modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(10.dp))
            .clickable {
                Log.d("BirthDatePickerField", "TextField clicked!")
                showDatePicker(context, selectedDate, onDateSelected)
            },
        placeholder = {
            Text("дд/мм/рррр", style = addtionalTypography.exampleText)
        },
        readOnly = true,
        enabled = false,
        colors = primaryTextFieldColors()
    )
}
