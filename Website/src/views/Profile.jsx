import React, { useEffect, useState } from "react";
import { getAchievementsByUserId } from "../services/AchievementsServices";
import { Link, useNavigate, useParams } from "react-router-dom";
import { getProfile, getUserById } from "../services/ProfileServices";

import "../styles/profile.css";

export function Profile() {
  const { id } = useParams(); // id undefined → власний профіль
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [allAchievements, setAllAchievements] = useState([]);

  useEffect(() => {
    const fetchAchievements = async () => {
      try {
        const userId = id || localStorage.getItem("id");
        const data = await getAchievementsByUserId(userId);
        setAllAchievements(data);
      } catch {
        setAllAchievements([]);
      }
    };
    fetchAchievements();
  }, [id]);

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const userData = id ? await getUserById(id) : await getProfile();
        setUser(userData);
      } catch (err) {
        console.error(err.message);
        navigate("/login");
      }
    };
    fetchProfile();
  }, [id, navigate]);

  if (!user) return <p>Завантаження…</p>;
  return (
    <div className="profile-page">
      <div className="profile-wrapper">
        <div className="info-box">
          <img
            className="avatar"
            src={
              user.photo && user.photo !== "" ? user.photo : "/Ellipse 20.svg"
            }
            alt={user.nickname}
          />
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
                {user.levelInfo.totalXp}/{user.levelInfo.xpForNextLevel} xp
              </p>
              {!id && (
                <Link to="/edit-profile" className="edit-profile">
                  <img src="/fi-rr-pencil.svg" alt="edit" />
                </Link>
              )}
            </div>
            <p className="description">
              {user.description ||
                "Опису ще нема, але ти можешь додати його у будь-який момент"}
            </p>
          </div>
        </div>
        <div className="achievements">
          <p className="naming">Досягнення</p>
          <div className="ach-list">
            {(user.achievements || []).map((achievement, index) => {
              const details =
                allAchievements.find(
                  (a) => String(a.id) === String(achievement.achievementId)
                ) || {};
              return (
                <div className="achievement" key={index}>
                  <img src="/Ellipse 21.svg" className="pic" />
                  <div className="ach-info">
                    <p className="ach-name">
                      {details.name || achievement.achievementId}
                    </p>
                    <p className="ach-desc">{details.description || ""}</p>
                  </div>
                </div>
              );
            })}
          </div>
        </div>
        <div className="friends">
          <p className="naming">Друзі</p>
          <div className="friends-list">
            {(user.friends || []).map((friend, index) => (
              <div className="friend" key={index}>
                <img
                  className="avatar"
                  src={friend && friend.photo ? friend.photo : "/Ellipse 4.svg"}
                  alt={friend ? friend.nickname : "avatar"}
                />
                <p className="friend-name">{friend.nickname}</p>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
