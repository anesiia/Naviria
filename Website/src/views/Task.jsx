import "../styles/task.css";
import { useState } from "react";
import { Subtasks } from "./Subtasks";
export function Task(props) {
  const [isCollapsed, setIsCollapsed] = useState(true);

  return (
    <div className="main-task">
      <div className="main-task-info">
        <div className="name">
          <input type="checkbox" id="tasks" name="tasks"></input>
          <label for="tasks">Task</label>
        </div>
        <div className="buttons">
          <button className="delete">
            <img src="Group 174.svg" />
          </button>
          <button className="edit">
            <img src="fi-rr-pencil.svg" />
          </button>
          <button className="open" onClick={() => setIsCollapsed(!isCollapsed)}>
            <img
              src={isCollapsed ? "fi-rr-caret-down.svg" : "fi-rr-caret-up.svg"}
            />
          </button>
        </div>
      </div>

      <div className="task-main-info">
        {isCollapsed ? null : props.type === "simple" ? (
          <div className="info-task-simple">
            <p className="desc">Опис</p>
            <div className="dates">
              <div className="start-date">
                <p className="name-date">Дата старту</p>
                <p className="value-date">dd/mm/yyyy</p>
              </div>
              <div className="deadline">
                <p className="name-date">Дедлайн</p>
                <p className="value-date">dd/mm/yyyy</p>
              </div>
            </div>
          </div>
        ) : props.type === "repeat" ? (
          <div className="info-task-repeat">
            <div className="info">
              <p>Опис</p>
              <div className="dates">
                <div className="start-date">
                  <p className="name-date">Дата старту</p>
                  <p className="value-date">dd/mm/yyyy</p>
                </div>
                <div className="deadline">
                  <p className="name-date">Дедлайн</p>
                  <p className="value-date">dd/mm/yyyy</p>
                </div>
              </div>
            </div>
            <div className="main-repeat">
              <div className="naming">
                <p>Дні для занять</p>
                <div className="days">
                  <div className="day">
                    <p>Пн</p>
                  </div>
                  <div className="day">
                    <p>Вт</p>
                  </div>
                  <div className="day">
                    <p>Ср</p>
                  </div>
                  <div className="day">
                    <p>Чт</p>
                  </div>
                  <div className="day">
                    <p>Пт</p>
                  </div>
                  <div className="day">
                    <p>Сб</p>
                  </div>
                  <div className="day">
                    <p>Нд</p>
                  </div>
                </div>
              </div>
              <button className="done-btn">Готово</button>
            </div>
          </div>
        ) : props.type === "scale" ? (
          <div className="info-task-scale">
            <div className="bg-gray">
              <div className="desc-scale">
                <p>Опис</p>
                <div className="scale-info">
                  <div className="scale">
                    <div className="color-scale" style={{ width: "60%" }}></div>
                  </div>
                  <p className="points">1488/3469</p>
                </div>
              </div>
              <div className="dates">
                <div className="start-date">
                  <p className="name-date">Дата старту</p>
                  <p className="value-date">dd/mm/yyyy</p>
                </div>
                <div className="deadline">
                  <p className="name-date">Дедлайн</p>
                  <p className="value-date">dd/mm/yyyy</p>
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
              <p>Опис</p>
              <div className="dates">
                <div className="start-date">
                  <p className="name-date">Дата старту</p>
                  <p className="value-date">dd/mm/yyyy</p>
                </div>
                <div className="deadline">
                  <p>Дедлайн</p>
                  <p>dd/mm/yyyy</p>
                </div>
              </div>
            </div>
            <div className="main-list">
              <Subtasks />
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
