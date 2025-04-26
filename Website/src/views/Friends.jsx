import React from "react";
// import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import {
  getUserFriends,
  getFriendRequests,
  getDiscoverUsers,
  sendFriendRequest,
  //updateFriendRequest,
  deleteFriend,
} from "../services/FriendsServices";
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
          const data = await getFriendRequests();
          setRequests(data);
        } else if (activeTab === "discover") {
          const data = await getDiscoverUsers();
          setDiscover(data);
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

  const renderActions = (friend, tab = activeTab) => {
    switch (tab) {
      case "my":
        return (
          <div className="actions">
            <button
              className="remove"
              onClick={() => handleRemoveFriend(friend.id)}
            >
              Видалити
            </button>
            <button className="support">Підтримати</button>
          </div>
        );
      case "requests":
        return (
          <div className="actions">
            <button
              className="reject"
              //onClick={() => handleUpdateRequest(id, "declined")}
            >
              Відхилити
            </button>
            <button
              className="accept"
              //onClick={() => handleUpdateRequest(id, "accepted")}
            >
              Прийняти
            </button>
          </div>
        );
      case "discover":
      default:
        return (
          <div className="actions">
            <button
              className="add-friend"
              onClick={() => handleSendRequest(friend.id)}
            >
              Додати друга
            </button>
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
        {list.map((friend, index) => (
          <div className="item" key={index}>
            <img className="avatar" src="Ellipse 19.svg" />
            <div className="info">
              <div className="name-lvl">
                <p className="name">{friend.nickname}</p>
                <p className="level">{friend.levelInfo.level} рівень</p>
              </div>
              <p className="desc">{friend.description || "Опис відсутній"}</p>
              {renderActions(friend)}
            </div>
          </div>
        ))}
      </div>
    );
  };

  const handleSendRequest = async (toUserId) => {
    try {
      await sendFriendRequest(toUserId);
      setDiscover((prev) => prev.filter((u) => u.id !== toUserId));
    } catch (e) {
      console.error("Не вдалося надіслати запит:", e.message);
    }
  };

  // const handleUpdateRequest = async (id, status) => {
  //   try {
  //     await updateFriendRequest(id, status);
  //     // опціонально: оновити список запитів після відповіді
  //     setRequests((prev) => prev.filter((r) => r.id !== id));
  //   } catch (e) {
  //     console.error("Не вдалося оновити статус запиту:", e.message);
  //   }
  // };

  const handleRemoveFriend = async (friendId) => {
    try {
      await deleteFriend(friendId);
      setFriends((prev) => prev.filter((f) => f.id !== friendId));
    } catch (e) {
      console.error("Не вдалося видалити друга:", e.message);
    }
  };
  const renderMyFriendsList = () => {
    return (
      <div className="friend-list">
        {friends.map((friend, index) => (
          <div className="friend" key={index}>
            <div className="info">
              <img className="avatar" src="Ellipse 23.svg" />
              <div className="name">{friend.nickname}</div>
            </div>
            {renderActions(friend, "my")}
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
