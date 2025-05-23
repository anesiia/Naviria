import { useState } from "react";
import "../styles/taskForm.css"; // окремо зроби файл стилів

export function TaskForm({ onCancel, onSave }) {
  const [type, setType] = useState("simple");
  const [showDeadline, setShowDeadline] = useState(false);
  const [showReminder, setShowReminder] = useState(false);
  const [showProgress, setShowProgress] = useState(false);
  const [tags, setTags] = useState([]);
  const [newTag, setNewTag] = useState("");
  const [days, setDays] = useState([]);
  const [scaleGoal, setScaleGoal] = useState("");
  const [scaleUnit, setScaleUnit] = useState("");
  const [subtasks, setSubtasks] = useState([
    {
      title: "",
      description: "",
      deadline: false,
      reminder: false,
      reminderDate: "",
      reminderTime: "",
      deadlineDate: "",
      type: "simple",
    },
  ]);

  const toggleDay = (day) => {
    setDays((prev) =>
      prev.includes(day) ? prev.filter((d) => d !== day) : [...prev, day]
    );
  };

  const updateSubtask = (index, key, value) => {
    const updated = [...subtasks];
    updated[index][key] = value;
    setSubtasks(updated);
  };

  const removeSubtask = (index) => {
    if (subtasks.length === 1) {
      setSubtasks([
        {
          title: "",
          description: "",
          deadline: false,
          reminder: false,
          type: "simple",
        },
      ]);
    } else {
      setSubtasks(subtasks.filter((_, i) => i !== index));
    }
  };

  const addTag = () => {
    if (
      newTag.trim() !== "" &&
      !tags.includes(newTag.trim()) &&
      tags.length < 10
    ) {
      setTags([...tags, newTag.trim()]);
      setNewTag("");
    }
  };

  const removeTag = (tagToRemove) => {
    setTags(tags.filter((tag) => tag !== tagToRemove));
  };

  return (
    <div className="task-form">
      <input
        type="text"
        placeholder="Введіть назву задачі"
        className="title"
        required
      />
      <input
        type="text"
        placeholder="Опис вашої задачі"
        className="description"
      />

      <div className="category">
        <label>Категорія</label>
        <select required>
          <option value="daily">Повсякденні</option>
          <option value="sport">Спорт</option>
        </select>
      </div>
      <div className="tags">
        <label>Теги</label>
        <div className="tag-wrapper">
          {tags.map((tag) => (
            <div key={tag} className="tag">
              <span className="remove" onClick={() => removeTag(tag)}>
                ✕
              </span>
              <span className="text">#{tag}</span>
            </div>
          ))}

          {tags.length < 10 && (
            <div className="tag-input-box">
              <span className="hash">#</span>
              <input
                type="text"
                value={newTag}
                onChange={(e) => setNewTag(e.target.value)}
                className="tag-input"
              />
              <button className="add-button" onClick={addTag}>
                +
              </button>
            </div>
          )}
        </div>
      </div>

      <div className="toggles">
        <ToggleSwitch
          checked={showDeadline}
          onChange={() => setShowDeadline(!showDeadline)}
          label="Дедлайн"
        />
        <input type="date" disabled={!showDeadline} />

        <ToggleSwitch
          checked={showProgress}
          onChange={() => setShowProgress(!showProgress)}
          label="Відображати прогрес на сторінці"
        />

        <ToggleSwitch
          checked={showReminder}
          onChange={() => setShowReminder(!showReminder)}
          label="Нагадування"
        />
        <div className="date-reminder">
          <label>Дата</label>
          <input className="date" type="date" disabled={!showReminder} />
        </div>
        <div className="time-reminder">
          <label>Час</label>
          <input type="time" disabled={!showReminder} />
        </div>
      </div>

      <div className="priority">
        <label>Пріоритет</label>
        <select required>
          <option>Низький</option>
          <option>Середній</option>
          <option>Високий</option>
        </select>
      </div>

      <div className="task-type">
        <label>Тип задачі</label>
        <div className="types">
          <label>
            <input
              type="radio"
              name="type"
              value="simple"
              onChange={() => setType("simple")}
              checked={type === "simple"}
            />{" "}
            Звичайна
          </label>
          <label>
            <input
              type="radio"
              name="type"
              value="list"
              onChange={() => setType("list")}
              checked={type === "list"}
            />{" "}
            Список
          </label>
          <label>
            <input
              type="radio"
              name="type"
              value="repeat"
              onChange={() => setType("repeat")}
              checked={type === "repeat"}
            />{" "}
            Повторювана
          </label>
          <label>
            <input
              type="radio"
              name="type"
              value="scale"
              onChange={() => setType("scale")}
              checked={type === "scale"}
            />{" "}
            Шкала
          </label>
        </div>
      </div>

      {type === "repeat" && (
        <div className="repeat-days">
          <label>Дні для занять</label>
          <div className="days">
            {["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд"].map((day) => (
              <div
                key={day}
                className={`day ${days.includes(day) ? "selected" : ""}`}
                onClick={() => toggleDay(day)}
              >
                <p>{day}</p>
              </div>
            ))}
          </div>
        </div>
      )}

      {type === "scale" && (
        <div className="scale-inputs">
          <div className="input-number">
            <label>Ціль</label>
            <input
              type="number"
              placeholder="Введіть число"
              value={scaleGoal}
              onChange={(e) => setScaleGoal(e.target.value)}
            />
          </div>
          <div className="unit">
            <label>Одиниця вимірювання</label>
            <input
              type="text"
              placeholder="Введіть символ або назву"
              value={scaleUnit}
              onChange={(e) => setScaleUnit(e.target.value)}
            />
          </div>
        </div>
      )}

      {type === "list" && (
        <div className="subtasks-block">
          {subtasks.map((subtask, index) => (
            <div className="subtask" key={index}>
              <label>{index + 1}.</label>
              <input
                type="text"
                placeholder="Введіть назву"
                className="title"
                value={subtask.title}
                onChange={(e) => updateSubtask(index, "title", e.target.value)}
              />
              <input
                type="text"
                placeholder="Опис"
                className="description"
                value={subtask.description}
                onChange={(e) =>
                  updateSubtask(index, "description", e.target.value)
                }
              />

              <div className="subtask-type">
                <label>Тип підзадачі</label>
                <div className="types">
                  <label>
                    <input
                      type="radio"
                      name={`subtask-type-${index}`}
                      value="simple"
                      checked={subtask.type === "simple"}
                      onChange={() => updateSubtask(index, "type", "simple")}
                    />{" "}
                    Звичайна
                  </label>
                  <label>
                    <input
                      type="radio"
                      name={`subtask-type-${index}`}
                      value="repeat"
                      checked={subtask.type === "repeat"}
                      onChange={() => updateSubtask(index, "type", "repeat")}
                    />{" "}
                    Повторювана
                  </label>
                  <label>
                    <input
                      type="radio"
                      name={`subtask-type-${index}`}
                      value="scale"
                      checked={subtask.type === "scale"}
                      onChange={() => updateSubtask(index, "type", "scale")}
                    />{" "}
                    Шкала
                  </label>
                </div>
              </div>
              {subtask.type === "repeat" && (
                <div className="repeat-days">
                  <label>Дні для занять</label>
                  <div className="days">
                    {["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд"].map((day) => (
                      <div
                        key={day}
                        className={`day ${
                          subtask.days?.includes(day) ? "selected" : ""
                        }`}
                        onClick={() => {
                          const currentDays = subtask.days || [];
                          const updatedDays = currentDays.includes(day)
                            ? currentDays.filter((d) => d !== day)
                            : [...currentDays, day];
                          updateSubtask(index, "days", updatedDays);
                        }}
                      >
                        <p>{day}</p>
                      </div>
                    ))}
                  </div>
                </div>
              )}

              {subtask.type === "scale" && (
                <div className="scale-inputs">
                  <div className="input-number">
                    <label>Ціль</label>
                    <input
                      type="number"
                      placeholder="Введіть число"
                      value={subtask.scaleGoal || ""}
                      onChange={(e) =>
                        updateSubtask(index, "scaleGoal", e.target.value)
                      }
                    />
                  </div>
                  <div className="unit">
                    <label>Одиниця вимірювання</label>
                    <input
                      type="text"
                      placeholder="Введіть символ або назву"
                      value={subtask.scaleUnit || ""}
                      onChange={(e) =>
                        updateSubtask(index, "scaleUnit", e.target.value)
                      }
                    />
                  </div>
                </div>
              )}
              <button
                className="delete-subtask"
                onClick={() => removeSubtask(index)}
              >
                Видалити
              </button>
            </div>
          ))}
          <button
            className="add-subtask"
            onClick={() =>
              setSubtasks([
                ...subtasks,
                {
                  title: "",
                  description: "",
                  deadline: false,
                  reminder: false,
                  reminderDate: "",
                  reminderTime: "",
                  deadlineDate: "",
                  type: "simple",
                },
              ])
            }
          >
            <span className="circle">+</span>
            <span className="text">Додати підзадачу</span>
          </button>
        </div>
      )}

      <div className="buttons">
        <button onClick={onCancel} className="cancel">
          Скасувати
        </button>
        <button onClick={onSave} className="save">
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
