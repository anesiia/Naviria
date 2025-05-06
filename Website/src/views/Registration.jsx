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
  const [errors, setErrors] = useState({});
  const registration = useRegistration();

  const isAtLeast18 = (dateString) => {
    const birth = new Date(dateString);
    const today = new Date();
    const age = today.getFullYear() - birth.getFullYear();
    const m = today.getMonth() - birth.getMonth();
    return age > 18 || (age === 18 && m >= 0);
  };

  const validate = () => {
    const newErrors = {};
    if (!name.match(/^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\s'-]{1,20}$/))
      newErrors.name = "Ім'я введено некоректно*";
    if (!surname.match(/^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\s'-]{1,20}$/))
      newErrors.surname = "Прізвище введено некоректно*";
    if (!email.match(/^\S+@\S+\.\S+$/))
      newErrors.email = "Невірний формат пошти*";
    if (!password.match(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/))
      newErrors.password = "Пароль має містити великі та малі літери та цифру*";
    if (password !== passwordCheck)
      newErrors.passwordCheck = "Паролі не збігаються*";
    if (!nickname.match(/^[a-zA-Z0-9]{3,20}$/))
      newErrors.nickname =
        "Нікнейм має містити лише латинські літери та цифри (3-20 символів)*";
    if (!gender) newErrors.gender = "Оберіть стать*";
    if (!birthDate) {
      newErrors.birthDate = "Вкажіть дату народження*";
    } else if (!isAtLeast18(birthDate)) {
      newErrors.birthDate = "Потрібно бути старше 18 років*";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;
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
      if (
        err.message.includes("User already exists") ||
        err.message.includes("409")
      ) {
        setErrors((prev) => ({
          ...prev,
          email: "Користувач з такою поштою або нікнеймом вже існує*",
        }));
      } else {
        console.error("Помилка реєстрації:", err);
      }
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
                pattern="^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\s'\-]{1,20}$"
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
              {errors.name && <span className="error-text">{errors.name}</span>}
            </label>
            <label>
              Прізвище
              <input
                type="text"
                name="surname"
                placeholder="Ягамі"
                pattern="^[a-zA-Zа-яА-ЯёЁіІїЇєЄ\s'\-]{1,20}$"
                value={surname}
                onChange={(e) => setSurname(e.target.value)}
              />
              {errors.surname && (
                <span className="error-text">{errors.surname}</span>
              )}
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
              {errors.email && (
                <span className="error-text">{errors.email}</span>
              )}
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
            {errors.password && (
              <span className="error-text">{errors.password}</span>
            )}
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
            {errors.passwordCheck && (
              <span className="error-text">{errors.passwordCheck}</span>
            )}
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
              {errors.gender && (
                <span className="error-text">{errors.gender}</span>
              )}
            </div>

            <label>
              Дата народження
              <input
                name="birth-date"
                type="date"
                onChange={(e) => setBirthDate(e.target.value)}
                value={birthDate}
              />
              {errors.birthDate && (
                <span className="error-text">{errors.birthDate}</span>
              )}
            </label>

            <label>
              І на останок! Як ти хочеш, щоб до тебе звертались однодумці?
              <input
                className="nickname"
                type="text"
                name="nickname"
                placeholder="sigma-killer3000"
                minLength="3"
                maxLength="20"
                pattern="^[a-zA-Z0-9]+$"
                onChange={(e) => setNickname(e.target.value)}
                value={nickname}
              />
              {errors.nickname && (
                <span className="error-text">{errors.nickname}</span>
              )}
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
