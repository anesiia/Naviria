package com.example.dyplomproject.mappers


import com.example.dyplomproject.data.remote.FolderDto
import com.example.dyplomproject.data.remote.dto.FriendShortDto
import com.example.dyplomproject.data.remote.IncomingFriendRequestDto
import com.example.dyplomproject.data.remote.Tag
import com.example.dyplomproject.data.remote.dto.UserDto
import com.example.dyplomproject.ui.viewmodel.Folder
import com.example.dyplomproject.ui.viewmodel.IncomingRequestUiModel
import com.example.dyplomproject.ui.viewmodel.UserShortUiModel

fun FriendShortDto.toUiModel(): UserShortUiModel = UserShortUiModel(
    id = this.userId,
    fullName = "", // You could enrich this with a user lookup later
    nickname = this.nickname,
    isOnline = false,
    isProUser = false,
    isRequestSent = false,
    photo = ""
)

fun UserDto.toUiModel(): UserShortUiModel = UserShortUiModel(
    id = this.id,
    fullName = this.fullName,
    nickname = this.nickname,
    isOnline = this.isOnline,
    isProUser = this.isProUser,
    isRequestSent = false,
    photo = photo
)

fun IncomingFriendRequestDto.toUiModel(): IncomingRequestUiModel {
    return IncomingRequestUiModel(
        requestId = request.id,
        fromUserId = request.fromUserId,
        toUserId = request.toUserId,
        status = request.status,
        sender = sender.toUiModel()
    )
}

fun FolderDto.toUiModel(): Folder {
    return Folder(
        id = this.id,
        name = this.name
    )
}

fun List<String>.toTagList(): List<Tag> = map { Tag(it) }

fun List<Tag>.toStringList(): List<String> = map { it.TagName }