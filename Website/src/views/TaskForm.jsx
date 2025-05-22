import { useState } from "react";
import "../styles/taskForm.css"; // окремо зроби файл стилів

export function TaskForm({ onCancel, onSave }) {
  const [type, setType] = useState("simple");
  const [showDeadline, setShowDeadline] = useState(false);
  const [showReminder, setShowReminder] = useState(false);
  const [showProgress, setShowProgress] = useState(false);
  const [tags, setTags] = useState([]);
  const [newTag, setNewTag] = useState("");

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
