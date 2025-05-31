package com.example.dyplomproject.ui.screen

import androidx.compose.ui.test.*
import androidx.compose.ui.test.junit4.createAndroidComposeRule
import androidx.navigation.NavHostController
import androidx.test.ext.junit.runners.AndroidJUnit4
import com.example.dyplomproject.MainActivity
import org.junit.Rule
import org.junit.Test
import org.junit.runner.RunWith

//@RunWith(AndroidJUnit4::class)
//class RegistrationScreenTest {
//
//    @get:Rule
//    val composeRule = createAndroidComposeRule<MainActivity>()
//
//    @Test
//    fun registration_fails_whenBirthDateNotSelected_showsErrorMessage() {
//        composeRule.onNodeWithText("Вперше з нами? Зареєструватися!")
//            .performClick()
//
//        val textFields = composeRule.onAllNodes(hasSetTextAction())
//
//        textFields[0].performTextInput("Микита Шевченко")
//
//        composeRule.onNodeWithText("Чоловік").performClick()
//
//        textFields[1].performTextInput("mykita.shevchenko@example.com")
//        textFields[2].performTextInput("StrongPass123")
//        textFields[3].performTextInput("StrongPass123")
//        textFields[4].performTextInput("mykita")
//        textFields[5].performTextInput("Вір у себе!")
//
//        composeRule.onNodeWithText("Зареєструватися").performClick()
//
//        composeRule.onNodeWithText("Дата народження")
//            .performScrollTo()
//
//        composeRule.onAllNodes(hasText("Дата народження"))
//            .filterToOne(hasTextExactly("Дата народження обов'язкова."))
//
//        Thread.sleep(5000)
//
//    }
//
//}

@RunWith(AndroidJUnit4::class)
class RegistrationScreenTest {

    @get:Rule
    val composeRule = createAndroidComposeRule<MainActivity>()

    private fun openRegistrationForm() {
        composeRule.onNodeWithText("Вперше з нами? Зареєструватися!")
            .assertIsDisplayed()
            .performClick()
    }

    private fun fillCommonFields(
        name: String = "Микита Шевченко",
        gender: String = "Чоловік",
        email: String = "mykita.shevchenko@example.com",
        password: String = "StrongPass123",
        confirmPassword: String = "StrongPass123",
        nickname: String = "mykita",
        motto: String = "Вір у себе!"
    ) {
        val textFields = composeRule.onAllNodes(hasSetTextAction())
        textFields[0].performTextInput(name)
        if (gender.isNotEmpty()) {
            composeRule.onNodeWithText(gender).assertIsDisplayed().performClick()
        }
        textFields[1].performTextInput(email)
        textFields[2].performTextInput(password)
        textFields[3].performTextInput(confirmPassword)
        textFields[4].performTextInput(nickname)
        textFields[5].performTextInput(motto)
    }

    @Test
    fun registration_fails_whenNameIsBlank_showsError() {
        openRegistrationForm()
        fillCommonFields(name = "")
        composeRule.onNodeWithText("Зареєструватися").performClick()
        composeRule.onNodeWithText("Ім'я обов’язкове.").assertIsDisplayed()
    }

    @Test
    fun registration_fails_whenGenderNotSelected_showsError() {
        openRegistrationForm()
        val textFields = composeRule.onAllNodes(hasSetTextAction())
        textFields[0].performTextInput("Микита Шевченко")
        // гендер не вибираємо
        textFields[1].performTextInput("mykita.shevchenko@example.com")
        textFields[2].performTextInput("StrongPass123")
        textFields[3].performTextInput("StrongPass123")
        textFields[4].performTextInput("mykita")
        textFields[5].performTextInput("Вір у себе!")

        composeRule.onNodeWithText("Зареєструватися").performClick()
        composeRule.onNodeWithText("Виберіть стать.").assertIsDisplayed()
    }

    @Test
    fun registration_fails_whenEmailIsBlank_showsError() {
        openRegistrationForm()
        fillCommonFields(email = "")
        composeRule.onNodeWithText("Зареєструватися").performClick()
        composeRule.onNodeWithText("Пошта обов’язкова.").assertIsDisplayed()
    }

    @Test
    fun registration_fails_whenEmailInvalid_showsError() {
        openRegistrationForm()
        fillCommonFields(email = "invalid-email")
        composeRule.onNodeWithText("Зареєструватися").performClick()
        composeRule.onNodeWithText("Неправильний формат пошти.").assertIsDisplayed()
    }

    @Test
    fun registration_fails_whenPasswordIsBlank_showsError() {
        openRegistrationForm()
        fillCommonFields(password = "")
        composeRule.onNodeWithText("Зареєструватися").performClick()
        composeRule.onNodeWithText("Пароль обов’язковий.").assertIsDisplayed()
    }

    @Test
    fun registration_fails_whenPasswordTooShort_showsError() {
        openRegistrationForm()
        fillCommonFields(password = "Short1")
        composeRule.onNodeWithText("Зареєструватися").performClick()
        composeRule.onNodeWithText("Пароль має містити щонайменше 8 символів.").assertIsDisplayed()
    }

    @Test
    fun registration_fails_whenPasswordsDoNotMatch_showsError() {
        openRegistrationForm()
        fillCommonFields(password = "StrongPass123", confirmPassword = "WrongPass123")
        composeRule.onNodeWithText("Зареєструватися").performClick()
        composeRule.onNodeWithText("Паролі не співпадають.").assertIsDisplayed()
    }

    @Test
    fun registration_fails_whenNicknameIsBlank_showsError() {
        openRegistrationForm()
        fillCommonFields(nickname = "")
        composeRule.onNodeWithText("Зареєструватися").performClick()
        composeRule.onNodeWithText("Нікнейм обов’язковий.").assertIsDisplayed()
    }

    @Test
    fun registration_fails_whenBirthDateNotSelected_showsError() {
        openRegistrationForm()
        fillCommonFields()
        // Тут не вибираємо дату народження (припускаємо, що є відповідне поле)
        composeRule.onNodeWithText("Зареєструватися").performClick()
        composeRule.onNodeWithText("Дата народження обов'язкова.").assertIsDisplayed()
    }
}