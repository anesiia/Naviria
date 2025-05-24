import "../styles/task.css";
import { useState } from "react";
import { Subtasks } from "./Subtasks";

export function Task(props) {
  const [isCollapsed, setIsCollapsed] = useState(true);

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

  function formatDate(dateStr) {
    if (!dateStr) return "-";
    const date = new Date(dateStr);
    return date.toLocaleDateString("uk-UA");
  }

  return (
    <div className="main-task">
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
          <button className="delete">
            <img src="Group 174.svg" alt="delete" />
          </button>
          <button className="edit">
            <img src="fi-rr-pencil.svg" alt="edit" />
          </button>
          <button className="open" onClick={() => setIsCollapsed(!isCollapsed)}>
            <img
              src={isCollapsed ? "fi-rr-caret-down.svg" : "fi-rr-caret-up.svg"}
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
                <p className="value-date">-</p>
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
              <button className="done-btn">Готово</button>
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
                <input placeholder="Введіть значення" type="number"></input>
                <button className="add-value-btn">Додати</button>
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
              {Array.isArray(props.subtasks) && props.subtasks.length > 0 ? (
                props.subtasks.map((subtask, idx) => (
                  <Subtasks key={subtask.id || idx} {...subtask} />
                ))
              ) : (
                <p>Підзадач немає</p>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
