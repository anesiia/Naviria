import React from "react";
// import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { getUserFriends } from "../services/FriendsServices";
import "../styles/friends.css";

export function Friends() {
  const [activeTab, setActiveTab] = useState("discover"); // "my", "discover", "requests"
  const [friends, setFriends] = useState([]);
  const [requests, setRequests] = useState([]);
  const [discover, setDiscover] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (activeTab === "my") {
          const data = await getUserFriends();
          setFriends(data);
        } else if (activeTab === "requests") {
          setRequests([1, 2]); // Тут має бути getFriendRequests()
        } else if (activeTab === "discover") {
          setDiscover([1, 2, 3]); // Тут має бути getDiscoverUsers()
        }
      } catch (e) {
        console.error("Помилка при завантаженні:", e.message);
      }
    };
    fetchData();
  }, [activeTab]);
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

  const renderActions = () => {
    switch (activeTab) {
      case "my":
        return (
          <div className="actions">
            <button className="remove">Видалити</button>
            <button className="support">Підтримати</button>
          </div>
        );
      case "requests":
        return (
          <div className="actions">
            <button className="reject">Відхилити</button>
            <button className="accept">Прийняти</button>
          </div>
        );
      case "discover":
      default:
        return (
          <div className="actions">
            <button className="add-friend">Додати друга</button>
          </div>
        );
    }
  };
  const renderList = () => {
    const list =
      activeTab === "my"
        ? friends
        : activeTab === "requests"
        ? requests
        : discover;
    return (
      <div className="discover-list">
        {list.map((user, index) => (
          <div className="item" key={index}>
            <img className="avatar" src="Ellipse 19.svg" />
            <div className="info">
              <div className="name-lvl">
                <p className="name">{user.nickname}</p>
                <p className="level">{user.levelInfo.level}</p>
              </div>
              <p className="desc">{user.description || "Опис відсутній"}</p>
              {renderActions()}
            </div>
          </div>
        ))}
      </div>
    );
  };

  const renderMyFriendsList = () => {
    return (
      <div className="friend-list">
        {friends.map((user, index) => (
          <div className="friend" key={index}>
            <div className="info">
              <img className="avatar" src="Ellipse 23.svg" />
              <div className="name">{user.nickname}</div>
            </div>
            <div className="actions">
              <button className="remove">Видалити</button>
              <button className="support">Підтримати</button>
            </div>
          </div>
        ))}
      </div>
    );
  };

  return (
    <div className="friends-page">
      <aside className="side-bar">
        <div className="title">
          <img className="icon" src="friends-icon.svg" />
          <h2 className="title-text">Мої друзі</h2>
        </div>
        <div className="search-box">
          <input type="text" placeholder="Пошук" />
          <button className="search-btn">
            <img src="search.svg" alt="search" />
          </button>
        </div>
        {renderMyFriendsList()}
      </aside>
      <main className="content">
        <div className="tabs">
          <button
            className={activeTab === "my" ? "active" : ""}
            onClick={() => setActiveTab("my")}
          >
            Мої друзі
          </button>
          <button
            className={activeTab === "discover" ? "active" : ""}
            onClick={() => setActiveTab("discover")}
          >
            Знайти друзів
          </button>
          <button
            className={activeTab === "requests" ? "active" : ""}
            onClick={() => setActiveTab("requests")}
          >
            Запити
          </button>
        </div>
        <div className="search">
          <div className="search-box">
            <input type="text" placeholder="Пошук" />
            <button className="search-btn">
              <img src="search.svg" alt="search" />
            </button>
          </div>
          <select className="options" name="options">
            <option value="by-categories">За категоріями</option>
            <option value="alphabetical">За алфввітом</option>
          </select>
        </div>
        {renderList()}
      </main>
    </div>
  );
}
