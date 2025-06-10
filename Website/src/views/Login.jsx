import React from "react";
import { Link } from "react-router-dom";
import { GoogleLogin } from "@react-oauth/google";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { googleLogin } from "../services/AuthServices";
import { useLogin } from "../hooks/useLogin";
import "../styles/login.css";

export function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [showPassword, setShowPassword] = useState(false);

  const login = useLogin();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await login(email, password);
    } catch (err) {
      console.error(err.message);
    }
  };

  const handleGoogleLogin = async (googleToken) => {
    try {
      await googleLogin(googleToken);
      navigate("/profile"); // Переходиш на профіль після входу
    } catch (err) {
      console.error("Помилка входу через Google:", err.message);
    }
  };
  return (
    <div className="login-page">
      <div className="login">
        <h1>Вітаємо у naviria!</h1>
        <p>Авторизуйтеся у naviria за допомогою облікового запису або Email</p>
        <GoogleLogin
          onSuccess={async (credentialResponse) => {
            try {
              await handleGoogleLogin(credentialResponse.credential);
              navigate("/profile"); // перенаправити після успіху
            } catch (err) {
              console.error(err.message);
            }
          }}
          onError={() => {
            console.error("Помилка авторизації через Google");
          }}
        />
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
                type={showPassword ? "text" : "password"}
                name="password"
                placeholder="********"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </label>
            <button
              type="button"
              className="show-hide"
              onClick={() => setShowPassword((prev) => !prev)}
            >
              <img
                src={
                  showPassword
                    ? "eyeball-open-view.svg"
                    : "fi-rr-eye-crossed.svg"
                }
              />
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
