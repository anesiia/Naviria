import React from "react";
import { Link } from "react-router-dom";
import "../styles/registration.css";

export function Registration() {
  return (
    <div className="registration-page">
      <div className="horizontal">
        <h1>Реєстрація</h1>
        <div className="registration-form">
          <div className="column1">
            <label>
              Ім'я
              <input type="text" name="name" placeholder="Лайт" />
            </label>
            <label>
              Прізвище
              <input type="text" name="surname" placeholder="Ягамі" />
            </label>
            <label>
              Пошта
              <input
                type="email"
                name="email"
                placeholder="example@gmail.com"
              />
            </label>
            <div className="password">
              <label>
                Пароль
                <input type="password" name="password" placeholder="********" />
              </label>
              <button className="show-hide">
                <img src="fi-rr-eye-crossed.svg" />
              </button>
            </div>
            <div className="password">
              <label>
                Підтвердіть пароль
                <input
                  type="password"
                  name="repeat-password"
                  placeholder="********"
                />
              </label>
              <button className="show-hide">
                <img src="fi-rr-eye-crossed.svg" />
              </button>
            </div>
          </div>
          <div className="column2">
            <div className="gender-select">
              <p>Стать</p>
              <div className="selection">
                <label className="radiobutton">
                  <input type="radio" name="gender" value="female" checked />
                  Жінка
                </label>
                <label className="radiobutton">
                  <input type="radio" name="gender" value="male" />
                  Чоловік
                </label>
              </div>
            </div>

            <label>
              Дата народження
              <input name="birth-date" type="date" />
            </label>

            <label>
              І на останок! Як ти хочеш, щоб до тебе звертались однодумці?
              <input
                className="nickname"
                type="text"
                name="nickname"
                placeholder="sigma-killer3000"
              />
            </label>
            <div className="registration-submit">
              <input className="submit-button" type="submit" value="Почати!" />
              <Link to="/login">
                Уже з нами? <b>Увійти</b>
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
