import { authHeaders } from "./AuthServices";

const API_URL = "http://localhost:5186";

export async function getAchievements() {
  const userId = localStorage.getItem("id");

  const res = await fetch(`${API_URL}/api/Achievements/user/${userId}`, {
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

export async function awardAchievementGift(achievementId) {
  const userId = localStorage.getItem("id");
  const res = await fetch(
    `${API_URL}/api/Achievements/${userId}/award-achievement-points/${achievementId}`,
    {
      method: "PUT",
      headers: {
        ...authHeaders(),
      },
    }
  );

  if (!res.ok) throw new Error("Не вдалося отримати подарунок");
  // Сервер може повернути пустий body
  return res.text();
}
