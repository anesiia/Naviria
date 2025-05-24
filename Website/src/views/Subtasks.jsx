import "../styles/subtask.css";
export function Subtasks(props) {
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
            <button className="done-btn">Готово</button>
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
                <input type="number" placeholder="Введіть значення"></input>
                <button className="add-value-btn">Додати</button>
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
              </p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
