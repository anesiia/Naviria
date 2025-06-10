package com.example.dyplomproject.ui.screen

import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import coil.compose.rememberAsyncImagePainter
import com.example.dyplomproject.data.remote.repository.SettingsRepository
import com.example.dyplomproject.data.remote.repository.TaskRepository
import com.example.dyplomproject.utils.RetrofitInstance
import com.example.dyplomproject.ui.theme.AppColors
import com.example.dyplomproject.ui.theme.additionalTypography
import com.example.dyplomproject.ui.viewmodel.SettingsViewModel
import androidx.compose.foundation.layout.size
import androidx.compose.ui.draw.clip

@Composable
fun SettingsScreen(
    navController: NavHostController,
    userId: String,
    padding: PaddingValues
) {
    val context = LocalContext.current
    val repository = remember { SettingsRepository(RetrofitInstance.api) }
    val viewModel: SettingsViewModel = viewModel(factory = object: ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T: ViewModel> create(modelClass: Class<T>) : T {
            if (modelClass.isAssignableFrom(SettingsViewModel::class.java)) {
                return SettingsViewModel(repository, context, userId) as T
            }
            throw IllegalArgumentException("Unknown ViewModel class")
        }
    })

    val uiState by viewModel.uiState.collectAsState()

    val launcher = rememberLauncherForActivityResult(
        contract = ActivityResultContracts.GetContent()
    ) { uri ->
        uri?.let { viewModel.onImageSelected(it) }
    }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .background(AppColors.White)
            .padding(padding)
    ) {
        Text(
            text = "Тут налаштування",
            color = AppColors.Orange,
            style = additionalTypography.exampleText
        )

        Button(onClick = { launcher.launch("image/*") }) {
            Text("Завантажити фото")
        }

        if (uiState.isUploading) {
            CircularProgressIndicator(modifier = Modifier.padding(16.dp))
        }

        uiState.uploadSuccess?.let {
            Text(
                text = if (it) "Фото завантажено успішно!" else "Помилка завантаження.",
                color = if (it) AppColors.MilkBlue else AppColors.Orange
            )
        }

        uiState.selectedImageUri?.let { uri ->
            Image(
                painter = rememberAsyncImagePainter(uri),
                contentDescription = "Selected Image",
                modifier = Modifier
                    .padding(16.dp)
                    .size(120.dp)
                    .clip(RoundedCornerShape(12.dp))
            )
        }
    }
}