import "../styles/tasks.css";
import { useEffect, useState } from "react";
import { updateTask } from "../services/TasksServices";
import {
  fetchGroupedTasksByUser,
  createFolder,
  deleteFolder,
} from "../services/FoldersServices";
import { Task } from "./Task";
import { Subtasks } from "./Subtasks";
import { TaskForm } from "./TaskForm";
export function Tasks() {
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [folders, setFolders] = useState([]);
  const [creatingFolder, setCreatingFolder] = useState(false);
  const [newFolderName, setNewFolderName] = useState("");
  const [selectedFolderId, setSelectedFolderId] = useState(null);
  // 1. Завантаження папок із задачами з сервера + сортування
  useEffect(() => {
    fetchGroupedTasksByUser()
      .then((data) => {
        // адаптуємо структуру під старий рендер і забираємо createdAt
        const mapped = data
          .map((folder) => ({
            id: folder.folderId,
            name: folder.folderName,
            createdAt: folder.createdAt, // важливо!
            tasks: folder.tasks.map((task) => ({
              id: task.id,
              name: task.title,
              completed: task.status === "Completed",
              ...task,
            })),
          }))
          .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
        setFolders(mapped);
      })
      .catch((err) => console.error("Помилка при завантаженні задач:", err));
  }, []);

  // 2. Створення нової папки (і вона одразу згори)
  const handleAddFolder = async () => {
    if (newFolderName.trim() !== "") {
      try {
        const res = await createFolder(newFolderName.trim());
        const folder = res.folder; // API повертає саме так

        // додаємо нову папку і сортуємо ще раз (на випадок різних дат)
        setFolders((prev) =>
          [
            {
              id: folder.id,
              name: folder.name,
              createdAt: folder.createdAt,
              tasks: [],
            },
            ...prev,
          ].sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
        );
      } catch (err) {
        console.error("Помилка при створенні папки:", err);
      }
    }
    setCreatingFolder(false);
    setNewFolderName("");
  };

  const handleDeleteFolder = async (folderId) => {
    try {
      await deleteFolder(folderId);
      setFolders((prev) => prev.filter((folder) => folder.id !== folderId));
      if (selectedFolderId === folderId) setSelectedFolderId(null); // якщо видалена була вибрана
    } catch (err) {
      console.error("Помилка при видаленні папки:", err);
      // можеш показати повідомлення користувачу
    }
  };

  const handleToggleTask = async (folderId, taskId) => {
    // Знаходимо задачу
    const folder = folders.find((f) => f.id === folderId);
    const task = folder?.tasks.find((t) => t.id === taskId);
    if (!task || task.completed) return; // Якщо вже виконано - нічого не робимо

    // Формуємо оновлений об'єкт для PUT (копіюємо всі потрібні поля!)
    const updatedTask = {
      ...task,
      status: "Completed",
      // Додати тут усі поля, які очікує сервер!
      // Наприклад:
      title: task.title,
      description: task.description,
      tags: task.tags,
      isDeadlineOn: task.isDeadlineOn,
      deadline: task.deadline,
      isShownProgressOnPage: task.isShownProgressOnPage,
      isNotificationsOn: task.isNotificationsOn,
      notificationDate: task.notificationDate,
      priority: task.priority,
      type: task.type,
      repeatDays: task.repeatDays,
      checkedInDays: task.checkedInDays,
      subtasks: task.subtasks,
      // тощо (додай усе, що повертає бекенд у GET)
    };

    try {
      await updateTask(taskId, updatedTask);

      // Локально оновлюємо статус
      setFolders((prev) =>
        prev.map((folder) =>
          folder.id !== folderId
            ? folder
            : {
                ...folder,
                tasks: folder.tasks.map((t) =>
                  t.id !== taskId
                    ? t
                    : { ...t, completed: true, status: "Completed" }
                ),
              }
        )
      );
    } catch (err) {
      console.error("Помилка при оновленні задачі:", err);
      // Можна показати повідомлення про помилку
    }
  };

  const selectedFolder = folders.find((f) => f.id === selectedFolderId);

  const fetchTasks = async () => {
    const data = await fetchGroupedTasksByUser();
    const mapped = data
      .map((folder) => ({
        id: folder.folderId,
        name: folder.folderName,
        createdAt: folder.createdAt,
        tasks: folder.tasks.map((task) => ({
          id: task.id,
          name: task.title,
          completed: task.status === "Completed",
          ...task,
        })),
      }))
      .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
    setFolders(mapped);
  };

  useEffect(() => {
    fetchTasks();
  }, []);

  return (
    <div className="tasks-page">
      <div className="side-bar">
        <div className="side-header">
          <div className="info">
            <img src="Ellipse 4.svg" />
            <p className="nickname">Nickname</p>{" "}
          </div>
          <img src="fi-rr-menu-burger.svg" />
        </div>
        <div className="add-folder">
          {!creatingFolder ? (
            <>
              <button
                className="add-folder-btn"
                onClick={() => setCreatingFolder(true)}
              >
                +
              </button>
              <p>Створити папку</p>
            </>
          ) : (
            <div className="create-folder-form">
              <input
                type="text"
                placeholder="Назва папки"
                value={newFolderName}
                onChange={(e) => setNewFolderName(e.target.value)}
              />
              <div className="buttons">
                <button
                  className="cancel"
                  onClick={() => setCreatingFolder(false)}
                >
                  Скасувати
                </button>
                <button className="save" onClick={handleAddFolder}>
                  Зберегти
                </button>
              </div>
            </div>
          )}
        </div>
        <div className="folders">
          {folders.map((folder) => (
            <div
              className={`folder ${
                folder.id === selectedFolderId ? "selected-folder" : ""
              }`}
              key={folder.id}
              onClick={() => setSelectedFolderId(folder.id)}
            >
              <div className="head">
                <div className="head-info">
                  <img src="fi-rr-thumbtack.svg" />
                  <p>{folder.name}</p>
                </div>

                <button onClick={() => handleDeleteFolder(folder.id)}>
                  <img src="Group 174.svg" />
                </button>
              </div>
              <div className="tasks">
                {[...folder.tasks]
                  .sort((a, b) => {
                    // Completed повинні бути внизу
                    if (a.completed && !b.completed) return 1;
                    if (!a.completed && b.completed) return -1;
                    return 0;
                  })
                  .map((task) => (
                    <label
                      key={task.id}
                      className={`task-label ${
                        task.completed ? "completed" : ""
                      }`}
                    >
                      <input
                        type="checkbox"
                        checked={task.completed}
                        disabled={task.completed}
                        onChange={() => handleToggleTask(folder.id, task.id)}
                      />
                      {task.name}
                    </label>
                  ))}
              </div>
            </div>
          ))}
        </div>
      </div>
      <div className="content">
        <h1>
          {selectedFolder ? `Мої ${selectedFolder.name}` : "Оберіть папку"}
        </h1>
        <div className="add-task">
          <button
            className="add-task-btn"
            onClick={() => setShowCreateForm(true)}
          >
            +
          </button>
          <p>Створити нову задачу</p>
        </div>
        <div className="in-progress">
          <h2>В процесі</h2>
          <div className="tasks">
            {showCreateForm && (
              <TaskForm
                onCancel={() => setShowCreateForm(false)}
                onSave={() => setShowCreateForm(false)}
              />
            )}
            {selectedFolder?.tasks
              ?.filter((task) => task.status !== "Completed")
              .map((task) => (
                <Task
                  key={task.id}
                  {...task}
                  folderId={selectedFolder.id}
                  onToggleTask={handleToggleTask}
                  fetchTasks={fetchTasks}
                  onDelete={fetchTasks} // або свій метод для оновлення
                />
              ))}
          </div>
        </div>
        <div className="done">
          <h2>Виконано</h2>
          <div className="tasks">
            {selectedFolder?.tasks
              ?.filter((task) => task.status === "Completed")
              .map((task) => (
                <Task
                  key={task.id}
                  {...task}
                  folderId={selectedFolder.id}
                  fetchTasks={fetchTasks}
                  onToggleTask={handleToggleTask}
                  onDelete={fetchTasks}
                />
              ))}
          </div>
        </div>

        <h1>Превʼю всіх типів задач</h1>

        <h2>Simple Task</h2>
        <Task type="simple" />

        <h2>Repeat Task</h2>
        <Task type="repeat" />

        <h2>Scale Task</h2>
        <Task type="scale" />

        <h2>List Task (з Subtasks)</h2>
        <Task type="list" />

        <hr />

        <h2>Simple Subtask</h2>
        <Subtasks type="simple" />

        <h2>Repeat Subtask</h2>
        <Subtasks type="repeat" />

        <h2>Scale Subtask</h2>
        <Subtasks type="scale" />
      </div>
    </div>
  );
}
