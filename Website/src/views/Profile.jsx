import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { getUserFriends } from "../services/FriendsServices";
import { getAchievements } from "../services/AchievementsServices";
import { getProfile } from "../services/ProfileServices";
import "../styles/profile.css";

export function Profile() {
  const [user, setUser] = useState(null);
  const [friends, setFriends] = useState([]);
  const [achievements, setAchievements] = useState([]);

  const navigate = useNavigate();

  useEffect(() => {
    getProfile()
      .then(setUser)
      .catch((err) => {
        console.error(err.message);
        navigate("/login");
      });
  }, []);

  useEffect(() => {
    const fetchFriends = async () => {
      try {
        const data = await getUserFriends();
        setFriends(data);
      } catch (e) {
        console.error("Помилка при завантаженні друзів:", e.message);
      }
    };
    fetchFriends();
  }, []);

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

  if (!user) return <p>Завантаження…</p>;
  return (
    <div className="profile-page">
      <div className="profile-wrapper">
        <div className="info-box">
          <img className="avatar" src="Ellipse 20.svg" />
          <div className="personal-info">
            <p className="name">{user.nickname}</p>
            <div className="level-info">
              <p className="bold">lvl {user.levelInfo.level}</p>
              <div className="scale">
                <div
                  className="color-scale"
                  style={{ width: `${user.levelInfo.progress * 100}%` }}
                ></div>
              </div>
              <p className="bold">
                {user.levelInfo.xpForNextLevel}/{user.levelInfo.totalXp} xp
              </p>
            </div>
            <p className="description">{user.description}</p>
          </div>
        </div>
        <div className="progress-info">
          <p className="naming">Особистий прогрес</p>
          <div className="scales">
            <p className="scale-naming">League of legends rank</p>
            <div className="scale-info">
              <div className="scale">
                <div className="color-scale" style={{ width: "60%" }}></div>
              </div>
              <p className="points">1488/3469</p>
            </div>
          </div>
        </div>
        <div className="achievements">
          <p className="naming">Досягнення</p>
          <div className="ach-list">
            {achievements.map((achievement, index) => (
              <div className="achievement" key={index}>
                <img src="Ellipse 21.svg" className="pic" />
                <div className="ach-info">
                  <p className="ach-name">{achievement.name}</p>
                  <p className="ach-desc">{achievement.description}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
        <div className="friends">
          <p className="naming">Друзі</p>

          <div className="friends-list">
            {friends.map((friend, index) => (
              <div className="friend" key={index}>
                <img src="Ellipse 21.svg" className="pic" />
                <p className="friend-name">{friend.nickname}</p>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
