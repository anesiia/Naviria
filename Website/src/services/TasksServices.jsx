// services/TasksService.jsx
import { authHeaders } from "./AuthServices";

const API_URL = "http://localhost:5186";

export async function updateTask(id, taskData) {
  const res = await fetch(`http://localhost:5186/api/Task/${id}`, {
    method: "PUT",
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
    body: JSON.stringify(taskData),
  });

  // Якщо відповідь пуста (204 або просто без тіла) — не парсимо як JSON
  if (res.status === 204) {
    return;
  }

  const text = await res.text();
  let data = null;
  try {
    data = text ? JSON.parse(text) : null;
  } catch (e) {
    console.error("JSON parse error in updateTask:", e);
    data = null;
  }

  if (!res.ok) {
    throw new Error((data && data.message) || "Не вдалося оновити задачу");
  }

  return data;
}
