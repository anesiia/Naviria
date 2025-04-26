import { authHeaders } from "./AuthServices";

const API_URL = "http://localhost:5186";

export async function getAchievements() {
  const id = localStorage.getItem("id");

  const res = await fetch(`${API_URL}/api/Achievements/${id}`, {
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
