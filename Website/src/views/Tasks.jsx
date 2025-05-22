import "../styles/tasks.css";
import { Task } from "./Task";
import { Subtasks } from "./Subtasks";
export function Tasks() {
  return (
    <div className="tasks-page">
      <div className="side-bar">
        <div className="side-header">
          <div className="info">
            <img src="Ellipse 4.svg" />
            <p className="nickname">Nickname</p>{" "}
          </div>
          <img src="fi-rr-menu-burger.svg" />
        </div>
        <div className="add-folder">
          <button className="add-folder-btn">+</button>
          <p>Створити папку</p>
        </div>
        <div className="folders">
          <div className="folder">
            <div className="head">
              <img src="fi-rr-thumbtack.svg" />
              <p> Folder name</p>
            </div>
            <div className="tasks">
              <div className="task">
                <input type="checkbox" id="tasks" name="tasks"></input>
                <label for="tasks">Task</label>
              </div>
            </div>
          </div>
          <div className="folder">
            <div className="head">
              <img src="fi-rr-thumbtack.svg" />
              <p> Folder name</p>
            </div>
            <div className="tasks">
              <div className="task">
                <input type="checkbox" id="tasks" name="tasks"></input>
                <label for="tasks">Task</label>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="content">
        <h1>Мої задачі</h1>
        <div className="add-task">
          <button className="add-task-btn">+</button>
          <p>Створити нову задачу</p>
        </div>
        <div className="in-progress">
          <h2>В процесі</h2>
          <div className="tasks">
            <Task />
            <div className="task">
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
                <button className="open">
                  <img src="fi-rr-caret-down.svg" />
                </button>
              </div>
            </div>
          </div>
        </div>
        <div className="done">
          <h2>Виконано</h2>
          <div className="tasks">
            <div className="task">
              <div className="name">
                <input type="checkbox" id="tasks" name="tasks"></input>
                <label for="tasks">Task</label>
              </div>
              <div className="buttons">
                <button className="delete">
                  <img src="Group 174.svg" />
                </button>
              </div>
            </div>
          </div>
        </div>
        <h1>Превʼю всіх типів задач</h1>

        <h2>Simple Task</h2>
        <Task type="simple" />

        <h2>Repeat Task</h2>
        <Task type="repeat" />

        <h2>Scale Task</h2>
        <Task type="scale" />

        <h2>List Task (з Subtasks)</h2>
        <Task type="list" />

        <hr />

        <h2>Simple Subtask</h2>
        <Subtasks type="simple" />

        <h2>Repeat Subtask</h2>
        <Subtasks type="repeat" />

        <h2>Scale Subtask</h2>
        <Subtasks type="scale" />
      </div>
    </div>
  );
}
