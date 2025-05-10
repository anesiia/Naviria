//package com.example.dyplomproject.toDelete
//
////import com.dyplomproject.data.AuthRepository
////import com.dyplomproject.network.ApiService
//import android.content.Context
//import com.example.dyplomproject.data.utils.DataStoreManager
//import com.example.dyplomproject.data.remote.ApiService
//import dagger.Module
//import dagger.Provides
//import dagger.hilt.InstallIn
//import dagger.hilt.android.qualifiers.ApplicationContext
//import dagger.hilt.components.SingletonComponent
//import retrofit2.Retrofit
//import retrofit2.converter.gson.GsonConverterFactory
//import javax.inject.Singleton
//
//@Module
//@InstallIn(SingletonComponent::class)
//object AppModule {
//    @Provides
//    @Singleton
//    fun provideDataStoreManager(
//        @ApplicationContext context: Context
//    ): DataStoreManager {
//        return DataStoreManager(context)
//    }
//
//    @Provides
//    @Singleton
//    fun provideRetrofit(): Retrofit {
//        return Retrofit.Builder()
//            .baseUrl("https://localhost:7172/") // replace with your base URL
//            .addConverterFactory(GsonConverterFactory.create())
//            .build()
//    }
//
//    @Provides
//    @Singleton
//    fun provideApiService(retrofit: Retrofit): ApiService {
//        return retrofit.create(ApiService::class.java)
//    }
//}


