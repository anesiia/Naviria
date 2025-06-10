package com.example.dyplomproject.utils

import android.content.Context
import android.util.Base64
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.stringPreferencesKey
import androidx.datastore.preferences.preferencesDataStore
import dagger.hilt.android.qualifiers.ApplicationContext
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.map
import org.json.JSONObject
import javax.inject.Inject

class DataStoreManager @Inject constructor(@ApplicationContext context: Context) {
    private val Context.dataStore by preferencesDataStore("auth_prefs")
    private val dataStore = context.dataStore

    companion object {
        val AUTH_TOKEN = stringPreferencesKey("auth_token")
    }

    suspend fun saveAuthToken(token: String) {
        dataStore.edit { prefs ->
            prefs[AUTH_TOKEN] = token
        }
    }

    suspend fun clearAuthToken() {
        dataStore.edit { prefs ->
            prefs.remove(AUTH_TOKEN)
        }
    }

    val authTokenFlow: Flow<String?> = dataStore.data
        .map { it[AUTH_TOKEN] }
///////////////////////////////////////////////
    val userIdFlow: Flow<String?> = authTokenFlow
        .map { token -> token?.let { decodeUserId(it) } }

    private fun decodeUserId(token: String): String? {
        return try {
            val payload = token.split(".")[1]
            val decoded = Base64.decode(payload, Base64.URL_SAFE or Base64.NO_WRAP)
            val json = JSONObject(String(decoded, Charsets.UTF_8))
            json.optString("sub")
        } catch (e: Exception) {
            null
        }
    }
}