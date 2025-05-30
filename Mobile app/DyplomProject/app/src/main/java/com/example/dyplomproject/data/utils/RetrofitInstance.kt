package com.example.dyplomproject.data.utils

import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.SubtaskCreateDto
import com.example.dyplomproject.data.remote.SubtaskDto
import com.example.dyplomproject.data.remote.SubtaskRepeatableCreateDto
import com.example.dyplomproject.data.remote.SubtaskRepeatableDto
import com.example.dyplomproject.data.remote.SubtaskScaleCreateDto
import com.example.dyplomproject.data.remote.SubtaskScaleDto
import com.example.dyplomproject.data.remote.SubtaskStandardCreateDto
import com.example.dyplomproject.data.remote.SubtaskStandardDto
import com.example.dyplomproject.data.remote.TaskCreateDto
import com.example.dyplomproject.data.remote.TaskDto
import com.example.dyplomproject.data.remote.TaskRepeatableCreateDto
import com.example.dyplomproject.data.remote.TaskRepeatableDto
import com.example.dyplomproject.data.remote.TaskScaleCreateDto
import com.example.dyplomproject.data.remote.TaskScaleDto
import com.example.dyplomproject.data.remote.TaskStandardCreateDto
import com.example.dyplomproject.data.remote.TaskStandardDto
import com.example.dyplomproject.data.remote.TaskWithSubtasksCreateDto
import com.example.dyplomproject.data.remote.TaskWithSubtasksDto
import com.example.dyplomproject.ui.screen.Subtask
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.security.SecureRandom
import java.security.cert.X509Certificate
import javax.net.ssl.SSLContext
import javax.net.ssl.TrustManager
import javax.net.ssl.X509TrustManager

object RetrofitInstance {
//
//    private const val BASE_URL = "http://10.0.2.2:5186/"//"http://192.168.1.9:5186"// Lisa's URL// "https://10.0.2.2:7172/"
//    //private const val BASE_URL = "http://...:5186" //Mariam's URL
////    private val unsafeOkHttpClient: OkHttpClient by lazy {
////        val trustAllCerts = arrayOf<TrustManager>(
////            object : X509TrustManager {
////                override fun checkClientTrusted(chain: Array<out X509Certificate>?, authType: String?) {}
////                override fun checkServerTrusted(chain: Array<out X509Certificate>?, authType: String?) {}
////                override fun getAcceptedIssuers(): Array<X509Certificate> = arrayOf()
////            }
////        )
////
////        val sslContext = SSLContext.getInstance("SSL")
////        sslContext.init(null, trustAllCerts, SecureRandom())
////
////        OkHttpClient.Builder()
////            .sslSocketFactory(sslContext.socketFactory, trustAllCerts[0] as X509TrustManager)
////            .hostnameVerifier { _, _ -> true } // ⚠️ НЕ ИСПОЛЬЗУЙ в продакшене
////            .build()
////    }
//
//    val loggingInterceptor = HttpLoggingInterceptor().apply {
//        level = HttpLoggingInterceptor.Level.BODY // Logs request/response lines AND bodies
//    }
//
//    val httpClient = OkHttpClient.Builder()
//        .addInterceptor(loggingInterceptor)
//        .build()
//
////    val retrofit = Retrofit.Builder()
////        .baseUrl("https://your-api-base-url.com/")
////        .client(httpClient)
////        .addConverterFactory(GsonConverterFactory.create())
////        .build()
//
//    val api: ApiService by lazy {
////        Retrofit.Builder()
////            .baseUrl(BASE_URL)
////            .client(unsafeOkHttpClient)
////            .addConverterFactory(GsonConverterFactory.create())
////            .build()
////            .create(ApiService::class.java)
//        Retrofit.Builder()
//            .baseUrl(BASE_URL)
//            .client(httpClient)
//            .addConverterFactory(GsonConverterFactory.create())
//            .build()
//            .create(ApiService::class.java)
//    }

    private const val BASE_URL = "http://10.0.2.2:5186/"
        //.baseUrl("http://10.0.2.2:5186/") //Mariam's URL emulator
        //.baseUrl("http://192.168.56.1:5186/")//Mariam's URL Physical device
        //.baseUrl("http://192.168.1.9:5186/") //Lisa's URL
    //.baseUrl("http://10.0.2.2:5186/") //Lisa's URL


    private val loggingInterceptor = HttpLoggingInterceptor().apply {
        level = HttpLoggingInterceptor.Level.BODY
    }

    private val httpClient = OkHttpClient.Builder()
        .addInterceptor(loggingInterceptor)
        .build()


    private val taskCreateAdapterFactory = RuntimeTypeAdapterFactory
        .of(TaskCreateDto::class.java, "type")
        .registerSubtype(TaskStandardCreateDto::class.java, "standard")
        .registerSubtype(TaskRepeatableCreateDto::class.java, "repeatable")
        .registerSubtype(TaskScaleCreateDto::class.java, "scale")
        .registerSubtype(TaskWithSubtasksCreateDto::class.java, "with_subtasks")

    val taskDtoAdapterFactory = RuntimeTypeAdapterFactory
        .of(TaskDto::class.java, "type")
        .registerSubtype(TaskStandardDto::class.java, "standard")
        .registerSubtype(TaskRepeatableDto::class.java, "repeatable")
        .registerSubtype(TaskScaleDto::class.java, "scale")
        .registerSubtype(TaskWithSubtasksDto::class.java, "with_subtasks")

    val subtaskCreateDtoAdapterFactory = RuntimeTypeAdapterFactory
        .of(SubtaskCreateDto::class.java, "subtask_type")
        .registerSubtype(SubtaskStandardCreateDto::class.java, "standard")
        .registerSubtype(SubtaskRepeatableCreateDto::class.java, "repeatable")
        .registerSubtype(SubtaskScaleCreateDto::class.java, "scale")

    val subtaskDtoAdapterFactory = RuntimeTypeAdapterFactory
        .of(SubtaskDto::class.java, "type")
        .registerSubtype(SubtaskStandardDto::class.java, "standard")
        .registerSubtype(SubtaskRepeatableDto::class.java, "repeatable")
        .registerSubtype(SubtaskScaleDto::class.java, "scale")


    private val gson: Gson = GsonBuilder()
        .registerTypeAdapterFactory(taskCreateAdapterFactory)
        .registerTypeAdapterFactory(taskDtoAdapterFactory)
        .registerTypeAdapterFactory(subtaskCreateDtoAdapterFactory)
        .registerTypeAdapterFactory(subtaskDtoAdapterFactory)
        .create()


    val api: ApiService by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .client(httpClient)
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build()
            .create(ApiService::class.java)
    }
}