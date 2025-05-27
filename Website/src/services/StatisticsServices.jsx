// src/services/StatisticsServices.jsx
const API_URL = "http://localhost:5186";

export async function fetchPersonalStats(userId) {
  const res = await fetch(
    `${API_URL}/api/StatisticByCategory/user/${userId}/piechart`
  );
  if (!res.ok) throw new Error("Failed to fetch personal stats");
  return res.json();
}

export async function fetchFriendsStats(userId) {
  const res = await fetch(
    `${API_URL}/api/StatisticByCategory/user/${userId}/friends/piechart`
  );
  if (!res.ok) throw new Error("Failed to fetch friends stats");
  return res.json();
}

export async function fetchGlobalStats() {
  const res = await fetch(`${API_URL}/api/StatisticByCategory/global/piechart`);
  if (!res.ok) throw new Error("Failed to fetch global stats");
  return res.json();
}

export async function fetchTotalUsersCount() {
  const res = await fetch(`${API_URL}/api/StatisticGeneral/users/count`);
  if (!res.ok) throw new Error("Failed to fetch total users count");
  return res.json();
}

export async function fetchTotalTasksCount() {
  const res = await fetch(`${API_URL}/api/StatisticGeneral/tasks/count`);
  if (!res.ok) throw new Error("Failed to fetch total tasks count");
  return res.json();
}

export async function fetchCompletedTasksPercentage() {
  const res = await fetch(
    `${API_URL}/api/StatisticGeneral/tasks/completed-percentage`
  );
  if (!res.ok) throw new Error("Failed to fetch completed tasks percentage");
  return res.json();
}

export async function fetchDaysSinceRegistration(userId) {
  const res = await fetch(
    `${API_URL}/api/StatisticGeneral/users/${userId}/days-since-registration`
  );
  if (!res.ok) throw new Error("Failed to fetch days since registration");
  return res.json();
}

export async function fetchDaysSinceBirthday() {
  const res = await fetch(
    `${API_URL}/api/StatisticGeneral/days-since-birthday`
  );
  if (!res.ok) throw new Error("Failed to fetch days since birthday");
  return res.json();
}

export async function fetchCompletedTasksMonthly(userId) {
  const res = await fetch(
    `${API_URL}/api/StatisticsTaskByDate/user/${userId}/completed/monthly`
  );
  if (!res.ok) throw new Error("Failed to fetch completed tasks monthly");
  return res.json();
}

export async function fetchFriendsCompletedTasksMonthly(userId) {
  const res = await fetch(
    `${API_URL}/api/StatisticsTaskByDate/user/${userId}/friends/completed/monthly`
  );
  if (!res.ok)
    throw new Error("Failed to fetch friends completed tasks monthly");
  return res.json();
}

export async function fetchGlobalCompletedTasksMonthly() {
  const res = await fetch(
    `${API_URL}/api/StatisticsTaskByDate/global/completed/monthly`
  );
  if (!res.ok)
    throw new Error("Failed to fetch global completed tasks monthly");
  return res.json();
}

export async function fetchLeaderboardTop() {
  const res = await fetch(`${API_URL}/api/Leaderboard/top`);
  if (!res.ok) throw new Error("Failed to fetch leaderboard data");
  return res.json();
}
