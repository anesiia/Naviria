// services/TasksService.jsx
import { authHeaders } from "./AuthServices";

const API_URL = "http://localhost:5186";

export async function updateTask(id, taskData) {
  const res = await fetch(`${API_URL}/api/Task/${id}`, {
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

export async function updateSubtask(taskId, subtaskId, subtaskData) {
  const res = await fetch(
    `${API_URL}/api/tasks/${taskId}/subtasks/${subtaskId}`,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        // додай авторизацію, якщо потрібно
      },
      body: JSON.stringify(subtaskData),
    }
  );

  if (res.status === 204) return;
  const text = await res.text();
  let data = null;
  try {
    data = text ? JSON.parse(text) : null;
  } catch {
    data = null;
  }
  if (!res.ok) {
    throw new Error((data && data.message) || "Не вдалося оновити підзадачу");
  }
  return data;
}

export async function checkinRepeatableTask(taskId, date) {
  return fetch(`${API_URL}/api/Task/${taskId}/checkin`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      // додай авторизацію, якщо потрібно
    },
    body: JSON.stringify({ date }),
  }).then((res) => {
    if (!res.ok) throw new Error("Check-in failed");
    return res.json().catch(() => ({}));
  });
}

export async function checkinRepeatableSubtask(taskId, subtaskId, date) {
  return fetch(`${API_URL}/api/tasks/${taskId}/subtasks/${subtaskId}/checkin`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ date }),
  }).then((res) => {
    if (!res.ok) throw new Error("Check-in failed");
    return res.json().catch(() => ({}));
  });
}

export async function createTask(data) {
  const res = await fetch(`${API_URL}/api/Task`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error("Не вдалося створити задачу");
  return await res.json();
}

export async function createSubtask(taskId, data) {
  const res = await fetch(`${API_URL}/api/tasks/${taskId}/subtasks`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error("Не вдалося створити підзадачу");
  return await res.json();
}

export async function fetchCategories() {
  const res = await fetch(`${API_URL}/api/Category`);
  if (!res.ok) throw new Error("Не вдалося отримати категорії");
  return await res.json();
}

export async function deleteTask(id) {
  const res = await fetch(`${API_URL}/api/Task/${id}`, {
    method: "DELETE",
  });
  if (!res.ok) throw new Error("Не вдалося видалити задачу");
}
