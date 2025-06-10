package com.example.dyplomproject.utils

import com.example.dyplomproject.data.remote.ApiService
import com.example.dyplomproject.data.remote.dto.SubtaskCreateDto
import com.example.dyplomproject.data.remote.dto.SubtaskDto
import com.example.dyplomproject.data.remote.dto.SubtaskRepeatableCreateDto
import com.example.dyplomproject.data.remote.dto.SubtaskRepeatableDto
import com.example.dyplomproject.data.remote.dto.SubtaskScaleCreateDto
import com.example.dyplomproject.data.remote.dto.SubtaskScaleDto
import com.example.dyplomproject.data.remote.dto.SubtaskStandardCreateDto
import com.example.dyplomproject.data.remote.dto.SubtaskStandardDto
import com.example.dyplomproject.data.remote.dto.TaskCreateDto
import com.example.dyplomproject.data.remote.dto.TaskDto
import com.example.dyplomproject.data.remote.dto.TaskRepeatableCreateDto
import com.example.dyplomproject.data.remote.dto.TaskRepeatableDto
import com.example.dyplomproject.data.remote.dto.TaskScaleCreateDto
import com.example.dyplomproject.data.remote.dto.TaskScaleDto
import com.example.dyplomproject.data.remote.dto.TaskStandardCreateDto
import com.example.dyplomproject.data.remote.dto.TaskStandardDto
import com.example.dyplomproject.data.remote.dto.TaskWithSubtasksCreateDto
import com.example.dyplomproject.data.remote.dto.TaskWithSubtasksDto
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

object RetrofitInstance {
    private const val BASE_URL = "http://10.0.2.2:5186"//"http://192.168.1.9:5186/"//
    private val loggingInterceptor = HttpLoggingInterceptor().apply {
        level = HttpLoggingInterceptor.Level.BODY
    }

    private val httpClient = OkHttpClient.Builder()
        .addInterceptor(loggingInterceptor)
        .build()


    private val taskCreateAdapterFactory = RuntimeTypeAdapterFactory.of(
        TaskCreateDto::class.java,
        "type"
    )
        .registerSubtype(TaskStandardCreateDto::class.java, "standard")
        .registerSubtype(TaskRepeatableCreateDto::class.java, "repeatable")
        .registerSubtype(TaskScaleCreateDto::class.java, "scale")
        .registerSubtype(TaskWithSubtasksCreateDto::class.java, "with_subtasks")

    val taskDtoAdapterFactory = RuntimeTypeAdapterFactory.of(TaskDto::class.java, "type")
        .registerSubtype(TaskStandardDto::class.java, "standard")
        .registerSubtype(TaskRepeatableDto::class.java, "repeatable")
        .registerSubtype(TaskScaleDto::class.java, "scale")
        .registerSubtype(TaskWithSubtasksDto::class.java, "with_subtasks")

    val subtaskCreateDtoAdapterFactory = RuntimeTypeAdapterFactory.of(
        SubtaskCreateDto::class.java,
        "subtask_type"
    )
        .registerSubtype(SubtaskStandardCreateDto::class.java, "standard")
        .registerSubtype(SubtaskRepeatableCreateDto::class.java, "repeatable")
        .registerSubtype(SubtaskScaleCreateDto::class.java, "scale")

    val subtaskDtoAdapterFactory = RuntimeTypeAdapterFactory.of(SubtaskDto::class.java, "type")
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