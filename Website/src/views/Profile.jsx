import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { getProfile } from "../services/ProfileServices";
import "../styles/profile.css";

export function Profile() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    getProfile()
      .then(setUser)
      .catch((err) => {
        console.error(err.message);
        navigate("/login");
      });
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
            <div className="achievement">
              <img src="Ellipse 21.svg" className="pic" />
              <div className="ach-info">
                <p className="ach-name">Lalala</p>
                <p className="ach-desc">Damn bro how...?</p>
              </div>
            </div>
            <div className="achievement">
              <img src="Ellipse 21.svg" className="pic" />
              <div className="ach-info">
                <p className="ach-name">Lalala</p>
                <p className="ach-desc">Damn bro how...?</p>
              </div>
            </div>
            <div className="achievement">
              <img src="Ellipse 21.svg" className="pic" />
              <div className="ach-info">
                <p className="ach-name">Lalala</p>
                <p className="ach-desc">Damn bro how...?</p>
              </div>
            </div>
            <div className="achievement">
              <img src="Ellipse 21.svg" className="pic" />
              <div className="ach-info">
                <p className="ach-name">Lalala</p>
                <p className="ach-desc">Damn bro how...?</p>
              </div>
            </div>
          </div>
        </div>
        <div className="friends">
          <p className="naming">Друзі</p>
          <div className="friends-list">
            <div className="friend">
              <img src="Ellipse 21.svg" className="pic" />
              <p className="friend-name">Alpha</p>
            </div>
            <div className="friend">
              <img src="Ellipse 21.svg" className="pic" />
              <p className="friend-name">Alpha</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
