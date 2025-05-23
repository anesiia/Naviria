import "../styles/tasks.css";
import { useState } from "react";
import { Task } from "./Task";
import { Subtasks } from "./Subtasks";
import { TaskForm } from "./TaskForm";
export function Tasks() {
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [folders, setFolders] = useState([
    {
      id: 1,
      name: "Особисте",
      tasks: [
        { id: 101, name: "Купити продукти", type: "simple", completed: false },
        { id: 102, name: "Спорт", type: "list", completed: true },
      ],
    },
    {
      id: 2,
      name: "Робота",
      tasks: [
        { id: 201, name: "Написати звіт", type: "repeat", completed: false },
      ],
    },
  ]);
  const [creatingFolder, setCreatingFolder] = useState(false);
  const [newFolderName, setNewFolderName] = useState("");
  const [selectedFolderId, setSelectedFolderId] = useState(null);

  const handleAddFolder = () => {
    if (newFolderName.trim() !== "") {
      const newFolder = {
        id: Date.now(),
        name: newFolderName.trim(),
        tasks: [],
      };
      setFolders([...folders, newFolder]);
    }
    setCreatingFolder(false);
    setNewFolderName("");
  };

  const handleDeleteFolder = (folderId) => {
    setFolders((prev) => prev.filter((folder) => folder.id !== folderId));
  };
  const handleToggleTask = (folderId, taskId) => {
    setFolders((prev) =>
      prev.map((folder) =>
        folder.id !== folderId
          ? folder
          : {
              ...folder,
              tasks: folder.tasks.map((task) =>
                task.id !== taskId
                  ? task
                  : { ...task, completed: !task.completed }
              ),
            }
      )
    );
  };

  const selectedFolder = folders.find((f) => f.id === selectedFolderId);

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
                <img src="fi-rr-thumbtack.svg" />
                <p>{folder.name}</p>
                <button onClick={() => handleDeleteFolder(folder.id)}>
                  <img src="Group 174.svg" />
                </button>
              </div>
              <div className="tasks">
                {folder.tasks.map((task) => (
                  <label
                    key={task.id}
                    className={`task-label ${
                      task.completed ? "completed" : ""
                    }`}
                  >
                    <input
                      type="checkbox"
                      checked={task.completed}
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
            {selectedFolder?.tasks?.map((task) => (
              <Task key={task.id} {...task} />
            ))}
          </div>
        </div>
        <div className="done">
          <h2>Виконано</h2>
          <div className="tasks">
            <div className="task">
              <div className="name">
                <input type="checkbox" id="tasks" name="tasks"></input>
                <label for="tasks">Task</label>
              </div>
              <div className="buttons">
                <button className="delete">
                  <img src="Group 174.svg" />
                </button>
              </div>
            </div>
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
