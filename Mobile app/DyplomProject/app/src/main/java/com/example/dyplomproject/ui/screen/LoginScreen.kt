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
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
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
import com.example.dyplomproject.ui.components.PrimaryButton
import com.example.dyplomproject.ui.viewmodel.AuthViewModel

@Composable
fun LoginScreen(
    navController: NavHostController,
    loginViewModel: LoginViewModel,
    authViewModel: AuthViewModel
) {
    val email by loginViewModel.email.collectAsState()
    val password by loginViewModel.password.collectAsState()
    val error by loginViewModel.error.collectAsState()
    //val rememberMe by viewModel.rememberMe.collectAsState()
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
        // Background Image
        Image(
            painter = painterResource(id = R.drawable.login_background),
            contentDescription = null,
            contentScale = ContentScale.Crop,
            modifier = Modifier.fillMaxSize()
        )

        // Foreground Content (your login UI)
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(24.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            // Your existing UI code here...
            OutlinedTextField(
                value = email,
                onValueChange = loginViewModel::onEmailChanged,
                label = { Text("Email") },
                modifier = Modifier.fillMaxWidth()
            )

            Spacer(modifier = Modifier.height(16.dp))

            OutlinedTextField(
                value = password,
                onValueChange = loginViewModel::onPasswordChanged,
                label = { Text("Password") },
                modifier = Modifier.fillMaxWidth(),
                visualTransformation = PasswordVisualTransformation()
            )

            Spacer(modifier = Modifier.height(16.dp))

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
                modifier = Modifier.clickable {
                    //viewModel.onForgotPasswordClicked()
                    navController.navigate("register")
                }
            )
            if (error != null) {
                Spacer(modifier = Modifier.height(16.dp))

                // Display the error message as Text or SnackBar
                Text(
                    text = error!!,
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

        // Foreground Content (your login UI)
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(24.dp),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            // Your existing UI code here...
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



//            Row(
//                verticalAlignment = Alignment.CenterVertically,
//                modifier = Modifier.fillMaxWidth()
//            ) {
//                RadioButton(
//                    selected = rememberMe,
//                    //onClick = { viewModel.onRememberMeChanged(!rememberMe) },
//                    colors = RadioButtonDefaults.colors(
//                        selectedColor = Color.Blue,
//                        unselectedColor = Color.Gray
//                    ),
//                    modifier = Modifier.size(24.dp)
//                )
//                Spacer(modifier = Modifier.width(8.dp))
//                Text(
//                    text = "Remember Me",
////                    modifier = Modifier.clickable {
////                        viewModel.onRememberMeChanged(!rememberMe)
////                    }
//                )
//            }

//            Button(
//                onClick = ,
//                modifier = Modifier
//                    .fillMaxWidth()
//                    .height(50.dp),
//                shape = RoundedCornerShape(10.dp)
//            ) {
//                Text("Увійти")
//            }