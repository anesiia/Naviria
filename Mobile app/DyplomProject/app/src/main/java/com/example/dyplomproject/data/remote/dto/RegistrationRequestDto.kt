package com.example.dyplomproject.data.remote.dto

data class RegistrationRequestDto(
    val fullName: String,
    val nickname: String,
    val gender: String,
    val birthDate: String,
    val email: String,
    val password: String,
    val futureMessage: String
)