import React, { useState } from "react";
import "../styles/taskForm.css";
import { updateTask } from "../services/TasksServices";
import { ToggleSwitch } from "./TaskForm";

export function TaskEditForm({ task, onCancel, onSave, fetchTasks }) {
  const [titleValue, setTitleValue] = useState(task.title || "");
  const [descriptionValue, setDescriptionValue] = useState(
    task.description || ""
  );
  const [tags, setTags] = useState((task.tags || []).map((t) => t.tagName));
  const [tagInput, setTagInput] = useState("");
  const [showDeadline, setShowDeadline] = useState(task.isDeadlineOn || false);
  const [deadlineDateValue, setDeadlineDateValue] = useState(
    task.deadline ? new Date(task.deadline).toISOString().slice(0, 16) : ""
  );
  const [showReminder, setShowReminder] = useState(
    task.isNotificationsOn || false
  );
  const [notificationDateValue, setNotificationDateValue] = useState(
    task.notificationDate
      ? new Date(task.notificationDate).toISOString().slice(0, 16)
      : ""
  );
  const [showProgress, setShowProgress] = useState(
    task.isShownProgressOnPage || false
  );
  const [priorityValue, setPriorityValue] = useState(task.priority || 1);
  const [type, setType] = useState(mapApiTypeToFormType(task.type));

  // Repeat
  const [days, setDays] = useState(
    (task.repeatDays || [])
      .map((d) => weekDays.find((w) => w.eng === d)?.short)
      .filter(Boolean)
  );

  // Scale
  const [scaleUnit, setScaleUnit] = useState(task.unit || "");
  const [scaleGoal, setScaleGoal] = useState(task.targetValue || "");
  const [subtasks, setSubtasks] = useState(
    Array.isArray(task.subtasks)
      ? task.subtasks.map((st) => ({
          ...st,
          title: st.title || "",
          description: st.description || "",
          type:
            st.subtask_type === "standard"
              ? "simple"
              : st.subtask_type === "repeatable"
              ? "repeat"
              : st.subtask_type === "scale"
              ? "scale"
              : "simple",
          days: (st.repeatDays || []).map(
            (d) => weekDays.find((w) => w.eng === d)?.short
          ),
          scaleUnit: st.unit || "",
          scaleGoal: st.targetValue || "",
        }))
      : []
  );
  const handleAddSubtask = () => {
    setSubtasks([
      ...subtasks,
      {
        title: "",
        description: "",
        type: "simple",
        days: [],
        scaleUnit: "",
        scaleGoal: "",
      },
    ]);
  };

  const handleSubtaskChange = (idx, newData) => {
    setSubtasks((prev) => prev.map((st, i) => (i === idx ? newData : st)));
  };

  const handleDeleteSubtask = (idx) => {
    setSubtasks((prev) =>
      prev.length === 1
        ? [
            {
              title: "",
              description: "",
              type: "simple",
              days: [],
              scaleUnit: "",
              scaleGoal: "",
            },
          ]
        : prev.filter((_, i) => i !== idx)
    );
  };

  const handleSubtaskDayToggle = (idx, day) => {
    setSubtasks((prev) =>
      prev.map((st, i) =>
        i === idx
          ? {
              ...st,
              days: (st.days || []).includes(day)
                ? st.days.filter((d) => d !== day)
                : [...(st.days || []), day],
            }
          : st
      )
    );
  };

  const weekDays = [
    { short: "Пн", eng: "Monday" },
    { short: "Вт", eng: "Tuesday" },
    { short: "Ср", eng: "Wednesday" },
    { short: "Чт", eng: "Thursday" },
    { short: "Пт", eng: "Friday" },
    { short: "Сб", eng: "Saturday" },
    { short: "Нд", eng: "Sunday" },
  ];

  function mapApiTypeToFormType(apiType) {
    switch (apiType) {
      case "standard":
        return "simple";
      case "repeatable":
        return "repeat";
      case "scale":
        return "scale";
      case "with_subtasks":
        return "list";
      default:
        return "simple";
    }
  }

  function toISOStringFromLocalDatetime(datetimeLocalValue) {
    if (!datetimeLocalValue) return null;
    const [datePart, timePart] = datetimeLocalValue.split("T");
    const [year, month, day] = datePart.split("-");
    const [hour, minute] = timePart.split(":");
    const date = new Date(
      Number(year),
      Number(month) - 1,
      Number(day),
      Number(hour),
      Number(minute)
    );
    return date.toISOString();
  }

  const handleAddTag = () => {
    if (tagInput.trim() && !tags.includes(tagInput.trim())) {
      setTags([...tags, tagInput.trim()]);
      setTagInput("");
    }
  };

  const handleDeleteTag = (tag) => {
    setTags(tags.filter((t) => t !== tag));
  };

  const handleDayToggle = (day) => {
    setDays((prev) =>
      prev.includes(day) ? prev.filter((d) => d !== day) : [...prev, day]
    );
  };

  const handleSave = async () => {
    const payload = {
      ...task,
      title: titleValue,
      description: descriptionValue,
      tags: tags.map((tag) => ({ tagName: tag })),

      isDeadlineOn: showDeadline,
      deadline:
        showDeadline && deadlineDateValue
          ? toISOStringFromLocalDatetime(deadlineDateValue)
          : null,
      notificationDate:
        showReminder && notificationDateValue
          ? toISOStringFromLocalDatetime(notificationDateValue)
          : null,
      isShownProgressOnPage: showProgress,
      isNotificationsOn: showReminder,
      priority: Number(priorityValue),
      type:
        type === "simple"
          ? "standard"
          : type === "repeat"
          ? "repeatable"
          : type === "scale"
          ? "scale"
          : type === "list"
          ? "with_subtasks"
          : "standard",
      ...(type === "repeat" && {
        repeatDays: days.map((d) => weekDays.find((w) => w.short === d)?.eng),
      }),
      ...(type === "scale" && {
        unit: scaleUnit,
        targetValue: Number(scaleGoal),
      }),
      ...(type === "list" && {
        subtasks: subtasks.map((st) => {
          const base = {
            title: st.title,
            description: st.description,
            subtask_type:
              st.type === "simple"
                ? "standard"
                : st.type === "repeat"
                ? "repeatable"
                : st.type === "scale"
                ? "scale"
                : "standard",
          };
          if (st.type === "repeat") {
            base.repeatDays = st.days.map(
              (d) => weekDays.find((w) => w.short === d)?.eng
            );
            base.checked_in_days = st.checked_in_days || [];
          }
          if (st.type === "scale") {
            base.unit = st.scaleUnit;
            base.current_value = st.current_value || 0;
            base.targetValue = Number(st.scaleGoal);
          }
          if (st.type === "simple") {
            base.isCompleted = st.isCompleted || false;
          }
          if (st.id) base.id = st.id; // збереження ідентифікатора для існуючих
          return base;
        }),
      }),
    };

    try {
      await updateTask(task.id, payload);
      fetchTasks && fetchTasks();
      onSave && onSave();
    } catch (err) {
      console.error(err);
      alert("Не вдалося оновити задачу");
    }
  };

  return (
    <div className="task-form">
      {/* Поля — такі ж як у TaskForm, тільки значення з пропсів */}
      <input
        className="title"
        placeholder="Введіть назву задачі"
        value={titleValue}
        onChange={(e) => setTitleValue(e.target.value)}
        required
      />
      <input
        className="description"
        placeholder="Опис вашої задачі"
        value={descriptionValue}
        onChange={(e) => setDescriptionValue(e.target.value)}
      />

      {/* Теги */}
      <label>Теги</label>
      <div className="tags-block">
        <div className="tags">
          {tags.map((tag) => (
            <span key={tag} className="tag">
              <button
                className="tag__remove"
                onClick={() => handleDeleteTag(tag)}
              >
                ×
              </button>
              <span className="tag__text">#{tag}</span>
            </span>
          ))}
        </div>
        <div className="tag-input-wrap">
          <span className="tag-hash">#</span>
          <input
            className="tag-input"
            value={tagInput}
            onChange={(e) => setTagInput(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && handleAddTag()}
            maxLength={30}
          />
          <button type="button" className="tag-add-btn" onClick={handleAddTag}>
            +
          </button>
        </div>
      </div>

      <div className="toggles">
        <div>
          <ToggleSwitch
            checked={showDeadline}
            onChange={(e) => setShowDeadline(e.target.checked)}
            label="Дедлайн"
          />
          {showDeadline && (
            <input
              type="datetime-local"
              value={deadlineDateValue}
              onChange={(e) => setDeadlineDateValue(e.target.value)}
              required
            />
          )}
        </div>
        <div>
          <ToggleSwitch
            checked={showReminder}
            onChange={(e) => setShowReminder(e.target.checked)}
            label="Нагадування"
          />
          {showReminder && (
            <input
              type="datetime-local"
              value={notificationDateValue}
              onChange={(e) => setNotificationDateValue(e.target.value)}
              required
            />
          )}
        </div>
        <div>
          <ToggleSwitch
            checked={showProgress}
            onChange={(e) => setShowProgress(e.target.checked)}
            label="Показувати прогрес"
          />
        </div>
      </div>

      <div className="priority">
        <label>Пріоритет</label>
        <input
          type="number"
          min={0}
          max={10}
          value={priorityValue}
          onChange={(e) => setPriorityValue(e.target.value)}
          required
        />
      </div>
      <div className="task-type">
        <label className="task-form__label">Тип задачі</label>
        <div className="types">
          <label>
            <input
              type="radio"
              name="taskType"
              value="simple"
              checked={type === "simple"}
              onChange={(e) => setType(e.target.value)}
            />
            Звичайна
          </label>
          <label>
            <input
              type="radio"
              name="taskType"
              value="repeat"
              checked={type === "repeat"}
              onChange={(e) => setType(e.target.value)}
            />
            Повторювана
          </label>
          <label>
            <input
              type="radio"
              name="taskType"
              value="scale"
              checked={type === "scale"}
              onChange={(e) => setType(e.target.value)}
            />
            Зі шкалою
          </label>
          <label>
            <input
              type="radio"
              name="taskType"
              value="list"
              checked={type === "list"}
              onChange={(e) => setType(e.target.value)}
            />
            З підзадачами
          </label>
        </div>
      </div>

      {type === "repeat" && (
        <div className="repeat-days">
          <label>Дні для виконання</label>
          <div className="days">
            {weekDays.map((d, i) => (
              <span
                key={i}
                className={days.includes(d.short) ? "day active" : "day"}
                onClick={() => handleDayToggle(d.short)}
              >
                {d.short}
              </span>
            ))}
          </div>
        </div>
      )}
      {/* Для scale */}
      {type === "scale" && (
        <div className="scale-form">
          <label>Одиниця вимірювання</label>
          <input
            placeholder="Введіть символ або назву"
            value={scaleUnit}
            onChange={(e) => setScaleUnit(e.target.value)}
          />
          <label>Ціль</label>
          <input
            placeholder="Введіть число"
            step="0.01"
            min={0}
            type="number"
            value={scaleGoal}
            onChange={(e) => setScaleGoal(e.target.value)}
          />
        </div>
      )}

      {type === "list" && (
        <div className="subtasks-form">
          <h4>Підзадачі:</h4>
          {subtasks.map((st, idx) => (
            <div key={idx} className="subtask">
              <div className="subtask-number">{idx + 1}.</div>
              <input
                placeholder="Введіть назву"
                className="input-text"
                value={st.title}
                onChange={(e) =>
                  handleSubtaskChange(idx, { ...st, title: e.target.value })
                }
                required
              />
              <input
                placeholder="Опис"
                className="input-text"
                value={st.description}
                onChange={(e) =>
                  handleSubtaskChange(idx, {
                    ...st,
                    description: e.target.value,
                  })
                }
              />
              <div className="subtask-type">
                <label>Тип підзадачі:</label>
                <div className="types">
                  <label>
                    <input
                      type="radio"
                      name={`type-${idx}`}
                      value="simple"
                      checked={st.type === "simple"}
                      onChange={(e) =>
                        handleSubtaskChange(idx, {
                          ...st,
                          type: e.target.value,
                          days: [],
                          scaleUnit: "",
                          scaleGoal: "",
                        })
                      }
                    />
                    Звичайна
                  </label>
                  <label>
                    <input
                      type="radio"
                      name={`type-${idx}`}
                      value="repeat"
                      checked={st.type === "repeat"}
                      onChange={(e) =>
                        handleSubtaskChange(idx, {
                          ...st,
                          type: e.target.value,
                          days: [],
                          scaleUnit: "",
                          scaleGoal: "",
                        })
                      }
                    />
                    Повторювана
                  </label>
                  <label>
                    <input
                      type="radio"
                      name={`type-${idx}`}
                      value="scale"
                      checked={st.type === "scale"}
                      onChange={(e) =>
                        handleSubtaskChange(idx, {
                          ...st,
                          type: e.target.value,
                          days: [],
                          scaleUnit: "",
                          scaleGoal: "",
                        })
                      }
                    />
                    Шкала
                  </label>
                </div>
              </div>
              {/* Для repeat — дні */}
              {st.type === "repeat" && (
                <div className="days">
                  {weekDays.map((d, i) => (
                    <span
                      key={i}
                      className={
                        st.days?.includes(d.short) ? "day active" : "day"
                      }
                      onClick={() => handleSubtaskDayToggle(idx, d.short)}
                    >
                      {d.short}
                    </span>
                  ))}
                </div>
              )}
              {/* Для scale — одиниця та ціль */}
              {st.type === "scale" && (
                <div className="scale-form">
                  <input
                    placeholder="Одиниця вимірювання"
                    value={st.scaleUnit || ""}
                    onChange={(e) =>
                      handleSubtaskChange(idx, {
                        ...st,
                        scaleUnit: e.target.value,
                      })
                    }
                    required
                  />
                  <input
                    placeholder="Ціль"
                    type="number"
                    value={st.scaleGoal || ""}
                    onChange={(e) =>
                      handleSubtaskChange(idx, {
                        ...st,
                        scaleGoal: e.target.value,
                      })
                    }
                    required
                  />
                </div>
              )}
              <button
                type="button"
                className="delete-subtask"
                onClick={() => handleDeleteSubtask(idx)}
              >
                Видалити
              </button>
            </div>
          ))}
          <button
            type="button"
            className="add-subtask"
            onClick={handleAddSubtask}
          >
            <span style={{ fontSize: 24, marginRight: 8 }}>+</span>Додати
            підзадачу
          </button>
        </div>
      )}

      <div className="form-actions">
        <button className="cancel" onClick={onCancel}>
          Скасувати
        </button>
        <button className="save" onClick={handleSave}>
          Зберегти
        </button>
      </div>
    </div>
  );
}
