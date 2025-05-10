package com.example.dyplomproject.data.remote.request

data class UserRegistrationRequest(
    val fullName: String,
    val nickname: String,
    val gender: String,
    val birthDate: String,
    val email: String,
    val password: String,
    val futureMessage: String
)