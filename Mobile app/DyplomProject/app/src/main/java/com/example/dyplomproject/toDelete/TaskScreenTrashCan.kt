package com.example.dyplomproject.toDelete


//        FolderTabs(
//            folders = uiState.folders,
//            selectedFolder = uiState.selectedFolder,
//            onFolderSelected = viewModel::selectFolder,
//            onAddFolder = {
//                val newFolder = Folder(UUID.randomUUID().toString(), "New Folder ${uiState.folders.size + 1}")
//                viewModel.addFolder(newFolder)
//            },
//            onEditFolder = { editingFolder = it },
//            onDeleteFolder = { viewModel.deleteFolder(it) }
//        )
//
//        Spacer(Modifier.height(16.dp))
//
//        when {
//            uiState.isLoading -> {
//                CircularProgressIndicator(modifier = Modifier.align(Alignment.CenterHorizontally))
//            }
//
//            uiState.folders.isEmpty() -> {
//                Text(
//                    "На разі немає жодної папки, cтвори папку для подальшого створення завдань",//"No folders. Click + to create one.",
//                    modifier = Modifier
//                        .padding(16.dp)
//                        .fillMaxWidth(),
//                    textAlign = TextAlign.Center
//                )
//            }
//
//            else -> {
//                uiState.selectedFolder?.let {
//                    Text("Showing contents of: ${it.name}", modifier = Modifier.padding(16.dp))
//                }
//            }
//        }


//@Composable
//fun FolderContent(uiState: TasksUiState) {
//    val selectedFolder = uiState.selectedFolder
//    var showAddTaskForm by remember { mutableStateOf(false) }
//    if (selectedFolder == null) {
//        Text("No folder selected")
//        return
//    }
//
//    //val tasks = selectedFolder.tasks  // Assuming `Folder` has a `tasks` list
//    val tasks = emptyList<Task>()
//    val groupedTasks = tasks.groupBy { it.status } // Assuming Task has a status: "TODO", "IN_PROGRESS", "DONE"
//
//    Column(modifier = Modifier
//        .fillMaxSize()
//        .padding(16.dp)) {
//
//        if (tasks.isEmpty()) {
//            Text("Папка пуста.")
//            Spacer(Modifier.height(8.dp))
//            Button(onClick = { showAddTaskForm = true }) {
//                Text("Додати завдання")
//            }
//        } else {
//            TaskSection(title = "До виконання", tasks = groupedTasks["TODO"].orEmpty())
//            TaskSection(title = "В процесі", tasks = groupedTasks["IN_PROGRESS"].orEmpty())
//            TaskSection(title = "Виконано", tasks = groupedTasks["DONE"].orEmpty())
//        }
//    }
//}

//
//@Composable
//fun FolderTabs(
//    folders: List<Folder>,
//    selectedFolder: Folder?,
//    onFolderSelected: (Folder) -> Unit,
//    onAddFolder: () -> Unit
//) {
//    LazyRow(
//        modifier = Modifier.fillMaxWidth(),
//        contentPadding = PaddingValues(horizontal = 8.dp, vertical = 8.dp),
//        verticalAlignment = Alignment.CenterVertically
//    ) {
//        item {
//            IconButton(onClick = onAddFolder) {
//                Icon(Icons.Default.Add, contentDescription = "Add Folder")
//            }
//        }
//
//        items(folders) { folder ->
//            val isSelected = folder == selectedFolder
//            val background = if (isSelected) Color.White else Color(0xFFF0F0F0)
//            val borderColor = if (isSelected) Color.LightGray else Color.Transparent
//
//            Box(
//                modifier = Modifier
//                    //.padding(horizontal = 4.dp)
//                    .clip(RoundedCornerShape(topStart = 12.dp/*, topEnd = 12.dp*/))
//                    .background(background)
//                    .border(1.dp, borderColor, RoundedCornerShape(topStart = 12.dp/*, topEnd = 12.dp*/))
//                    .clickable { onFolderSelected(folder) }
//                    .padding(horizontal = 16.dp, vertical = 10.dp)
//            ) {
//                Text(folder.name)
//            }
//        }
//    }
//}
//@Composable
//fun FolderTabs(
//    folders: List<Folder>,
//    selectedFolder: Folder?,
//    onFolderSelected: (Folder) -> Unit,
//    onAddFolder: () -> Unit
//) {
//    val overlapOffset = (-8).dp
//
//    LazyRow(
//        modifier = Modifier.fillMaxWidth(),
//        contentPadding = PaddingValues(horizontal = 8.dp, vertical = 8.dp),
//        verticalAlignment = Alignment.CenterVertically
//    ) {
//        item {
//            IconButton(onClick = onAddFolder) {
//                Icon(Icons.Default.Add, contentDescription = "Add Folder")
//            }
//        }
//
//        itemsIndexed(folders) { index, folder ->
//            val isSelected = folder == selectedFolder
//            val background = if (isSelected) Color.White else Color(0xFFF0F0F0)
//            val borderColor = if (isSelected) Color.LightGray else Color.Transparent
//            val shape = RoundedCornerShape(topStart = 12.dp, topEnd = 12.dp)
//            val elevation = if (isSelected) 6.dp else 2.dp
//
//            Box(
//                modifier = Modifier
//                    .offset(x = if (index > 0) overlapOffset else 0.dp)
//                    .zIndex(if (isSelected) 1f else 0f)
//                    .shadow(elevation, shape = shape, clip = false)
//                    .clip(shape)
//                    .background(background)
//                    .border(1.dp, borderColor, shape)
//                    .clickable { onFolderSelected(folder) }
//                    .padding(horizontal = 16.dp, vertical = 10.dp)
//            ) {
//                Text(folder.name)
//            }
//        }
//    }
//}