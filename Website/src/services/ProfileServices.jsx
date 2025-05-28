import { authHeaders } from "./AuthServices";

const API_URL = "http://localhost:5186";

export async function getProfile() {
  const id = localStorage.getItem("id");

  const res = await fetch(`${API_URL}/api/User/${id}`, {
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося отримати профіль");
  }

  return data;
}

// Оновлення даних профілю (крім фото)
export async function updateProfile(data) {
  const id = localStorage.getItem("id");
  const res = await fetch(`${API_URL}/api/User/${id}`, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json",
      ...authHeaders(),
    },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error("Failed to update profile");

  // Виправлення: Перевіряємо, чи є тіло
  const text = await res.text();
  if (!text) return null; // або return {}
  try {
    return JSON.parse(text);
  } catch {
    return null;
  }
}

// Завантаження фото
export async function uploadProfilePhoto(file) {
  const id = localStorage.getItem("id");
  const formData = new FormData();
  formData.append("file", file);

  const res = await fetch(`${API_URL}/api/User/${id}/upload-profile-photo`, {
    method: "POST",
    headers: {
      ...authHeaders(),
      // Не став Content-Type — браузер його поставить автоматично для multipart/form-data
    },
    body: formData,
  });

  if (!res.ok) throw new Error("Failed to upload photo");
  return res.json();
}
