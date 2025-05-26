import React, { useState, useEffect } from "react";
import {
  fetchPersonalStats,
  fetchFriendsStats,
  fetchGlobalStats,
  fetchTotalUsersCount,
  fetchTotalTasksCount,
  fetchCompletedTasksPercentage,
  fetchDaysSinceRegistration,
  fetchDaysSinceBirthday,
  fetchCompletedTasksMonthly,
  fetchFriendsCompletedTasksMonthly,
  fetchGlobalCompletedTasksMonthly,
} from "../services/StatisticsServices";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip as ReTooltip,
  Legend as ReLegend,
  ResponsiveContainer,
} from "recharts";

import { PieChart, Pie, Cell, Tooltip, Legend } from "recharts";
import "../styles/statistics.css";

const COLORS = [
  "#0088FE",
  "#00C49F",
  "#FFBB28",
  "#FF8042",
  "#8884d8",
  "#82ca9d",
  "#a4de6c",
  "#d0ed57",
  "#ffc658",
  "#d88884",
  "#84d8d8",
  "#d884d8",
];
const COLORS_LINE = {
  personal: "#0088FE", // синій
  friends: "#00C49F", // бірюзовий
  global: "#FF8042", // помаранчевий
};

export function Statistics() {
  const [userId, setUserId] = useState(null);
  const [mode, setMode] = useState("personal"); // personal, friends, global
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [generalStats, setGeneralStats] = useState({
    usersCount: null,
    tasksCount: null,
    completedTasksPercent: null,
    daysSinceRegistration: null,
    daysSinceBirthday: null,
  });

  const [lineData, setLineData] = useState([]);

  useEffect(() => {
    const id = localStorage.getItem("id");
    setUserId(id);
  }, []);

  // Завантаження лінійної статистики
  useEffect(() => {
    if (!userId) return;

    setLoading(true);
    setError(null);

    const fetchAll = async () => {
      try {
        // Паралельно завантажуємо всі 3 масиви
        const [personalData, friendsData, globalData] = await Promise.all([
          fetchCompletedTasksMonthly(userId),
          fetchFriendsCompletedTasksMonthly(userId),
          fetchGlobalCompletedTasksMonthly(),
        ]);

        // Функція для перетворення у формат { YYYY-MM: completedCount }
        const formatData = (arr) =>
          arr.reduce((acc, { year, month, completedCount }) => {
            const key = `${year}-${month.toString().padStart(2, "0")}`;
            acc[key] = completedCount;
            return acc;
          }, {});

        const personalMap = formatData(personalData);
        const friendsMap = formatData(friendsData);
        const globalMap = formatData(globalData);

        // Збираємо унікальні місяці по всіх трьох наборах
        const allMonths = Array.from(
          new Set([
            ...Object.keys(personalMap),
            ...Object.keys(friendsMap),
            ...Object.keys(globalMap),
          ])
        ).sort();

        // Формуємо масив об’єктів для графіка
        const mergedData = allMonths.map((month) => ({
          monthYear: month,
          personal: personalMap[month] || 0,
          friends: friendsMap[month] || 0,
          global: globalMap[month] || 0,
        }));

        setLineData(mergedData);
      } catch (err) {
        setError(err.message || "Error fetching line chart data");
      } finally {
        setLoading(false);
      }
    };

    fetchAll();
  }, [userId]);

  useEffect(() => {
    if (!userId) return;

    async function fetchGeneral() {
      try {
        const [
          usersCountRes,
          tasksCountRes,
          completedTasksPercentRes,
          daysSinceRegistrationRes,
          daysSinceBirthdayRes,
        ] = await Promise.all([
          fetchTotalUsersCount(),
          fetchTotalTasksCount(),
          fetchCompletedTasksPercentage(),
          fetchDaysSinceRegistration(userId),
          fetchDaysSinceBirthday(),
        ]);

        setGeneralStats({
          totalUsers: usersCountRes.totalUsers ?? usersCountRes,
          totalTasks: tasksCountRes.totalTasks ?? tasksCountRes,
          completedPercentage:
            completedTasksPercentRes.completedPercentage ??
            completedTasksPercentRes,
          daysSinceRegistration:
            daysSinceRegistrationRes.daysSinceRegistration ??
            daysSinceRegistrationRes,
          daysSinceBirthday:
            daysSinceBirthdayRes.daysSinceBirthday ?? daysSinceBirthdayRes,
        });
      } catch (err) {
        console.error("Failed to fetch general stats", err);
      }
    }

    fetchGeneral();
  }, [userId]);

  // Зчитуємо userId з localStorage при завантаженні компонента
  useEffect(() => {
    const id = localStorage.getItem("id");
    setUserId(id);
  }, []);

  useEffect(() => {
    if (!userId) return; // Чекаємо, поки userId стане доступним

    setLoading(true);
    setError(null);

    const fetchData = async () => {
      try {
        let result = [];
        if (mode === "personal") {
          result = await fetchPersonalStats(userId);
        } else if (mode === "friends") {
          result = await fetchFriendsStats(userId);
        } else if (mode === "global") {
          result = await fetchGlobalStats();
        }

        const chartData = result.map(({ categoryName, value }) => ({
          name: categoryName,
          value,
        }));

        setData(chartData);
      } catch (err) {
        setError(err.message || "Error fetching stats");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [mode, userId]);

  if (!userId) return <p>Завантаження користувача...</p>;

  return (
    <div className="stats-page">
      <div className="cat-stats">
        <div className="title-buttons">
          <h2>Статистика виконання цілей за категоріями</h2>
          <p>
            Тут ти побачиш, скільки відсотків усіх твоїх задач припадає на
            "Навчання", скільки на "Спорт", а скільки були створеними завдяки
            ассистенту.
          </p>
          <p>
            Але це ще не все! Цікаво, що планують твої друзі? А всі користувачі
            разом? Ти можеш порівняти свою статистику з ними і зрозуміти, хто
            тут справжній гуру продуктивності!
          </p>
          <div className="actions">
            <button
              onClick={() => setMode("personal")}
              disabled={mode === "personal"}
            >
              Особиста
            </button>
            <button
              onClick={() => setMode("friends")}
              disabled={mode === "friends"}
            >
              Друзі
            </button>
            <button
              onClick={() => setMode("global")}
              disabled={mode === "global"}
            >
              Всі користувачі
            </button>
          </div>
        </div>

        {loading && <p>Завантаження...</p>}
        {error && <p style={{ color: "red" }}>{error}</p>}

        {!loading && !error && data.length > 0 && (
          <PieChart width={560} height={400}>
            <Pie
              data={data}
              dataKey="value"
              nameKey="name"
              cx="50%"
              cy="50%"
              outerRadius={120}
              label
            >
              {data.map((entry, index) => (
                <Cell
                  key={`cell-${index}`}
                  fill={COLORS[index % COLORS.length]}
                />
              ))}
            </Pie>
            <Tooltip />
            <Legend verticalAlign="middle" layout="vertical" align="right" />
          </PieChart>
        )}

        {!loading && !error && data.length === 0 && <p>Дані відсутні</p>}
      </div>
      <div className="general-stats">
        <div className="cards-container">
          <div className="stat-card">
            <h4>Користувачів</h4>
            <p>{generalStats.totalUsers ?? "..."}</p>
          </div>
          <div className="stat-card">
            <h4>Всього задач</h4>
            <p>{generalStats.totalTasks ?? "..."}</p>
          </div>
          <div className="stat-card">
            <h4>З них виконано</h4>
            <p>
              {generalStats.completedPercentage
                ? `${generalStats.completedPercentage}%`
                : "..."}
            </p>
          </div>
          <div className="stat-card">
            <h4>Ви з нами вже</h4>
            <p>{generalStats.daysSinceRegistration ?? "..."} днів</p>
          </div>
          <div className="stat-card">
            <h4>Ми існуємо</h4>
            <p>{generalStats.daysSinceBirthday ?? "..."} днів</p>
          </div>
        </div>
      </div>
      <div className="data-stats">
        {loading && <p>Завантаження...</p>}
        {error && <p style={{ color: "red" }}>{error}</p>}

        {!loading && !error && lineData.length > 0 && (
          <ResponsiveContainer width="100%" height={300}>
            <LineChart
              data={lineData}
              margin={{ top: 20, right: 90, left: 0, bottom: 5 }}
            >
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="monthYear" />
              <YAxis allowDecimals={false} />
              <ReTooltip />
              <ReLegend />
              <Line
                type="monotone"
                dataKey="personal"
                stroke={COLORS_LINE.personal}
                strokeWidth={3}
                name="Ти"
                activeDot={{ r: 8 }}
              />
              <Line
                type="monotone"
                dataKey="friends"
                stroke={COLORS_LINE.friends}
                strokeWidth={3}
                name="Друзі"
              />
              <Line
                type="monotone"
                dataKey="global"
                stroke={COLORS_LINE.global}
                strokeWidth={3}
                name="Всі користувачі"
              />
            </LineChart>
          </ResponsiveContainer>
        )}

        {!loading && !error && lineData.length === 0 && <p>Дані відсутні</p>}
        <div className="title-desc">
          <h2>Статистика виконаних задач за місяцями</h2>
          <p>
            A тут ти можеш легко порівняти, як ти виконуєш задачі самостійно,
            разом із друзями та серед усіх користувачів. Кожна лінія на графіку
            — це твій прогрес за місяцями. Спостерігай за своїми успіхами,
            помічай активні періоди та надихайся ставити нові цілі! Ти на
            правильному шляху — рухайся вперед і досягай більше!
          </p>
        </div>
      </div>
    </div>
  );
}
