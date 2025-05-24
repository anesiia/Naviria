import "../styles/subtask.css";
import {
  checkinRepeatableSubtask,
  updateSubtask,
} from "../services/TasksServices";
import { useState } from "react";
export function Subtasks(props) {
  const [addValue, setAddValue] = useState("");
  const [adding, setAdding] = useState(false);

  const handleAddValue = async () => {
    if (!addValue || isNaN(addValue)) return;
    setAdding(true);
    try {
      const updatedSubtask = {
        ...props,
        currentValue: (props.currentValue || 0) + Number(addValue),
      };
      await updateSubtask(props.taskId, props.id, updatedSubtask);
      await props.fetchTasks();
    } catch {
      setError("Не вдалося додати значення");
    }
    setAdding(false);
    setAddValue("");
  };

  let subtaskType;
  switch (props.type) {
    case "standard":
      subtaskType = "simple";
      break;
    case "repeatable":
      subtaskType = "repeat";
      break;
    case "scale":
      subtaskType = "scale";
      break;
    case "with_subtasks":
      subtaskType = "list";
      break;
    default:
      subtaskType = "simple";
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
    weekDays[today.getDay() === 0 ? 6 : today.getDay() - 1].eng;
  const todayISO = today.toISOString().split("T")[0];

  const canCheckin =
    Array.isArray(props.repeatDays) && props.repeatDays.includes(weekDayEng);
  const alreadyChecked =
    Array.isArray(props.checkedInDays) &&
    props.checkedInDays.some((d) => d.startsWith(todayISO));
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  return (
    <div className="subtask">
      {subtaskType === "simple" ? (
        <div className="info-subtask-simple">
          <div className="subtask-name">
            <input type="checkbox" checked={props.isCompleted} disabled />
            <label>{props.title}</label>
          </div>
          <div className="desc">{props.description}</div>
        </div>
      ) : subtaskType === "repeat" ? (
        <div className="info-subtask-repeat">
          <div className="subtask-name">
            <input type="checkbox" checked={props.isCompleted} disabled />
            <label>{props.title}</label>
          </div>
          <div className="desc">{props.description}</div>
          <div className="values-repeat">
            <div className="naming">
              <p>Дні для занять</p>
              <div className="days">
                {weekDays.map((day, i) => {
                  const isActive = props.repeatDays?.includes(day.eng);
                  return (
                    <div className={`day${isActive ? " active" : ""}`} key={i}>
                      <p>{day.short}</p>
                    </div>
                  );
                })}
              </div>
            </div>
            {canCheckin && (
              <button
                className="done-btn"
                onClick={async () => {
                  setError("");
                  setLoading(true);
                  try {
                    await checkinRepeatableSubtask(
                      props.taskId,
                      props.id,
                      new Date().toISOString()
                    );
                    await props.fetchTasks();
                  } catch {
                    setError("Помилка при фіксації прогресу.");
                  }
                  setLoading(false);
                }}
                disabled={alreadyChecked || loading}
              >
                {alreadyChecked ? "Вже відмічено" : "Відмітити"}
              </button>
            )}
            {error && <p className="error">{error}</p>}
          </div>
        </div>
      ) : (
        <div className="info-subtask-scale">
          <div className="subtask-name">
            <input type="checkbox" checked={props.isCompleted} disabled />
            <label>{props.title}</label>
          </div>
          <div className="desc">{props.description}</div>
          <div className="values">
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
                {props.unit ? (
                  <>
                    {" "}
                    <br />
                    {props.unit}
                  </>
                ) : null}
              </p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
