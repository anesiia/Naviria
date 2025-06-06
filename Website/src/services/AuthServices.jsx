const API_URL = "http://localhost:5186";

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
    throw new Error(data.message || "Сталася помилка при вході");
  }

  localStorage.setItem("token", data.token);

  const payload = parseJwt(data.token);
  if (payload?.sub) {
    localStorage.setItem("id", payload.sub);
  }

  return data;
}

export async function googleLogin(googleToken) {
  const res = await fetch(`${API_URL}/api/Auth/google-login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ token: googleToken }),
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Помилка при вході через Google");
  }

  localStorage.setItem("token", data.token);

  const payload = parseJwt(data.token);
  if (payload?.sub) {
    localStorage.setItem("id", payload.sub);
  }

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
  const res = await fetch(`${API_URL}/api/User`, {
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
  const payload = parseJwt(data.token);
  if (payload?.sub) {
    localStorage.setItem("id", payload.sub); // сохраняем id из токена
  }

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

function parseJwt(token) {
  try {
    const base64Payload = token.split(".")[1];
    const decoded = atob(base64Payload);
    return JSON.parse(decoded);
  } catch {
    return null;
  }
}
