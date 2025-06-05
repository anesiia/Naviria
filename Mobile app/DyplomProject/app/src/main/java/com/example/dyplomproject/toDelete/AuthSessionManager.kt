package com.example.dyplomproject.toDelete
//
//class AuthSessionManager {
//}@Singleton
//class AuthSessionManager @Inject constructor(
//    private val dataStoreManager: DataStoreManager
//) {
//    val userIdFlow: Flow<String?> = dataStoreManager.authTokenFlow
//        .map { token -> token?.let { decodeUserId(it) } }
//
//    private fun decodeUserId(token: String): String? {
//        return try {
//            val payload = token.split(".")[1]
//            val decodedBytes = Base64.decode(payload, Base64.URL_SAFE or Base64.NO_WRAP)
//            val json = JSONObject(String(decodedBytes, Charsets.UTF_8))
//            json.optString("sub")
//        } catch (e: Exception) {
//            null
//        }
//    }
//}
//🔹 Inject and Use in Any ViewModel
//kotlin
//Копировать
//Редактировать
//@HiltViewModel
//class TasksViewModel @Inject constructor(
//    private val authSessionManager: AuthSessionManager
//) : ViewModel() {
//
//    val userId = authSessionManager.userIdFlow
//        .stateIn(viewModelScope, SharingStarted.WhileSubscribed(5_000), null)
//
//    fun loadUserFolders() {
//        viewModelScope.launch {
//            userId.value?.let {
//                // fetch folders with it
//            }
//        }
//    }
//}