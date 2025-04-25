import React from "react";
import { getAchievements } from "../services/AchievementsServices";
import { useEffect, useState } from "react";

import "../styles/achievements.css";

const API_URL = "http://localhost:5186";

export function Achievements() {
  const [achievements, setAchievements] = useState([]);
  useEffect(() => {
    const fetchAchievements = async () => {
      try {
        const data = await getAchievements();
        setAchievements(data);
      } catch (e) {
        console.error("Помилка при завантаженні досягнень:", e.message);
      }
    };
    fetchAchievements();
  }, []);

  return (
    <div className="achievements-page">
      <h1>Твої досягнення</h1>
      <div className="achievements">
        {achievements.map((achievement, index) => (
          <div className="achievement" key={index}>
            <img className="preview" />
            <div className="content">
              <p className="name">Назва</p>
              <p className="desc">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce
                vel ornare neque.
              </p>
              <button className="gift-btn">Подарунок</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
