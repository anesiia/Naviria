import { authHeaders } from "./AuthServices";
const API_URL = "http://localhost:5186";

export async function getNotifications() {
  const id = localStorage.getItem("id");

  const res = await fetch(`${API_URL}/api/Notification/user/${id}`, {
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося отримати повідомлення");
  }

  return data;
}

export async function markAllNotificationsRead() {
  const id = localStorage.getItem("id");

  await fetch(`${API_URL}/api/Notification/user/${id}/mark-all-read`, {
    method: "PUT",
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });
}
