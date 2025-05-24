import { authHeaders } from "./AuthServices";

const API_URL = "http://localhost:5186";

export async function fetchGroupedTasksByUser() {
  const id = localStorage.getItem("id");
  const res = await fetch(`${API_URL}/api/Task/grouped/user/${id}`, {
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося отримати задачі");
  }

  return data;
}

export async function createFolder(name) {
  const userId = localStorage.getItem("id");
  const res = await fetch("http://localhost:5186/api/Folder", {
    method: "POST",
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      userId,
      name,
    }),
  });

  const data = await res.json();
  if (!res.ok) {
    throw new Error(data.message || "Не вдалося створити папку");
  }
  return data; // Тут можеш повернути, наприклад, нову папку
}

export async function deleteFolder(id) {
  const res = await fetch(`http://localhost:5186/api/Folder/${id}`, {
    method: "DELETE",
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });

  // Бекенд може повертати порожню відповідь, просто перевіримо res.ok
  if (!res.ok) {
    const data = await res.json().catch(() => ({}));
    throw new Error(data.message || "Не вдалося видалити папку");
  }
}
