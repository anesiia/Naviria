import React, { useState, useEffect } from "react";
import "../styles/taskForm.css";
import { createTask, fetchCategories } from "../services/TasksServices";

// Дні тижня
const weekDays = [
  { short: "Пн", eng: "Monday" },
  { short: "Вт", eng: "Tuesday" },
  { short: "Ср", eng: "Wednesday" },
  { short: "Чт", eng: "Thursday" },
  { short: "Пт", eng: "Friday" },
  { short: "Сб", eng: "Saturday" },
  { short: "Нд", eng: "Sunday" },
];

export function TaskForm({ selectedFolderId, onCancel, onSave, fetchTasks }) {
  // Основні стейти для задачі
  const [titleValue, setTitleValue] = useState("");
  const [descriptionValue, setDescriptionValue] = useState("");
  const [tags, setTags] = useState([]);
  const [tagInput, setTagInput] = useState("");
  const [showDeadline, setShowDeadline] = useState(false);
  const [deadlineDateValue, setDeadlineDateValue] = useState("");
  const [showReminder, setShowReminder] = useState(false);
  const [notificationDateValue, setNotificationDateValue] = useState("");
  const [showProgress, setShowProgress] = useState(false);
  const [priorityValue, setPriorityValue] = useState(1);
  const [type, setType] = useState("simple"); // simple, repeat, scale, list

  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState("");

  // Повторювані задачі
  const [days, setDays] = useState([]);

  // Scale
  const [scaleUnit, setScaleUnit] = useState("");
  const [scaleGoal, setScaleGoal] = useState("");

  // Підзадачі для списку (тип list)
  const [subtasks, setSubtasks] = useState([
    {
      title: "",
      description: "",
      type: "simple",
      days: [],
      scaleUnit: "",
      scaleGoal: "",
    },
  ]);

  function toISOStringFromLocalDatetime(datetimeLocalValue) {
    if (!datetimeLocalValue) return null;
    // datetimeLocalValue має формат "2025-06-01T08:00"
    const [datePart, timePart] = datetimeLocalValue.split("T");
    if (!datePart || !timePart) return null;
    const [year, month, day] = datePart.split("-");
    const [hour, minute] = timePart.split(":");
    // Дата створюється в локальній зоні (як і в інпуті)
    const date = new Date(
      Number(year),
      Number(month) - 1,
      Number(day),
      Number(hour),
      Number(minute)
    );
    return date.toISOString();
  }

  // Додає нову порожню підзадачу
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

  // Змінити значення підзадачі (індекс + об'єкт)
  const handleSubtaskChange = (idx, newData) => {
    setSubtasks((subtasks) =>
      subtasks.map((st, i) => (i === idx ? newData : st))
    );
  };

  // Тогл дня для підзадачі repeat
  const handleSubtaskDayToggle = (idx, day) => {
    setSubtasks((subtasks) =>
      subtasks.map((st, i) =>
        i === idx
          ? {
              ...st,
              days: st.days.includes(day)
                ? st.days.filter((d) => d !== day)
                : [...st.days, day],
            }
          : st
      )
    );
  };

  // Видалити підзадачу
  const handleDeleteSubtask = (idx) => {
    setSubtasks((subtasks) => {
      if (subtasks.length === 1) {
        // Скидаємо форму до дефолтної, не видаляємо
        return [
          {
            title: "",
            description: "",
            type: "simple",
            days: [],
            scaleUnit: "",
            scaleGoal: "",
          },
        ];
      }
      // Видаляємо підзадачу якщо їх більше однієї
      return subtasks.filter((_, i) => i !== idx);
    });
  };

  useEffect(() => {
    fetchCategories().then((data) => {
      setCategories(data);
      // Якщо є категорії, ставимо першу як дефолт
      if (data.length > 0) setSelectedCategory(data[0].id);
    });
  }, []);

  // Додаємо теги
  const handleAddTag = () => {
    if (tagInput.trim() && !tags.includes(tagInput.trim())) {
      setTags([...tags, tagInput.trim()]);
      setTagInput("");
    }
  };

  // Видалити тег
  const handleDeleteTag = (tag) => {
    setTags(tags.filter((t) => t !== tag));
  };

  // Змінити вибір днів для repeat
  const handleDayToggle = (day) => {
    setDays((prev) =>
      prev.includes(day) ? prev.filter((d) => d !== day) : [...prev, day]
    );
  };

  // Аналогічно для підзадачі

  // Основне збереження
  const handleSave = async () => {
    if (!titleValue.trim()) return;

    console.log("showDeadline", showDeadline);
    console.log("deadlineDateValue", deadlineDateValue);
    console.log(
      "deadline toISOString",
      toISOStringFromLocalDatetime(deadlineDateValue)
    );

    const userId = localStorage.getItem("id");

    const payload = {
      userId,
      folderId: selectedFolderId,
      title: titleValue,
      description: descriptionValue,
      tags: tags.map((tag) => ({ tagName: tag })),
      categoryId: selectedCategory,
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
        repeat_days: days.map(
          (d) => ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд"].indexOf(d) + 1
        ),
        checked_in_days: [],
      }),
      ...(type === "scale" && {
        unit: scaleUnit,
        current_value: 0,
        target_value: Number(scaleGoal),
      }),
      ...(type === "list" && {
        subtasks: subtasks.map((st) => {
          const base = {
            subtask_type:
              st.type === "simple"
                ? "standard"
                : st.type === "repeat"
                ? "repeatable"
                : st.type === "scale"
                ? "scale"
                : "standard",
            title: st.title,
            description: st.description,
          };
          if (st.type === "repeat") {
            base.repeat_days = st.days.map(
              (d) => ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд"].indexOf(d) + 1
            );
            base.checked_in_days = [];
          }
          if (st.type === "scale") {
            base.unit = st.scaleUnit;
            base.current_value = 0;
            base.target_value = Number(st.scaleGoal);
          }
          if (st.type === "simple") {
            base.isCompleted = false;
          }
          return base;
        }),
      }),
    };

    try {
      await createTask(payload);
      if (fetchTasks) fetchTasks();
      onSave && onSave();
    } catch (err) {
      console.error(err);
      console.alert("Не вдалося створити задачу");
    }
  };

  return (
    <div className="task-form">
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

      <label className="category">
        Категорія
        <select
          value={selectedCategory}
          onChange={(e) => setSelectedCategory(e.target.value)}
          required
        >
          {categories.map((cat) => (
            <option key={cat.id} value={cat.id}>
              {cat.name}
            </option>
          ))}
        </select>
      </label>

      <label>Теги</label>
      <div className="tags-block">
        {/* Додані теги */}
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
        {/* Поле для вводу нового тегу */}
        <div className="tag-input-wrap">
          <span className="tag-hash">#</span>
          <input
            className="tag-input"
            value={tagInput}
            onChange={(e) => setTagInput(e.target.value)}
            placeholder=""
            onKeyDown={(e) => {
              if (e.key === "Enter") handleAddTag();
            }}
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

export function ToggleSwitch({ checked, onChange, label }) {
  return (
    <label className="toggle-container">
      <input type="checkbox" checked={checked} onChange={onChange} />
      <span className="slider" />
      <span className="label-text">{label}</span>
    </label>
  );
}
