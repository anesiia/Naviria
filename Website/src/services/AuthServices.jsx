// src/services/auth.js

const API_URL = "http://localhost:5186"; // из .env

export async function login(email, password) {
  const res = await fetch(`${API_URL}/api/Auth/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ email, password }),
  });

  const data = await res.json();
  if (!res.ok) {
    // сервер может вернуть { message: '...' }
    throw new Error(data.message || "Сталася помилка при вході");
  }

  // сохраняем токен, чтобы использовать в других запросах
  localStorage.setItem("token", data.token);
  return data;
}

export async function registration(
  fullName,
  nickname,
  gender,
  birthDate,
  email,
  password
) {
  const res = await fetch(`${API_URL}/api/User/add`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      fullName,
      nickname,
      gender,
      birthDate,
      email,
      password,
    }),
  });

  const data = await res.json();
  if (!res.ok) {
    throw new Error(data.message || "Сталася помилка при спробі реєстрації");
  }
  localStorage.setItem("token", data.token);
  return data;
}
/**
 * Удаляет токен и "выходит" из системы.
 */
export function logout() {
  localStorage.removeItem("token");
}

/**
 * Вспомогательная функция — возвращает заголовки с авторизацией.
 */
export function authHeaders() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}
