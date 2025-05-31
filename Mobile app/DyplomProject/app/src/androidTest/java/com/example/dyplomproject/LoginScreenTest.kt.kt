package com.example.dyplomproject.ui.screen

import androidx.compose.ui.test.*
import androidx.compose.ui.test.junit4.createAndroidComposeRule
import androidx.test.ext.junit.runners.AndroidJUnit4
import com.example.dyplomproject.MainActivity
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith
@RunWith(AndroidJUnit4::class)
class LoginScreenTest {

    @get:Rule
    val composeRule = createAndroidComposeRule<MainActivity>()

//    @Test
//    fun loginWithEmptyFields_showsValidationErrors() {
//        // Натискаємо "Увійти" без заповнення полів
//        composeRule.onNodeWithText("Увійти").performClick()
//
//        // Чекаємо появу повідомлень про помилки
//        composeRule.waitUntil(timeoutMillis = 3_000) {
//            composeRule.onAllNodesWithText("Пошта обов’язкова.").fetchSemanticsNodes().isNotEmpty()
//        }
//
//        composeRule.onNodeWithText("Пошта обов’язкова.").assertIsDisplayed()
//        composeRule.onNodeWithText("Пароль обов’язковий.").assertIsDisplayed()
//    }

    @Test
    fun loginRealUser_works_correctly() {
        val textFields = composeRule.onAllNodes(hasSetTextAction())

        // Вводимо email
        textFields[0].performClick()
        textFields[0].performTextInput("maria@example.com")

        // Вводимо пароль
        textFields[1].performClick()
        textFields[1].performTextInput("Maria1234")

        // Натискаємо кнопку "Увійти"
        composeRule.onNodeWithText("Увійти").performClick()
    }

}
