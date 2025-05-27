import "../styles/task.css";
import { useState, useEffect } from "react";
import { Subtasks } from "./Subtasks";
import { TaskEditForm } from "./TaskEditForm";
import {
  checkinRepeatableTask,
  updateTask,
  deleteTask,
} from "../services/TasksServices";

export function Task(props) {
  const [isCollapsed, setIsCollapsed] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [isEditing, setIsEditing] = useState(false);

  let taskType;
  switch (props.type) {
    case "standard":
      taskType = "simple";
      break;
    case "repeatable":
      taskType = "repeat";
      break;
    case "scale":
      taskType = "scale";
      break;
    case "with_subtasks":
      taskType = "list";
      break;
    default:
      taskType = "simple";
  }

  const weekDays = [
    { short: "Пн", eng: "Monday" },
    { short: "Вт", eng: "Tuesday" },
    { short: "Ср", eng: "Wednesday" },
    { short: "Чт", eng: "Thursday" },
    { short: "Пт", eng: "Friday" },
    { short: "Сб", eng: "Saturday" },
    { short: "Нд", eng: "Sunday" },
  ];

  const today = new Date();
  const weekDayEng =
    weekDays[today.getDay() === 0 ? 6 : today.getDay() - 1].eng; // js: 0-неділя
  const todayISO = today.toISOString().split("T")[0];
  const canCheckin =
    Array.isArray(props.repeatDays) && props.repeatDays.includes(weekDayEng);
  const alreadyChecked =
    Array.isArray(props.checkedInDays) &&
    props.checkedInDays.some((d) => d.startsWith(todayISO));

  const handleCheckin = async () => {
    setError("");
    setLoading(true);
    try {
      await checkinRepeatableTask(props.id, new Date().toISOString());
      // Тут можна оновити локальний state, або оновити задачі через props.onCheckinSuccess
      await props.fetchTasks(); // або краще через onCheckinSuccess, якщо ти прокидаєш пропс зверху
    } catch {
      setError("Помилка при фіксації прогресу.");
    }
    setLoading(false);
  };

  function formatDate(dateStr) {
    if (!dateStr) return "-";
    const date = new Date(dateStr);
    return date.toLocaleDateString("uk-UA");
  }

  const [addValue, setAddValue] = useState("");
  const [adding, setAdding] = useState(false);

  const handleAddValue = async () => {
    if (!addValue || isNaN(addValue)) return;

    setAdding(true);
    try {
      const updatedTask = {
        ...props, // копіюємо всі поточні дані
        currentValue: (props.currentValue || 0) + Number(addValue),
      };
      await updateTask(props.id, updatedTask);
      await props.fetchTasks(); // або зробити оновлення стану без reload
    } catch {
      setError("Не вдалося додати значення");
    }
    setAdding(false);
    setAddValue(""); // очистити поле
  };

  const handleDelete = async () => {
    if (!window.confirm("Ви точно хочете видалити цю задачу?")) return;
    try {
      await deleteTask(props.id);
      if (props.onDelete) props.onDelete(); // Оновити список задач після видалення
    } catch {
      alert("Не вдалося видалити задачу");
    }
  };
  useEffect(() => {
    setIsEditing(false); // або force rerender
  }, [props.tags]);

  return (
    <div className="main-task">
      {isEditing ? (
        <TaskEditForm
          task={props}
          onCancel={() => setIsEditing(false)}
          onSave={() => {
            setIsEditing(false);
            props.fetchTasks?.();
          }}
          fetchTasks={props.fetchTasks}
        />
      ) : (
        <>
          <div className="main-task-info">
            <div className="name">
              <input
                type="checkbox"
                checked={props.status === "Completed"}
                disabled={props.status === "Completed"}
                onChange={() => {
                  if (props.status !== "Completed" && props.onToggleTask) {
                    props.onToggleTask(props.folderId, props.id);
                  }
                }}
              />
              <label>{props.title}</label>
            </div>
            <div className="buttons">
              <button className="delete" onClick={handleDelete}>
                <img src="Group 174.svg" alt="delete" />
              </button>
              <button className="edit" onClick={() => setIsEditing(true)}>
                <img src="fi-rr-pencil.svg" alt="edit" />
              </button>
              <button
                className="open"
                onClick={() => setIsCollapsed(!isCollapsed)}
              >
                <img
                  src={
                    isCollapsed ? "fi-rr-caret-down.svg" : "fi-rr-caret-up.svg"
                  }
                  alt="open"
                />
              </button>
            </div>
          </div>
          <div className="task-main-info">
            {isCollapsed ? null : taskType === "simple" ? (
              <div className="info-task-simple">
                <p className="desc">{props.description}</p>
                <div className="dates">
                  <div className="start-date">
                    <p className="name-date">Дата старту</p>
                    <p className="value-date">{formatDate(props.createdAt)}</p>
                  </div>
                  <div className="deadline">
                    <p className="name-date">Дедлайн</p>
                    <p className="value-date">{formatDate(props.deadline)}</p>
                  </div>
                </div>
              </div>
            ) : taskType === "repeat" ? (
              <div className="info-task-repeat">
                <div className="info">
                  <p>{props.description}</p>
                  <div className="dates">
                    <div className="start-date">
                      <p className="name-date">Дата старту</p>
                      <p className="value-date">-</p>
                    </div>
                    <div className="deadline">
                      <p className="name-date">Дедлайн</p>
                      <p className="value-date">{formatDate(props.deadline)}</p>
                    </div>
                  </div>
                </div>
                <div className="main-repeat">
                  <div className="naming">
                    <p>Дні для занять</p>
                    <div className="days">
                      {weekDays.map((day, i) => {
                        const isActive = props.repeatDays?.includes(day.eng);
                        return (
                          <div
                            className={`day${isActive ? " active" : ""}`}
                            key={i}
                          >
                            <p>{day.short}</p>
                          </div>
                        );
                      })}
                    </div>
                  </div>
                  {canCheckin && (
                    <button
                      className="done-btn"
                      onClick={handleCheckin}
                      disabled={!canCheckin || alreadyChecked || loading}
                    >
                      {alreadyChecked ? "Вже відмічено!" : "Відмітити"}
                    </button>
                  )}

                  {error && <p className="error">{error}</p>}
                </div>
              </div>
            ) : taskType === "scale" ? (
              <div className="info-task-scale">
                <div className="bg-gray">
                  <div className="desc-scale">
                    <p>{props.description}</p>
                    <div className="scale-info">
                      <div className="scale">
                        <div
                          className="color-scale"
                          style={{
                            width: props.targetValue
                              ? `${Math.round(
                                  (props.currentValue / props.targetValue) * 100
                                )}%`
                              : "0%",
                          }}
                        ></div>
                      </div>
                      <p className="points">
                        {props.currentValue || 0}/{props.targetValue || 0}
                        <br />
                        {props.unit}
                      </p>
                    </div>
                  </div>
                  <div className="dates">
                    <div className="start-date">
                      <p className="name-date">Дата старту</p>
                      <p className="value-date">-</p>
                    </div>
                    <div className="deadline">
                      <p className="name-date">Дедлайн</p>
                      <p className="value-date">{formatDate(props.deadline)}</p>
                    </div>
                  </div>
                </div>
                <div className="add-value">
                  <p>Додати значення</p>
                  <div className="add-action">
                    <input
                      placeholder="Введіть значення"
                      type="number"
                      value={addValue}
                      onChange={(e) => setAddValue(e.target.value)}
                      min={1}
                    />
                    <button
                      className="add-value-btn"
                      onClick={handleAddValue}
                      disabled={!addValue || isNaN(addValue) || adding}
                    >
                      Додати
                    </button>
                  </div>
                </div>
              </div>
            ) : (
              <div className="info-task-list">
                <div className="info">
                  <p>{props.description}</p>
                  <div className="dates">
                    <div className="start-date">
                      <p className="name-date">Дата старту</p>
                      <p className="value-date">-</p>
                    </div>
                    <div className="deadline">
                      <p className="name-date">Дедлайн</p>
                      <p className="value-date">{formatDate(props.deadline)}</p>
                    </div>
                  </div>
                </div>
                <div className="main-list">
                  {Array.isArray(props.subtasks) &&
                  props.subtasks.length > 0 ? (
                    props.subtasks.map((subtask, idx) => (
                      <Subtasks
                        key={subtask.id || idx}
                        {...subtask}
                        taskId={props.id}
                        fetchTasks={props.fetchTasks}
                      />
                    ))
                  ) : (
                    <p>Підзадач немає</p>
                  )}
                </div>
              </div>
            )}
          </div>
        </>
      )}
    </div>
  );
}
