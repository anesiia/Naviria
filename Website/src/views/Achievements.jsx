import React from "react";
import {
  getAchievements,
  awardAchievementGift,
} from "../services/AchievementsServices";
import { useEffect, useState } from "react";
import { getProfile } from "../services/ProfileServices";
import "../styles/achievements.css";

const API_URL = "http://localhost:5186";

export function Achievements() {
  const [achievements, setAchievements] = useState([]);
  const [profileAchievements, setProfileAchievements] = useState([]);
  const [loading, setLoading] = useState(true);
  const [awarding, setAwarding] = useState({});
  useEffect(() => {
    const fetchAll = async () => {
      try {
        const data = await getAchievements();
        setAchievements(data);
        const user = await getProfile();
        setProfileAchievements(user.achievements || []);
      } catch (e) {
        console.error(e);
        alert("Помилка при завантаженні досягнень або профілю");
      }
      setLoading(false);
    };
    fetchAll();
  }, []);

  const isGiftReceived = (achievementId) => {
    const userAch = profileAchievements.find(
      (a) => a.achievementId === achievementId
    );
    return userAch ? userAch.isPointsReceived : false;
  };

  const handleAward = async (achievementId) => {
    setAwarding((prev) => ({ ...prev, [achievementId]: true }));
    try {
      await awardAchievementGift(achievementId);
      // Після нагороди — оновити статуси (getProfile)
      const user = await getProfile();
      setProfileAchievements(user.achievements || []);
    } catch (e) {
      alert(e.message);
    }
    setAwarding((prev) => ({ ...prev, [achievementId]: false }));
  };
  if (loading) return <div>Завантаження...</div>;
  return (
    <div className="achievements-page">
      <h1>Твої досягнення</h1>
      <div className="achievements">
        {achievements.map((achievement) => {
          const received = isGiftReceived(achievement.id);
          return (
            <div className="achievement" key={achievement.id}>
              <img className="preview" alt="" />
              <div className="content">
                <p className="name">{achievement.name}</p>
                <p className="desc">{achievement.description}</p>
                <button
                  className={`gift-btn${received ? " awarded" : ""}`}
                  onClick={() => handleAward(achievement.id)}
                  disabled={received || awarding[achievement.id]}
                >
                  {received
                    ? "Отримано"
                    : awarding[achievement.id]
                    ? "..."
                    : "Подарунок"}
                </button>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}
