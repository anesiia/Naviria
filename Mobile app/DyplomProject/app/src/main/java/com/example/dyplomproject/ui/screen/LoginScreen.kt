package com.example.dyplomproject.ui.screen

import androidx.compose.foundation.Image
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.wrapContentSize
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.dyplomproject.R
import com.example.dyplomproject.ui.viewmodel.LoginViewModel
import androidx.navigation.NavHostController
import com.example.dyplomproject.ui.components.LabeledTextField
import com.example.dyplomproject.ui.components.PrimaryButton
import com.example.dyplomproject.ui.components.UnderlinedText
import com.example.dyplomproject.ui.viewmodel.AuthViewModel

@Composable
fun LoginScreen(
    navController: NavHostController,
    loginViewModel: LoginViewModel,
    authViewModel: AuthViewModel
) {
    val uiState by loginViewModel.uiState.collectAsState()
    LaunchedEffect(authViewModel.isAuthenticated.collectAsState().value) {
        if (authViewModel.isAuthenticated.value) {
            navController.navigate("main") {
                popUpTo("auth") { inclusive = true }  // Clear auth screens from backstack
            }
        }
    }
    var passwordVisible by remember { mutableStateOf(false) }
    Box(
        modifier = Modifier.fillMaxSize()
    ) {
        Image(
            painter = painterResource(id = R.drawable.login_background),
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
                text = "Увійти в акаунт", useGradient = false
            )
            Spacer(modifier = Modifier.height(48.dp))

            LabeledTextField(
                label = "Пошта",
                value = uiState.email,
                onValueChange = { loginViewModel.updateField("email", it) },
                placeholder = "example@gmail.com",
                fieldKey = "email",
                fieldErrors = uiState.fieldErrors
            )

            LabeledTextField(
                label = "Пароль",
                value = uiState.password,
                onValueChange = {  loginViewModel.updateField("password", it) },
                placeholder = "*******",
                fieldKey = "password",
                fieldErrors = uiState.fieldErrors,
                isPassword = true,
                isPasswordVisible = passwordVisible,
                onPasswordToggleClick = { passwordVisible = !passwordVisible }
            )
            Spacer(modifier = Modifier.height(24.dp))

            PrimaryButton(
                imageRes = R.drawable.default_button_bg,
                text = "Увійти",
                onClick = { loginViewModel.onLoginClicked(authViewModel) }
            )

            Spacer(modifier = Modifier.height(8.dp))

            Text(
                text = "Вперше з нами? Зареєструватися!",
                //color = Color.Blue,
                modifier = Modifier
                    .align(Alignment.CenterHorizontally)
                    .clickable {
                    //viewModel.onForgotPasswordClicked()
                    navController.navigate("register")
                }
            )

            if (uiState.isLoading) {
                CircularProgressIndicator(
                    modifier = Modifier.align(Alignment.CenterHorizontally)
                )
            }

            if (uiState.error != null) {
                Spacer(modifier = Modifier.height(8.dp))
                Text(
                    text = uiState.error ?: "",
                    color = Color.Red,
                    style = MaterialTheme.typography.bodyLarge
                )
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun LoginScreenPreview() {
    var email = "";
    var password = "";
    Box(
        modifier = Modifier.fillMaxSize()
    ) {
        // Background Image
        Image(
            painter = painterResource(id = R.drawable.login_background),
            contentDescription = null,
            contentScale = ContentScale.Crop,
            modifier = Modifier.fillMaxSize()
        )

        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(24.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            OutlinedTextField(
                value = email,
                onValueChange = {},
                label = { Text("Email") },
                modifier = Modifier.fillMaxWidth()
            )

            Spacer(modifier = Modifier.height(16.dp))

            OutlinedTextField(
                value = password,
                onValueChange = {},
                label = { Text("Password") },
                modifier = Modifier.fillMaxWidth(),
                visualTransformation = PasswordVisualTransformation()
            )

            Spacer(modifier = Modifier.height(16.dp))

            Spacer(modifier = Modifier.height(24.dp))

            PrimaryButton(
                imageRes = R.drawable.default_button_bg,
                text = "Увійти",
                onClick = {  }
            )

            Spacer(modifier = Modifier.height(8.dp))

            Text(
                text = "Вперше з нами? Зареєструватися!",
                //color = Color.Blue,
                modifier = Modifier.clickable {
                    //viewModel.onForgotPasswordClicked()
                }
            )
        }
    }
}