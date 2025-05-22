import "../styles/subtask.css";
export function Subtasks(props) {
  return (
    <div className="subtask">
      {props.type === "simple" ? (
        <div className="info-subtask-simple">
          <div className="subtask-name">
            <input type="checkbox" id="subtask" name="subtask"></input>
            <label for="subtask">Task</label>
          </div>
        </div>
      ) : props.type === "repeat" ? (
        <div className="info-subtask-repeat">
          <div className="subtask-name">
            <input type="checkbox" id="subtask" name="subtask"></input>
            <label for="subtask">Task</label>
          </div>
          <div className="values-repeat">
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
      ) : (
        <div className="info-subtask-scale">
          <div className="subtask-name">
            <input type="checkbox" id="subtask" name="subtask"></input>
            <label for="subtask">Task</label>
          </div>
          <div className="values">
            <div className="add-value">
              <p>Додати значення</p>
              <div className="add-action">
                <input type="number" placeholder="Введіть значення"></input>
                <button className="add-value-btn">Додати</button>
              </div>
            </div>
            <div className="scale-info">
              <div className="scale">
                <div className="color-scale" style={{ width: "60%" }}></div>
              </div>
              <p className="points">1488/3469</p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
