import React from "react";
import { useState } from "react";
import { Link } from "react-router-dom";
import "../styles/registration.css";
import { useRegistration } from "../hooks/useRegistration";

export function Registration() {
  const [name, setName] = useState("");
  const [surname, setSurname] = useState("");
  const [nickname, setNickname] = useState("");
  const [gender, setGender] = useState("");
  const [birthDate, setBirthDate] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [passwordCheck, setPasswordCheck] = useState("");
  const registration = useRegistration();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await registration(
        name + " " + surname,
        nickname,
        gender,
        birthDate,
        email,
        password
      );
    } catch (err) {
      console.error(err.message);
    }
  };

  return (
    <div className="registration-page">
      <div className="horizontal">
        <h1>Реєстрація</h1>
        <div className="registration-form">
          <div className="column1">
            <label>
              Ім'я
              <input
                type="text"
                name="name"
                placeholder="Лайт"
                pattern="^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\s'\-]{1,50}$"
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
            </label>
            <label>
              Прізвище
              <input
                type="text"
                name="surname"
                placeholder="Ягамі"
                pattern="^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\s'\-]{1,50}$"
                value={surname}
                onChange={(e) => setSurname(e.target.value)}
              />
            </label>
            <label>
              Пошта
              <input
                type="email"
                name="email"
                placeholder="example@gmail.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </label>
            <div className="password">
              <label>
                Пароль
                <input
                  type="password"
                  name="password"
                  placeholder="********"
                  pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
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
                  onChange={(e) => setPasswordCheck(e.target.value)}
                  value={passwordCheck}
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
                  <input
                    type="radio"
                    name="gender"
                    value="F"
                    onChange={(e) => setGender(e.target.value)}
                    checked={gender === "F"}
                  />
                  Жінка
                </label>
                <label className="radiobutton">
                  <input
                    type="radio"
                    name="gender"
                    value="m"
                    onChange={(e) => setGender(e.target.value)}
                    checked={gender === "m"}
                  />
                  Чоловік
                </label>
              </div>
            </div>

            <label>
              Дата народження
              <input
                name="birth-date"
                type="date"
                onChange={(e) => setBirthDate(e.target.value)}
                value={birthDate}
              />
            </label>

            <label>
              І на останок! Як ти хочеш, щоб до тебе звертались однодумці?
              <input
                className="nickname"
                type="text"
                name="nickname"
                placeholder="sigma-killer3000"
                minLength="3"
                maxLength="50"
                pattern="^[a-zA-Z0-9]+$"
                onChange={(e) => setNickname(e.target.value)}
                value={nickname}
              />
            </label>
            <div className="registration-submit">
              <input
                className="submit-button"
                type="submit"
                value="Почати!"
                onClick={handleSubmit}
              />
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
