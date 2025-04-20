import React from "react";
import { Link } from "react-router-dom";
import { useState } from "react";
import { useLogin } from "../hooks/useLogin";
import "../styles/login.css";

export function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const login = useLogin();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await login(email, password);
    } catch (err) {
      console.error(err.message);
    }
  };

  return (
    <div className="login-page">
      <div className="login">
        <h1>Вітаємо у naviria!</h1>
        <p>Авторизуйтеся у naviria за допомогою облікового запису або Email</p>
        <div className="g-signin2" data-onsuccess="onSignIn"></div>
        <div className="or">
          <img src="Line.svg" />
          <p>або</p>
          <img src="Line.svg" />
        </div>
        <div className="login-form">
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
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </label>
            <button className="show-hide">
              <img src="fi-rr-eye-crossed.svg" />
            </button>
          </div>
        </div>
        <div className="login-submit">
          <input
            className="submit-button"
            type="submit"
            value="Увійти"
            onClick={handleSubmit}
          />
          <Link to="/registration">
            Ще не з нами? <b>Реєстрація</b>
          </Link>
        </div>
      </div>
    </div>
  );
}
