package com.example.dyplomproject.data.utils

import android.content.Context
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.stringPreferencesKey
import androidx.datastore.preferences.preferencesDataStore
import dagger.hilt.android.qualifiers.ApplicationContext
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.map
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
}