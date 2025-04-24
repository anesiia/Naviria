import React from "react";
// import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import "../styles/friends.css";

export function Friends() {
  const [activeTab, setActiveTab] = useState("discover"); // "my", "discover", "requests"

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
    return (
      <div className="discover-list">
        {[1, 2, 3, 4].map((_, index) => (
          <div className="item" key={index}>
            <img className="avatar" src="Ellipse 19.svg" />
            <div className="info">
              <div className="name-lvl">
                <p className="name">Ім'я</p>
                <p className="level">43 lvl</p>
              </div>
              <p className="desc">
                Body text for whatever you'd like to say. Add main takeaway
                points, quotes, anecdotes, or even a very very short story.
              </p>
              {renderActions()}
            </div>
          </div>
        ))}
      </div>
    );
  };

  const renderMyFriensList = () => {
    return (
      <div className="friend-list">
        {[1, 2, 3, 4].map((_, index) => (
          <div className="friend" key={index}>
            <div className="info">
              <img className="avatar" src="Ellipse 23.svg" />
              <div className="name">Ім'я</div>
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
        {renderMyFriensList()}
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
